using MedApp.Data;
using MedApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using MedApp.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using System.Resources;
using Microsoft.Extensions.Logging.Abstractions;
using MedApp.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MedAppTest
{
    [TestClass]
    public class MedAppTests
    {
        DatabaseMock databaseMock;
        public MedAppTests()
        {
            databaseMock = DatabaseMock.Instance;
        }

        [TestMethod]
        public void TestMethodGetAll()
        {
            MedicationRepository controller = new MedicationRepository(databaseMock.context);
            IEnumerable<Medication> medications = controller.GetMedications();

            Assert.AreEqual(2, medications.ToList().Count());
            Assert.AreEqual("Ipobrufeno", medications.FirstOrDefault().Description);
            Assert.AreEqual(4, medications.FirstOrDefault().Quantity);
            Assert.AreEqual(new DateTime(2015, 12, 31, 5, 10, 20), medications.FirstOrDefault().CreationDate);
            Assert.AreEqual(1, medications.FirstOrDefault().MedicationID);


            Assert.AreEqual("Paracetamol", medications.ElementAt(1).Description);
            Assert.AreEqual(3, medications.ElementAt(1).Quantity);
            Assert.AreEqual(new DateTime(2015, 12, 31, 5, 10, 20), medications.ElementAt(1).CreationDate);
            Assert.AreEqual(2, medications.ElementAt(1).MedicationID);
        }


        [TestMethod]
        public void TestMethodGetMedicationByID()
        {
            MedicationRepository controller = new MedicationRepository(databaseMock.context);
            Medication medication = controller.GetMedicationByID(1);

            Assert.AreEqual("Ipobrufeno", medication.Description);
            Assert.AreEqual(4, medication.Quantity);
            Assert.AreEqual(new DateTime(2015, 12, 31, 5, 10, 20), medication.CreationDate);
            Assert.AreEqual(1, medication.MedicationID);
        }


        [TestMethod]
        public void TestMethodSuccessPostMedication()
        {
            MedicationRepository medicationRepository = new MedicationRepository(databaseMock.context);

            Medication medication = new Medication();
            medication.CreationDate = DateTime.Now;
            medication.Description = "ARAVA";
            medication.Quantity = 1;

            var options = Options.Create(new LocalizationOptions());
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            var localizer = new StringLocalizer<Resource>(factory);

            var controller = new MedicationsController(medicationRepository, localizer);

            ActionResult<Medication> result = controller.PostMedication(medication);

            Assert.AreEqual(3, medicationRepository.GetMedications().ToList().Count);
            Assert.IsInstanceOfType(result, typeof(ActionResult<Medication>));

            var contentResult = (Medication) ((ObjectResult) result.Result).Value;
            Assert.AreEqual("ARAVA", contentResult.Description);

        }


        [TestMethod]
        public void TestMethodErrorQuantityPostMedication()
        {
            MedicationRepository medicationRepository = new MedicationRepository(databaseMock.context);

            Medication medication = new Medication();
            medication.CreationDate = DateTime.Now;
            medication.Description = "ARAVA";
            medication.Quantity = -1;

            var options = Options.Create(new LocalizationOptions());
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            var localizer = new StringLocalizer<Resource>(factory);

            var controller = new MedicationsController(medicationRepository, localizer);

            var result = controller.PostMedication(medication);

            Assert.AreNotEqual(3, medicationRepository.GetMedications().ToList().Count);

            var contentResult = result.Result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status422UnprocessableEntity, contentResult.StatusCode);

        }
    }
}
