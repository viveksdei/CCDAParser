using CCDA_Parser.Parser.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CCDA_Parser.Parser
{
    public class VitalSigns
    {
        string startpath = "act";
        Dictionary<string, string> paths = new Dictionary<string, string>();

        public VitalSigns()
        {
            paths.Add("VitalValue", $"observation/value[@value]");
            paths.Add("VitalUnit", $"observation/value[@unit]");
            paths.Add("VitalName", $"observation/code");
            paths.Add("SnomedCode", $"observation/code[@code]");
            paths.Add("Date", $"observation/effectiveTime");
        }

        public void VitalSignsReading(XElement element, StringBuilder scriptBuilder, string externalMappingID, string FileName, string _APUID)
        {
            var VitalSigns = element.XPathSelectElements("entry/organizer/component");
            var countTD = element.XPathSelectElements("text/table/tbody/tr/td");
            int trCount = 1;
            int tdCount = 2;
            int thCount = 2;
            int SnCount = 1;
            string VitalName, value, VitalValue, StatusCode, SnomedCode;           
            foreach (var s in countTD)
            {
                VitalName = string.Empty;
                value= string.Empty;
                VitalValue= string.Empty;
                StatusCode = string.Empty;
                SnomedCode = string.Empty;
                var vac = new VitalSigns();
                if (thCount == 11)
                {
                    thCount = 2;
                }
                if (tdCount == 11)
                {
                    trCount++;
                    tdCount = 2;
                }
                VitalName = element.GetTableReferenceVital(element, $"id[@root]", 1, 1, thCount);
                value = element.GetTableReferenceVital(element, $"id[@root]", 2, trCount, tdCount);
                VitalValue = GetValue(value);
                if (!string.IsNullOrEmpty(VitalName))
                {
                    if (!string.IsNullOrEmpty(VitalValue))
                    {
                        scriptBuilder.Append("\n");
                        scriptBuilder.Append("EXEC usp_SaveUpdatePatientVitals ");
                        scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                        scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamVitalStartDate = '"); scriptBuilder.Append(CommonLib.UpdateDateFormated(element.GetTableReferenceVital(element, $"id[@root]", 2, trCount, 1)));
                        scriptBuilder.Append("'");
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamVitalName = '"); scriptBuilder.Append(element.GetTableReferenceVital(element, $"id[@root]", 1, 1, thCount).Replace("'", "''"));
                        scriptBuilder.Append("'");       
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamVitalValue = '"); scriptBuilder.Append(VitalValue);
                        scriptBuilder.Append("'");
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamVitalUnit = '"); scriptBuilder.Append(GetUnit(value));
                        scriptBuilder.Append("'");                       
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatusCode = '"); scriptBuilder.Append(element.GetAttributeValue($"entry/organizer/statusCode","code"));
                        scriptBuilder.Append("'");
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamSnomedCode = '"); scriptBuilder.Append("'");
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append(element.GetAttributeValue("code[@code]", "code"));
                        scriptBuilder.Append("'");
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOId = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystem]", "codeSystem"));
                        scriptBuilder.Append("'");
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystemName]", "codeSystemName"));
                        scriptBuilder.Append("'");
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(element.GetAttributeValue("templateId[@root]", "root"));
                        scriptBuilder.Append("'");
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@displayName]", "displayName"));
                        scriptBuilder.Append("'");
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append(element.GetAttributeValue("templateId[@extension]", "extension"));
                        scriptBuilder.Append("'");
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID,");
                        scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
                        scriptBuilder.Append("@ParamPatientId = @PatientId,");
                        scriptBuilder.Append("@ParamFileName = @FileName");
                        scriptBuilder.Append(" ;\n");                        
                    }
                }
                tdCount++;
                thCount++;
                SnCount++;
            }
        }

        private string GetUnit(string str)
        {
            str = str.ToUpper();
            if (string.IsNullOrEmpty(str))
                return null;

            var arr = str.Split(' ');
            var arr1 = "";
            if (str.Contains('%'))
            {
                arr1 = str.Substring(str.IndexOf('%'));
                return arr1;
            }
            if (str.Contains("CM"))
            {
                arr1 = str.Substring(str.IndexOf("CM"));
                arr1 = arr1.ToLower();
                return arr1;
            }
            if (str.Contains("KG"))
            {
                arr1 = str.Substring(str.IndexOf("KG"));
                arr1 = arr1.ToLower();
                return arr1;
            }
            if (str.Contains("KG/M2"))
            {
                arr1 = str.Substring(str.IndexOf("KG/M2"));
                arr1 = arr1.ToLower();
                return arr1;
            }

            if (str.Contains("CEL"))
            {
                arr1 = str.Substring(str.IndexOf("CEL"));
                arr1 = arr1.ToLower();
                return arr1;
            }
            if (str.Contains("/MIN"))
            {
                arr1 = str.Substring(str.IndexOf("/MIN"));
                arr1 = arr1.ToLower();
                return arr1;
            }
            if (str.Contains("MM[HG]"))
            {
                arr1 = str.Substring(str.IndexOf("MM[HG]"));
                arr1 = arr1.ToLower();
                return arr1;
            }

            return "";
        }

        private string GetValue(string str)
        {
            str = str.ToUpper();
            if (string.IsNullOrEmpty(str))
                return null;

            var arr = str.Split(' ');
            var arr1 = "";
            if (str.Contains('%'))
            {
                arr1 = str.Substring(0, str.IndexOf('%'));
                return arr1;
            }
            if (str.Contains("CM"))
            {
                arr1 = str.Substring(0, str.IndexOf("CM"));
                arr1 = arr1.ToLower();
                return arr1;
            }
            if (str.Contains("KG"))
            {
                arr1 = str.Substring(0, str.IndexOf("KG"));
                arr1 = arr1.ToLower();
                return arr1;
            }
            if (str.Contains("KG/M2"))
            {
                arr1 = str.Substring(0, str.IndexOf("KG/M2"));
                arr1 = arr1.ToLower();
                return arr1;
            }
            if (str.Contains("CEL"))
            {
                arr1 = str.Substring(0, str.IndexOf("CEL"));
                arr1 = arr1.ToLower();
                return arr1;
            }
            if (str.Contains("/MIN"))
            {
                arr1 = str.Substring(0, str.IndexOf("/MIN"));
                arr1 = arr1.ToLower();
                return arr1;
            }
            if (str.Contains("MM[HG]"))
            {
                arr1 = str.Substring(0, str.IndexOf("MM[HG]"));
                arr1 = arr1.ToLower();
                return arr1;
            }
            return "";
        }

    }
}
