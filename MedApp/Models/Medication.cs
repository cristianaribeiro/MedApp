using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedApp.Models
{
    public class Medication
    {
        public int MedicationID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
