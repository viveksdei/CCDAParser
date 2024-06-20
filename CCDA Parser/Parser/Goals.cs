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
    public class Goals
    {
        Dictionary<string, string> paths = new Dictionary<string, string>();
        string startingPath = "entry/observation";

        public Goals()
        {
            paths.Add("FirstName", $"author/assignedAuthor/assignedPerson/name/given");
            paths.Add("LastName", $"author/assignedAuthor/assignedPerson/name/family");
            paths.Add("NPI", $"author/assignedAuthor/id[@root]");
            paths.Add("StatusCode", $"statusCode[@code]");            
        }


        public void GoalsReading(XElement element, StringBuilder scriptBuilder, string externalMappingID, string FileName, string _APUID)
        {
            var Goals = element.XPathSelectElements("entry/observation");
            int trCount = 1;
            foreach (var s in Goals)
            {               
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientGoals ");
                scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamFirstName = '"); scriptBuilder.Append(s.GetElementValue(paths["FirstName"]).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamLastName = '"); scriptBuilder.Append(s.GetElementValue(paths["LastName"]).Replace("'", "''"));
                scriptBuilder.Append("'");                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamProgress = '"); scriptBuilder.Append(element.GetSectionDetails(element, $"GetStatus", 2, trCount, 2).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDate = '"); scriptBuilder.Append(CommonLib.UpdateDateFormated(element.GetSectionDetails(element, $"GetDate", 2, trCount, 1)));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamNotes = '"); scriptBuilder.Append(element.GetSectionNotes(element, $"GetNotes", 2, trCount, 3).Replace("'", "''"));
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
                trCount++;
            }

        }
    }
}
