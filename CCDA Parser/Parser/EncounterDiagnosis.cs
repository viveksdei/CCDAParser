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
    public class EncounterDiagnosis
    {
        Dictionary<string, string> paths = new Dictionary<string, string>();
        string startingPath = "entry/observation";

        public EncounterDiagnosis()
        {
            paths.Add("FirstName", $"author/assignedAuthor/assignedPerson/name/given");
            paths.Add("LastName", $"author/assignedAuthor/assignedPerson/name/family");
            paths.Add("NPI", $"author/assignedAuthor/id[@root]");
            paths.Add("StatusCode", $"statusCode[@code]");
        }

        public void EncounterDiagnosisReading(XElement element, StringBuilder scriptBuilder, string externalMappingID, string FileName, string _APUID)
        {
            var EncounterDiagnosis = element.XPathSelectElements("entry/encounter/entryRelationship/act/entryRelationship");
            int trCount = 1;
            foreach (var s in EncounterDiagnosis)
            {
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientEncounterDiagnosis ");
                scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDiagnosis = '"); scriptBuilder.Append(element.GetTableReferenceEncounterDiagnosis(element, $"Diagnosis", 1, trCount, 1).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDiagnosisDescription = '"); scriptBuilder.Append(element.GetTableReferenceEncounterDiagnosis(element, $"Diagnosis", 2, trCount, 2).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSnomedCode = '"); scriptBuilder.Append(s.GetAttributeValue($"observation/code[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDisplayName = '"); scriptBuilder.Append(s.GetAttributeValue($"observation/code[@displayName]", "displayName").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatusCode = '"); scriptBuilder.Append(s.GetAttributeValue($"observation/statusCode[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamICD10 = '"); scriptBuilder.Append(s.GetAttributeValue($"observation/value[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCPT4Code = '"); scriptBuilder.Append(element.GetAttributeValue($"entry/encounter/code[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCPT4DisplayName = '"); scriptBuilder.Append(element.GetAttributeValue($"entry/encounter/code[@displayName]", "displayName").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStartDate = '"); scriptBuilder.Append(getcomponentDatevalue(s.GetAttributeValue($"observation/effectiveTime/low[@value]", "value")));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEndDate = '"); scriptBuilder.Append(getcomponentDatevalue(s.GetAttributeValue($"observation/effectiveTime/high[@value]", "value")));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamTypeCode = '"); scriptBuilder.Append(element.GetAttributeValue($"entry/encounter/entryRelationship/act/entryRelationship[@typeCode]", "typeCode"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append(element.GetAttributeValue("code[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOId = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystem]", "codeSystem"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@displayName]", "displayName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@extension]", "extension"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystemName]", "codeSystemName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@root]", "root"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID,");
                scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
                scriptBuilder.Append("@ParamPatientId = @PatientId,");
                scriptBuilder.Append("@ParamFileName = @FileName");
                scriptBuilder.Append(" ;\n");
                trCount++;

            }
        }
        public DateTime? getcomponentDatevalue(string node)
        {
            DateTime? returndate = null;
            try
            {
                string returnvalue = "";
                var element = node;
                if (!string.IsNullOrEmpty(element))
                {
                    returnvalue = node;
                    if (returnvalue.Length >= 4)
                    {
                        if (returnvalue.Contains("-"))
                        {
                            returnvalue = returnvalue.Substring(0, returnvalue.IndexOf("-"));
                        }
                        string[] formats = new string[] { "yyyy", "yyyyMMdd", "yyyyMMddHHmmss" };
                        returndate = DateTime.ParseExact(returnvalue, formats, null, System.Globalization.DateTimeStyles.None);
                        return returndate;
                    }
                    else
                    {
                        return null;
                    }


                }

            }
            catch (Exception ex)
            {
                return returndate;
            }
            return returndate;
        }
    }
}
