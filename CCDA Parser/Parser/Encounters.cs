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
    public class Encounters
    {
        public void EncountersReading(XElement element, StringBuilder scriptBuilder, string externalMappingId, long orgId, string _fileName, string _APUID)
        {
            var Encounters = element.XPathSelectElements("encompassingEncounter");
            foreach (var e in Encounters)
            {
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientEncounters ");
                scriptBuilder.Append("@ParamSectionName = 'componentOf',");
                scriptBuilder.Append("@ParamSectionChildName = 'encompassingEncounter'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStartDateTime = '"); scriptBuilder.Append(e.GetDate($"effectiveTime/low[@value]", "value"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEndDateTime = '"); scriptBuilder.Append(e.GetDate($"effectiveTime/high[@value]", "value"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEncounterDescription = '"); scriptBuilder.Append(getendesc(e).Replace("'", "''"));
                scriptBuilder.Append("'");                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamLocation = '"); scriptBuilder.Append(e.GetElementValue("location/healthCareFacility/serviceProviderOrganization/name").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamPerformerName = '"); scriptBuilder.Append(e.GetName("responsibleParty/assignedEntity/assignedPerson/name").Replace("'", "''"));
                scriptBuilder.Append("'");                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamPatientVisitId = '"); scriptBuilder.Append(e.GetAttributeValue("id[@root]", "root"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamExternalProviderId = '"); scriptBuilder.Append(e.GetAttributeValue($"responsibleParty/assignedEntity/id[@root]", "root"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCode = '"); scriptBuilder.Append(e.GetAttributeValue("id[@code]", "code"));
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
                scriptBuilder.Append("@ParamFileName = @FileName,");
                scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
                scriptBuilder.Append("@ParamPatientId = @PatientId");
                scriptBuilder.Append(" ;\n");
            }
        }
        public string getendesc(XElement node)
        {
            try
            {
                var name = node.GetAttributeValue("code[@displayName]", "displayName");
                if (string.IsNullOrEmpty(name))
                {
                    name = node.GetContentByReference("code/originalText");
                }
                return name;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
