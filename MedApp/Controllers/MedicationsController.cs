using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedApp.Models;
using Microsoft.Extensions.Localization;
using MedApp.Resources;
using MedApp.Data;

namespace MedApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private readonly IMedicationRepository medicationRepository;
        private readonly IStringLocalizer<Resource> localizer;

        public MedicationsController(IMedicationRepository _medicationRepository, IStringLocalizer<Resource> localizer)
        {
            this.medicationRepository = _medicationRepository;
            this.localizer = localizer;
        }

        // GET: api/Medications
        [HttpGet]
        public ActionResult<IEnumerable<Medication>> GetMedications()
        {
            return Ok(medicationRepository.GetMedications());
        }

        // GET: api/Medications/5
        [HttpGet("{id}")]
        public ActionResult<Medication> GetMedication(int id)
        {
            var medication = medicationRepository.GetMedicationByID(id);

            if (medication == null)
            {
                return NotFound();
            }

            return Ok(medication);
        }

        // POST: api/Medications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Medication> PostMedication(Medication medication)
        {
            List<string> errorMessages = new List<string>();

            if (medication.Quantity <= 0)
            {
                errorMessages.Add(localizer["QuantityGreaterThanZero"].Value);
            }

            if(medication.Description == null)
            {
                errorMessages.Add(localizer["DescriptionMissing"].Value);

            } else if (medication.Description.Length == 0)
            {
                errorMessages.Add(localizer["DescriptionNotEmpty"].Value);
            }

            if(errorMessages.Count > 0)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, String.Join(Environment.NewLine, errorMessages));
            }

            medication.CreationDate = DateTime.Now;

            medicationRepository.InsertMedication(medication);

            return CreatedAtAction(nameof(GetMedication), new { id = medication.MedicationID }, medication);
        }

        // DELETE: api/Medications/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMedication(int id)
        {
            var medication = medicationRepository.GetMedicationByID(id);
            if (medication == null)
            {
                return NotFound();
            }

            medicationRepository.DeleteMedication(id);
            return NoContent();
        }
    }
}
