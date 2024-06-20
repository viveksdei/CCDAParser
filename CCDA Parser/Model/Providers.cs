using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDA_Parser.Model
{
    public class Providers
    {
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Sufix { get; set; }
        public string NPI { get; set; }
        public string DisplayName { get; set; }
        public string CodeDisplayName { get; set; }
        public string StreetAddressLine1 { get; set; }
        public string StreetAddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string SectionCodeSystemName { get; set; }
        public string SectionName { get; set; }
        public string SectionChildName { get; set; }
        public string SectionDisplayName { get; set; }
        public string SectionCode { get; set; }
        public string SectionRoot { get; set; }
        public string SectionOID { get; set; }
        public string SectionExtension { get; set; }

        public string OtherAddress1 { get; set; }
        public string OtherAddress2 { get; set; }
        public string AddressType { get; set; }
        public string OtherCity { get; set; }
        public string OtherState { get; set; }
        public string OtherCountry { get; set; }
        public string OtherZip { get; set; }
    }
}
