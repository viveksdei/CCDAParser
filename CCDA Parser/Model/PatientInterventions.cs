using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDA_Parser.Model
{
    public class PatientInterventions
    {
        public string Status { get; set; }
        public string Date { get; set; }
        public string Notes { get; set; }
        public string ProviderName { get; set; }
        public string ProviderId { get; set; }
        public string APUID { get; set; }
        public string SectionOId { get; set; }
        public string SectionCode { get; set; }
        public string SectionCodeSystemName { get; set; }
        public string SectionDisplayName { get; set; }
        public string SectionChildName { get; set; }
        public string SectionName { get; set; }
        public string ChildRoot { get; set; }
        public string ChildExtension { get; set; }
        public string ChildOID { get; set; }
        public string SectionExtension { get; set; }
        public string SectionRoot { get; set; }
    }
}
