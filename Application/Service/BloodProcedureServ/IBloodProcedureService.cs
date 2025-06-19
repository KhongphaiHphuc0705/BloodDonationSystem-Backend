using Application.DTO.BloodProcedureDTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.BloodProcedureServ
{
    public interface IBloodProcedureService
    {
        Task<BloodProcedure?> RecordBloodCollectionAsync(int id, BloodCollectionRequest request);
<<<<<<< HEAD
        Task<BloodProcedure?> UpdateBloodQualificationAsync(int regisId, RecordBloodQualification request);
=======
        Task<BloodProcedure?> UpdateBloodQualificationAsync(int id, RecordBloodQualification request);
>>>>>>> 7813e3d6e8429ba0d2072fc7e67b73930be3fabd
    }
}
