using MedApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MedicationContext context)
        {
            context.Database.EnsureCreated();

            if (context.MedicationItems.Any())
            {
                return;   // DB has been seeded
            }

            var medications = new Medication[]
            {
                new Medication{Description="Adderall",Quantity=20,CreationDate=DateTime.Now},
                new Medication{Description="Amoxicillin",Quantity=35,CreationDate=DateTime.Now},
                new Medication{Description="Xanax",Quantity=289,CreationDate=DateTime.Now},
                new Medication{Description="Tramadol",Quantity=2,CreationDate=DateTime.Now},
                new Medication{Description="Omeprazole",Quantity=150,CreationDate=DateTime.Now},
            };

            foreach (Medication m in medications)
            {
                context.MedicationItems.Add(m);
            }

            context.SaveChanges();

        }
    }
}
