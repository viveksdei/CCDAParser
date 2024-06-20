using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDA_Parser.Model
{
    public class PlanOfCare
    {
        public string PatientVisitId { get; set; }
        public string Goal { get; set; }
        public string Instructions { get; set; }
        public Nullable<DateTime> PlannedDate { get; set; }
        public string Description { get; set; }
        public string POCType { get; set; }
        public string SnomedCode { get; set; }
        public string CPTCodes { get; set; }
        public string ICDCode { get; set; }
        public string ICDCodedDescription { get; set; }
        public string ICD9PCS { get; set; }
        public string ICD9CM { get; set; }
        public string ICD10CM { get; set; }
        public string ICD10PCS { get; set; }
        public string Type { get; set; }
        public string HCPCS { get; set; }
        public string LOINC { get; set; }
        public string IMOCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string PerformerName { get; set; }
        public string PerformerFirstName { get; set; }
        public string PerformerMiddleName { get; set; }
        public string PerformerLastName { get; set; }
        public string Suffix { get; set; }
        public string Comments { get; set; }
        public Boolean IsActive { get; set; } = false;
    }
}
