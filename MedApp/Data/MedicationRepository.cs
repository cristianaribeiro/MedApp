using MedApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedApp.Data
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly MedicationContext _context;

        public MedicationRepository(MedicationContext context)
        {
            _context = context;
        }
        public void DeleteMedication(int medicationID)
        {
            Medication medication = _context.MedicationItems.Find(medicationID);
            _context.MedicationItems.Remove(medication);
            _context.SaveChanges();
        }

        public Medication GetMedicationByID(int medicationID)
        {
            return _context.MedicationItems.Find(medicationID);
        }

        public IEnumerable<Medication> GetMedications()
        {
            return _context.MedicationItems.ToList();
        }

        public void InsertMedication(Medication medication)
        {
            _context.MedicationItems.Add(medication);
            _context.SaveChanges();
        }
    }
}
