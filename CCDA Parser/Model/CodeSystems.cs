using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDA_Parser.Model
{
    public class CodeSystems
    {
        public static string IMOCode { get { return "IMOCode"; } }
        public static string IMOCodeCodesystemOID { get { return "2.16.840.1.113883.3.247.1.1"; } }
        public static string ICD10CM { get { return "ICD10CM"; } }
        public static string ICD10CMCodesystemOID { get { return "2.16.840.1.113883.6.90"; } }

        public static string ICD9PCS { get { return "ICD9PCS"; } }
        public static string ICD9PCSCodesystemOID { get { return "2.16.840.1.113883.6.104"; } }

        public static string ICD9CM { get { return "ICD9CM"; } }
        public static string ICD9CMCodesystemOID { get { return "2.16.840.1.113883.6.103"; } }

        public static string POS { get { return "POS"; } }
        public static string POSCodesystemOID { get { return "2.16.840.1.113883.6.50"; } }


        public static string ICD10PCS { get { return "ICD10PCS"; } }
        public static string ICD10PCSCodesystemOID { get { return "2.16.840.1.113883.6.4"; } }

        public static string CPT { get { return "CPT"; } }
        public static string CPTCodesystemOID { get { return "2.16.840.1.113883.6.12"; } }

        public static string CPTCATII { get { return "CPT-CAT-II"; } }
        public static string CPTCATIICodesystemOID { get { return "2.16.840.1.113883.6.12"; } }

        public static string UBREV { get { return "UBREV"; } }
        public static string UBREVCodesystemOID { get { return "2.16.840.1.113883.6.301.3"; } }

        public static string LOINC { get { return "LOINC"; } }
        public static string LOINCCodesystemOID { get { return "2.16.840.1.113883.6.1"; } }

        public static string HCPCS { get { return "HCPCS"; } }
        public static string HCPCSCodesystemOID { get { return "2.16.840.1.113883.6.285"; } }

        public static string CVX { get { return "CVX"; } }
        public static string CVXCodesystemOID { get { return "2.16.840.1.113883.12.292"; } }

        public static string UBTOB { get { return "UBTOB"; } }
        public static string UBTOBCodesystemOID { get { return "2.16.840.1.113883.6.301.1"; } }
        public static string SNOMEDCT { get { return "SNOMEDCT"; } }
        public static string SNOMEDCTCodesystemOID { get { return "2.16.840.1.113883.6.96"; } }
    }
}
