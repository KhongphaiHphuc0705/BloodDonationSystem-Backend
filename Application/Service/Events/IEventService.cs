﻿using Application.DTO.EventsDTO;
using Domain.Entities;
using Infrastructure.Helper;

namespace Application.Service.Events
{
    public interface IEventService
    {
        Task<Event?> AddEventAsync(EventDTO eventRequest);
        Task<Event?> AddUrgentEventAsync(EventDTO eventRequest);

        Task<PaginatedResult<EventDTO>> GetAllEventAsync(int pageNumber, int pageSize);
        Task<Event?> GetEventByIdAsync(int eventId);

        Task<EventDTO> UpdateEventAsync(int eventId, EventDTO updateEvent);

        Task<Event> DeleteEventAsync(int eventId);
    }
}
