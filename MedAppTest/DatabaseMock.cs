using MedApp.Data;
using MedApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedAppTest
{
    public class DatabaseMock
    {
        private static DatabaseMock instance;

        private DbContextOptions<MedicationContext> options;
        public MedicationContext context;

        public DatabaseMock()
        {
            this.options = new DbContextOptionsBuilder<MedicationContext>()
               .UseInMemoryDatabase(databaseName: "MedDB")
               .Options;

            this.context = new MedicationContext(this.options);

            this.context.MedicationItems.Add(new Medication
            {
                Description = "Ipobrufeno",
                CreationDate = new DateTime(2015, 12, 31, 5, 10, 20),
                Quantity = 4,
                MedicationID = 1
            });

            this.context.MedicationItems.Add(new Medication
            {
                Description = "Paracetamol",
                CreationDate = new DateTime(2015, 12, 31, 5, 10, 20),
                Quantity = 3,
                MedicationID = 2
            });

            this.context.SaveChanges();
        }
        public static DatabaseMock Instance
        {
            get
            {
                if (instance == null)
                {

                    instance = new DatabaseMock();
                }
                return instance;
            }
        }
    }
}
