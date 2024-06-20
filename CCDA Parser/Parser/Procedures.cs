using CCDA_Parser.Model;
using CCDA_Parser.Parser.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CCDA_Parser.Parser
{
    public class Procedures
    {
        string startpath = "act";
        Dictionary<string, string> paths = new Dictionary<string, string>();       

        public Procedures()
        {
            paths.Add("allergyref", $"act / entryRelationship / observation / value / originalText ");
            paths.Add("cpt", $"code[@codeSystem='{CodeSystems.CPTCodesystemOID}']");
            paths.Add("imo", $"code/translation[@codeSystem='{CodeSystems.IMOCodeCodesystemOID}']");
            paths.Add("icd10cm", $"code/translation[@codeSystem='{CodeSystems.ICD10CMCodesystemOID}']");
            paths.Add("icd9cm", $"code/translation[@codeSystem='{CodeSystems.ICD9CMCodesystemOID}']");
            paths.Add("icd10pcs", $"code/translation[@codeSystem='{CodeSystems.ICD10PCSCodesystemOID}']");
            paths.Add("icd9pcs", $"code/translation[@codeSystem='{CodeSystems.ICD9PCSCodesystemOID}']");
            paths.Add("hcpcs", $"code/translation[@codeSystem='{CodeSystems.HCPCSCodesystemOID}']");
            paths.Add("loinc", $"code/translation[@codeSystem='{CodeSystems.LOINCCodesystemOID}']");
            paths.Add("snomed", $"code[@codeSystem='{CodeSystems.SNOMEDCTCodesystemOID}']");
            paths.Add("performer", $"performer/assignedEntity/assignedPerson/name");
            paths.Add("performerId", $"performer/assignedEntity/id[@extension]");
            paths.Add("des", $"code/originalText");

        }

        public void ProceduresReading(XElement element, StringBuilder scriptBuilder, string externalMappingID, string FileName, string _APUID)
        {
            var PatientProcedures = element.XPathSelectElements("entry/procedure");          
            foreach (var s in PatientProcedures)
            {
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientProcedures ");
                scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStartDate = '"); scriptBuilder.Append(s.GetDate($"effectiveTime[@value]", "value"));
                scriptBuilder.Append("'");                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCPTCodes = '"); scriptBuilder.Append(GetIcCode(s).Replace("*", ""));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDescription = '"); scriptBuilder.Append(s.GetAttributeValue("code", "displayName").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEndDate = '"); scriptBuilder.Append(s.GetDate($"effectiveTime[@value]", "value"));
                scriptBuilder.Append("'"); 
                var NameNpi = s.GetAttributeValue($"performer/assignedEntity/id[@assigningAuthorityName]", "assigningAuthorityName");
                if (NameNpi == "National Provider Identifier (NPI) Number")
                {                   
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamPerformerId = '"); scriptBuilder.Append(s.GetAttributeValue(paths["performerId"], "extension"));
                    scriptBuilder.Append("'");
                }
                else
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamPerformerId = '"); scriptBuilder.Append("'");
                }                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamPerformerName = '"); scriptBuilder.Append(s.GetName(paths["performer"]).Replace("'", "''"));
                scriptBuilder.Append("'"); 
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatus = '"); scriptBuilder.Append(s.GetAttributeValue("statusCode", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append(element.GetAttributeValue("code[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOId = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystem]", "codeSystem"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@displayName]", "displayName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystemName]", "codeSystemName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@extension]", "extension"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@root]", "root"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID,");
                scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
                scriptBuilder.Append("@ParamPatientId = @PatientId,");
                scriptBuilder.Append("@ParamFileName = @FileName");
                scriptBuilder.Append(" ;\n");               
            }

            PatientProcedures = null;
            PatientProcedures = element.XPathSelectElements("entry/observation");
            foreach (var s in PatientProcedures)
            {
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientProcedures ");
                scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStartDate = '"); scriptBuilder.Append(s.GetDate($"effectiveTime/low[@value]", "value"));
                scriptBuilder.Append("'");               
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCPTCodes = '"); scriptBuilder.Append(GetIcCode(s).Replace("*", ""));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDescription = '"); scriptBuilder.Append(s.GetContentByReference(paths["des"]).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEndDate = '"); scriptBuilder.Append(s.GetDate($"effectiveTime/high[@value]", "value"));
                scriptBuilder.Append("'");
                var NameNpi = s.GetAttributeValue($"performer/assignedEntity/id[@assigningAuthorityName]", "assigningAuthorityName");
                if (NameNpi == "National Provider Identifier (NPI) Number")
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamPerformerId = '"); scriptBuilder.Append(s.GetAttributeValue(paths["performerId"], "extension"));
                    scriptBuilder.Append("'");
                }
                else
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamPerformerId = '"); scriptBuilder.Append("'");
                }                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamPerformerName = '"); scriptBuilder.Append(s.GetName(paths["performer"]).Replace("'", "''"));
                scriptBuilder.Append("'"); 
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatus = '"); scriptBuilder.Append(s.GetAttributeValue("statusCode", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append(element.GetAttributeValue("code[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOId = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystem]", "codeSystem"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@displayName]", "displayName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystemName]", "codeSystemName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@extension]", "extension"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@root]", "root"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID,");
                scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
                scriptBuilder.Append("@ParamPatientId = @PatientId,");
                scriptBuilder.Append("@ParamFileName = @FileName");
                scriptBuilder.Append(" ;\n");

            } 
          
        }

        public string GetIcCode(XElement s)
        {
            var Cpt = s.GetAttributeValue(paths["cpt"], "code");
            if (Cpt == "")
            {
                var c = s.GetContentByReference(paths["des"]);
                Regex regex = new Regex(@"(\d{5})");
                Match match = regex.Match(c);
                if (match.Success)
                {
                    Cpt = match.Value;
                }
            }
            return Cpt;
        }
    }
}
