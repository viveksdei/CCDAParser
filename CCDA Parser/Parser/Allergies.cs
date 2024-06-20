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
    public class Allergies
    {
        string startpath = "act";
        Dictionary<string, string> paths = new Dictionary<string, string>();

        public Allergies()
        {
            paths.Add("status", $"{startpath}/ statusCode[@code]");
            paths.Add("severity", $"act/entryRelationship[@typeCode='SUBJ']/observation/entryRelationship[@typeCode='SUBJ']/ observation / text / reference[@value]");
            paths.Add("startdate", $"{startpath}/entryRelationship / observation / effectiveTime / low[@value]");
            paths.Add("cause", $"{startpath}/entryRelationship / observation / participant / participantRole / playingEntity / code[@displayName]");
            paths.Add("allergyref", $"act/entryRelationship/observation/participant/participantRole/playingEntity/code[@displayName] ");
            paths.Add("RXNORM", $"act/entryRelationship/observation/participant/participantRole/playingEntity/code[@code] ");
            paths.Add("allergytype", $"act / entryRelationship[1] / observation / value[@displayName]");
            paths.Add("snomedcode", $"act / entryRelationship[1] / observation / value[@codeSystemName='SNOMED CT']");
            paths.Add("performerid", $"act / entryRelationship[1] / observation / performer / assignedEntity / id[@extension]");
            paths.Add("performername", $" act / entryRelationship[1] / observation / performer / assignedEntity / assignedPerson / name");
            paths.Add("encounterid", $"act / entryRelationship / encounter / id[@extension]");
            paths.Add("Comment", $"act/entryRelationship[@typeCode='SUBJ']/observation/entryRelationship[@typeCode='SUBJ']/act/text");
        }


        public void AllergiesReading(XElement element, StringBuilder scriptBuilder, string externalMappingID, string FileName, string _APUID)
        {
            var datagroup = element.XPathSelectElements("entry");
            foreach (var s in datagroup)
            {
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientAllergies  ");
                scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value);
                scriptBuilder.Append("',"); scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStartdate = '"); scriptBuilder.Append(s.GetDate($"{paths["startdate"]}", "value"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAllergen = '"); scriptBuilder.Append(s.GetAttributeValue(paths["allergyref"], "displayName").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamRXNORM = '"); scriptBuilder.Append(s.GetAttributeValue(paths["RXNORM"], "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSubstance = '"); scriptBuilder.Append(s.GetContentByReference(paths["allergyref"]).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAllergyTypeDescripition = '"); scriptBuilder.Append(s.GetAttributeValue($"{paths["allergytype"]}", "displayName").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSeverity = '"); scriptBuilder.Append(s.GetAttributeValue($"{paths["severity"]}", "value"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamProviderName = '"); scriptBuilder.Append(s.GetName(paths["performername"]).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDescription = '"); scriptBuilder.Append(s.GetContentByReference(paths["Comment"]).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamNote = '"); scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatus = '"); scriptBuilder.Append(s.GetAttributeValue(paths["status"], "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatusCode = '"); scriptBuilder.Append(s.GetAttributeValue("act/entryRelationship/observation/statusCode[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSnomedCode = '"); scriptBuilder.Append(s.GetAttributeValue("act/entryRelationship/observation/value[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append(element.GetAttributeValue("code[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOId = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystem]", "codeSystem"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@extension]", "extension"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@root]", "root"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@displayName]", "displayName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystemName]", "codeSystemName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamPatientId = @PatientId");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
                scriptBuilder.Append("@ParamFileName = @FileName");
                scriptBuilder.Append(" ;\n");

            }

        }
    }
}
