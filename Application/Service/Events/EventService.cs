﻿using Application.DTO.EventsDTO;
using Domain.Entities;
using Infrastructure.Helper;
using Infrastructure.Repository.Blood;
using Infrastructure.Repository.BloodRegistrationRepo;
using Infrastructure.Repository.Events;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Service.Events
{
    public class EventService(IEventRepository _eventRepository, 
                            IHttpContextAccessor _contextAccessor,
                            IBloodTypeRepository _bloodRepository,
                            IBloodRegistrationRepository _bloodRegisRepo) : IEventService
    {
        public async Task<Event?> AddEventAsync(NormalEventDTO eventRequest)
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid creatorId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            var events = new Event
            {
                Title = eventRequest.Title,
                MaxOfDonor = eventRequest.MaxOfDonor,
                EstimatedVolume = eventRequest.EstimatedVolume,
                CreateAt = DateTime.Now,
                EventTime = eventRequest.EventTime,
                IsUrgent = false,
                IsExpired = false,
                CreateBy = creatorId,
                FacilityId = 1
            };
            await _eventRepository.AddEventAsync(events);
            return events;
        }

        public async Task<Event?> AddUrgentEventAsync(UrgentEventDTO eventRequest)
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid creatorId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            var bloodType = await _bloodRepository.GetBloodTypeByIdAsync(eventRequest.BloodTypeId);

            var events = new Event
            {
                Title = eventRequest.Title,
                MaxOfDonor = eventRequest.MaxOfDonor,
                EstimatedVolume = eventRequest.EstimatedVolume,
                BloodTypeId = bloodType.Id,
                BloodComponent = eventRequest.BloodComponent,
                CreateAt = DateTime.Now,
                EventTime = eventRequest.EventTime,
                IsUrgent = true,
                IsExpired = false,
                CreateBy = creatorId,
                FacilityId = 1
            };
            await _eventRepository.AddEventAsync(events);
            return events;
        }

        public async Task<Event> DeleteEventAsync(int eventId)
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid updaterId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            var existEvent = await _eventRepository.GetEventByIdAsync(eventId);
            if (existEvent == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            }

            existEvent.UpdateBy = updaterId; // Set the updater ID
            existEvent.UpdateAt = DateTime.Now; // Update the timestamp
            existEvent.IsExpired = true; // Update the expired status
            await _eventRepository.UpdateEventAsync(existEvent);
            return existEvent;
        }

        public async Task<int> ExpireEventsAsync()
        {
            return await _eventRepository.EventExpiredAsync();
        }

        public async Task<PaginatedResult<EventDTO>> GetAllEventAsync(int pageNumber, int pageSize)
        {
            var userRole = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

            int totalItems;
            IEnumerable<Event> events;

            if (userRole == "Staff")
            {
                totalItems = await _eventRepository.CountAllEventAsync();
                events = await _eventRepository.GetAllEventAsync(pageNumber, pageSize);
            }
            else
            {
                totalItems = await _eventRepository.CountAllActiveEventAsync();
                events = await _eventRepository.GetAllActiveEventAsync(pageNumber, pageSize);
            }

            var eventDTOs = events.Select(e => new EventDTO
            {
                Id = e.Id,
                Title = e.Title,
                MaxOfDonor = e.MaxOfDonor,
                EstimatedVolume = e.EstimatedVolume,
                EventTime = e.EventTime,
                IsUrgent = e.IsUrgent,
                BloodType = e.BloodType?.Type,
                BloodComponent = e.BloodComponent?.ToString(),
                BloodRegisCount = _bloodRegisRepo.GetAllAsync().Result
                                        .Where(br => br.EventId == e.Id).Count()
            }).ToList();

            
            return new PaginatedResult<EventDTO>
            {
                Items = eventDTOs,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public Task<Event?> GetEventByIdAsync(int eventId)
        {
            var eventItem = _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            }

            return eventItem;
        }

        public async Task<PaginatedResultWithEventTime<ListWaiting>> GetEventListDoBloodProcedure(int pageNumber, int pageSize)
        {
            var events = await _eventRepository.GetEventListDoBloodProcedure(pageNumber, pageSize);
            var totalItems = await _eventRepository.CountEventListDoBloodProcedure();
            var eventTime = events.FirstOrDefault()?.EventTime;

            var dto = events.Select(e => new ListWaiting
            {
                Id = e.Id,
                Name = e.Title,
                Total = e.BloodRegistrations.Count,
            }).Where(e => e.Total > 0)
              .ToList();

            return new PaginatedResultWithEventTime<ListWaiting>
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                EventTime = eventTime,
                Items = dto,
            };
        }

        public async Task<PaginatedResultWithEventTime<ListWaiting>> GetPassedHealthProcedureAsync(int pageNumber, int pageSize)
        {
            var events = await _eventRepository.GetPassedHealthProcedureAsync(pageNumber, pageSize);
            var totalItems = await _eventRepository.CountEventPassedHealthProcedureAsync();
            var eventTime = events.FirstOrDefault()?.EventTime;

            var dto = events.Select(e => new ListWaiting
            {
                Id = e.Id,
                Name = e.Title,
                Total = e.BloodRegistrations.Count,
            }).Where(e => e.Total > 0)
              .ToList();

            return new PaginatedResultWithEventTime<ListWaiting>
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                EventTime = eventTime,
                Items = dto,
            };
        }

        public async Task<PaginatedResult<EventDTO>> SearchEventByDayAsync(int pageNumber, int pageSize, DateOnly? startDay, DateOnly? endDay)
        {
            var events = await _eventRepository.SearchEventByDayAsync(pageNumber, pageSize, startDay, endDay);
            var total = await _eventRepository.CountEventFromDayToDay(startDay, endDay);

            if (!events.Any())
            {
                return null;
            }

            var dto = events.Select(e => new EventDTO
            {
                Id = e.Id,
                Title = e.Title,
                MaxOfDonor = e.MaxOfDonor,
                EventTime = e.EventTime,
                BloodType = e.BloodType?.Type,
                BloodComponent = e.BloodComponent?.ToString(),
                EstimatedVolume = e.EstimatedVolume,
                IsUrgent = e.IsUrgent,
                BloodRegisCount = e.BloodRegistrations.Count(),
            }).ToList();

            return new PaginatedResult<EventDTO>
            {
                TotalItems = total,
                Items = dto,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<EventDTO> UpdateEventAsync(int eventId, EventDTO updateEvent)
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid updaterId))
            {
                throw new UnauthorizedAccessException("User not found or invalid");
            }

            var existEvent = await _eventRepository.GetEventByIdAsync(eventId);
            if (existEvent == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            }
            existEvent.Title = updateEvent.Title;
            existEvent.MaxOfDonor = updateEvent.MaxOfDonor;
            existEvent.EstimatedVolume = updateEvent.EstimatedVolume;
            existEvent.EventTime = updateEvent.EventTime;
            existEvent.IsUrgent = updateEvent.IsUrgent;
            existEvent.UpdateAt = DateTime.Now;
            existEvent.IsExpired = existEvent.IsExpired; // Keep original expired status
            //existEvent.BloodTypeId = updateEvent.BloodTypeId; // Update blood type if provided
            //existEvent.BloodComponent = updateEvent.BloodComponent.; // Update blood component if provided
            existEvent.UpdateBy = updaterId; // Set the updater ID

            await _eventRepository.UpdateEventAsync(existEvent);
            return new EventDTO
            {
                Title = existEvent.Title,
                MaxOfDonor = existEvent.MaxOfDonor,
                EstimatedVolume = existEvent.EstimatedVolume,
                EventTime = existEvent.EventTime,
                IsUrgent = existEvent.IsUrgent,
                //    IsExpired = existEvent.IsExpired,
                //BloodType = existEvent.BloodTypeId, // Include blood type if available
                BloodComponent = existEvent.BloodComponent.ToString() // Include blood component if available
            };
        }
    }
}
