﻿using Domain.Entities;
using Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Events
{
    public interface IEventRepository
    {
        Task<Event?> AddEventAsync(Event newEvent);

        Task<int> CountAllAsync();

        Task<List<Event>> GetAllEventAsync(int pageNumber, int pageSize);
        Task<List<Event>> GetAllUrgentEventAsync(int pageNumber, int pageSize);
        Task<List<Event>> GetAllNormalEventAsync(int pageNumber, int pageSize);
        Task<Event?> GetEventByIdAsync(int eventId);

        Task<Event> UpdateEventAsync(Event updateEvent);
    }
}
