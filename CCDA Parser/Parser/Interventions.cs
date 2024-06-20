using CCDA_Parser.Model;
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
    public class Interventions
    {
        string startpath = "entry/act";
        Dictionary<string, string> paths = new Dictionary<string, string>();
        
        public Interventions()
        {
            paths.Add("date", $"td[2]");
            paths.Add("status", $"td[1]");
            paths.Add("notes", $"td[3]/content");
            paths.Add("providername", $"author/assignedAuthor/assignedPerson/name");
        }

        public void InterventionsReading(XElement element, StringBuilder scriptBuilder, string externalMappingId, string FileName, string _APUID)
        {
            var Interventions = element.XPathSelectElements("text/table/tbody/tr");
            int trCount = 1;
            List<PatientInterventions> InterventionslList = new List<PatientInterventions>();
            foreach (var s in Interventions)
            {
                PatientInterventions patientInterventions = new PatientInterventions();
                patientInterventions.SectionName = element.XPathSelectElement("title").Value;
                patientInterventions.SectionChildName = "section";
                patientInterventions.SectionCode = element.GetAttributeValue("code[@code]", "code");
                patientInterventions.SectionOId = element.GetAttributeValue("code[@codeSystem]", "codeSystem");
                patientInterventions.SectionDisplayName = element.GetAttributeValue("code[@displayName]", "displayName");
                patientInterventions.SectionCodeSystemName = element.GetAttributeValue("code[@codeSystemName]", "codeSystemName");
                patientInterventions.SectionRoot = element.GetAttributeValue($"templateId[@root]", "root");
                patientInterventions.SectionExtension = element.GetAttributeValue($"templateId[@extension]", "extension");
                patientInterventions.Status = element.GetSectionDetails(element, $"GetInterventionsStatus", 2, trCount, 1);
                patientInterventions.Notes = element.GetSectionNotes(element, $"GetInterventionsNotes", 2, trCount, 3);                
                patientInterventions.Date = element.GetSectionDetails(element, $"GetInterventionsStatus", 2, trCount, 2);                
                InterventionslList.Add(patientInterventions);
                trCount++;
            }

            var index = 0;
            foreach (var s in element.XPathSelectElements(startpath))
            {
                InterventionslList[index].ProviderName = s.GetName(paths["providername"]);
                index++;
            }

            foreach (var il in InterventionslList)
            {
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientInterventions ");
                scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(il.SectionName); scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatus = '"); scriptBuilder.Append(il.Status);
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamNotes = '"); scriptBuilder.Append(il.Notes.Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDate = '"); scriptBuilder.Append(CommonLib.UpdateDateFormated(il.Date));
                scriptBuilder.Append("'");               
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamProviderName = '"); scriptBuilder.Append((il.ProviderName).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append(il.SectionCode);
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOId = '"); scriptBuilder.Append(il.SectionOId);
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append(il.SectionDisplayName);
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append(il.SectionCodeSystemName);
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(il.SectionRoot);
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension= '"); scriptBuilder.Append(il.SectionExtension);
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID,");
                scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
                scriptBuilder.Append("@ParamPatientId = @PatientID,");
                scriptBuilder.Append("@ParamFileName = @FileName");
                scriptBuilder.Append(" ;\n");                
            }

        }           
    }
}
