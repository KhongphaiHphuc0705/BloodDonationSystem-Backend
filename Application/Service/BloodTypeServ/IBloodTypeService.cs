﻿using Application.DTO.BloodTypeDTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.BloodTypeServ
{
    public interface IBloodTypeService
    {
        Task<List<BloodTypeDTO>> GetAllBloodTypesAsync();
    }
}
