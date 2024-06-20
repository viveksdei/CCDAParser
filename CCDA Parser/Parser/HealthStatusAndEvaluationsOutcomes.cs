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
    public class HealthStatusAndEvaluationsOutcomes
    {
        public void HealthStatusAndEvaluationsOutcomesReading(XElement element, StringBuilder scriptBuilder, string externalMappingID, string FileName, string _APUID)
        {
            var EvaluationsOutcomes = element.XPathSelectElements("entry");
            int trCount = 1;
            foreach (var s in EvaluationsOutcomes)
            {                
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientHealthStatusAndEvaluationsOutcomes ");
                scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCode = '"); scriptBuilder.Append(element.GetAttributeValue($"code[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamHealthStatusEvaluationsDate = '"); scriptBuilder.Append(CommonLib.UpdateDateFormated(s.GetHealthStatusEvaluations_Outcomes(element, $"VName", 1, trCount, 1)));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatus = '"); scriptBuilder.Append(s.GetHealthStatusEvaluations_Outcomes(element, $"VName", 1, trCount, 2));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamNote = '"); scriptBuilder.Append(element.GetSectionNotes(element, $"GetNotes", 2, trCount, 3).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamValue = '"); scriptBuilder.Append(GetValue(s.GetAttributeValue("observation/value[@value]", "value")));
                scriptBuilder.Append("'");                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamUnit = '"); scriptBuilder.Append(s.GetAttributeValue("observation/value[@unit]", "unit"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDisplayName = '"); scriptBuilder.Append(s.GetAttributeValue("observation/entryRelationship/observation/value[@displayName]", "displayName").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSnomedCode = '"); scriptBuilder.Append(s.GetAttributeValue("observation/entryRelationship/observation/value[@code]", "code"));
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
        private string GetValue(string str)
        {
            Decimal h2 = 0;
            var bca = Decimal.TryParse(str, out h2);
            decimal h4 = Decimal.Parse(str, System.Globalization.NumberStyles.Any);
            h4 = Math.Round(h4, 2);
            // var bv = "dfd";
            return h4.ToString();
        }
    }
}
