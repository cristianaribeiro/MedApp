using MedApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedApp.Data
{
    public interface IMedicationRepository 
    {
        IEnumerable<Medication> GetMedications();
        Medication GetMedicationByID(int medicationID);
        void InsertMedication(Medication medication);
        void DeleteMedication(int medicationID);
    }
}
