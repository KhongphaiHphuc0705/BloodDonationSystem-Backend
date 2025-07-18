﻿using Application.DTO.EventsDTO;
using Domain.Entities;
using Infrastructure.Helper;

namespace Application.Service.Events
{
    public interface IEventService
    {
        Task<Event?> AddEventAsync(NormalEventDTO eventRequest);
        Task<Event?> AddUrgentEventAsync(UrgentEventDTO eventRequest);

        Task<PaginatedResult<EventDTO>> GetAllEventAsync(int pageNumber, int pageSize);
        Task<Event?> GetEventByIdAsync(int eventId);

        Task<EventDTO> UpdateEventAsync(int eventId, EventDTO updateEvent);

        Task<Event> DeleteEventAsync(int eventId);

        Task<int> ExpireEventsAsync();

        Task<PaginatedResult<EventDTO>> SearchEventByDayAsync(int pageNumber, int pageSize, DateOnly? startDay, DateOnly? endDay);

        Task<PaginatedResultWithEventTime<ListWaiting>> GetPassedHealthProcedureAsync(int pageNumber, int pageSize);
        Task<PaginatedResultWithEventTime<ListWaiting>> GetEventListDoBloodProcedure(int pageNumber, int pageSize);
    }
}
