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
    public class LabTestAndResults
    {
        string startpath = "substanceAdministration";
        Dictionary<string, string> paths = new Dictionary<string, string>();

        public LabTestAndResults() 
        {
            paths.Add("status", $"statusCode[@code]");
            paths.Add("date", $"effectiveTime[@value]");
            paths.Add("commentref", $"entryRelationship/act/text");
            paths.Add("loinc", $"code[@code]");
            paths.Add("nameref", $"text/reference[@value]");
            paths.Add("providerid", $"author/assignedAuthor/id[@extension]");
            paths.Add("providername", $"author/assignedAuthor/assignedPerson/name");
            paths.Add("name", $"code[@displayName]");
            paths.Add("unit", $"value[@unit]");
            paths.Add("result", $"value[@value]");
            paths.Add("range", "referenceRange/observationRange/text");           
        }

        public void LabTestAndResultsReading(XElement element, StringBuilder scriptBuilder, string _ExternalMappingId, string FileName, string _APUID)
        {
            var LabTest = element.XPathSelectElements("entry/organizer/component/observation");
            var LabResults = element.XPathSelectElements("text/table[2]/tbody/tr");
            int trCount = 1;
            int trResultCount = 1;

            foreach (var lt in LabTest)
            {
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientLabTest ");
                scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");               
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamOrderDate = '"); scriptBuilder.Append(CommonLib.UpdateDateFormated(getcomponentDatevalue(lt.GetAttributeValue($"effectiveTime/low[@value]", "value"))));
                scriptBuilder.Append("'");                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamTestStatus ='"); scriptBuilder.Append(lt.GetTableReference(element, $"id[@root]", 2, trCount, 4));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamTestName ='"); scriptBuilder.Append(lt.GetTableReference(element, $"id[@root]", 2, trCount, 2).Replace("'", "''"));
                scriptBuilder.Append("'");
                var value = lt.GetAttributeValue($"value[@code]", "code");
                if (!string.IsNullOrEmpty(value))
                {                    
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamTestResult ='"); scriptBuilder.Append(lt.GetTableReference(element, $"id[@root]", 2, trCount, 3).Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamTestResultUnit =''"); 
                }
                else
                {  
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamTestResult ='"); scriptBuilder.Append(lt.GetAttributeValue("value[@value]", "value").Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamTestResultUnit ='"); scriptBuilder.Append(lt.GetAttributeValue("value[@unit]", "unit"));
                    scriptBuilder.Append("'");
                }
                if (lt.GetAttributeValue("code[@code]", "code") != "UNK")
                {                   
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamLonicCode ='"); scriptBuilder.Append(lt.GetAttributeValue("code[@code]", "code"));
                    scriptBuilder.Append("'");
                }
                else
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamLonicCode ='"); scriptBuilder.Append("'");
                }
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName ='"); scriptBuilder.Append(element.GetAttributeValue($"code[@displayName]", "displayName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName ='"); scriptBuilder.Append(element.GetAttributeValue($"code[@codeSystemName]", "codeSystemName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode ='"); scriptBuilder.Append(element.GetAttributeValue("code", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@root]", "root"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOID = '"); scriptBuilder.Append(element.GetAttributeValue($"code[@codeSystem]", "codeSystem"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@extension]", "extension"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID,");
                scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
                scriptBuilder.Append("@ParamPatientId = @PatientId,");
                scriptBuilder.Append("@ParamFileName = @FileName");
                scriptBuilder.Append(" ;\n");
                trCount++;

            }

            foreach (var lr in LabResults)
            {
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientLabResults ");
                scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCPTCode = '"); scriptBuilder.Append(lr.GetTableReference(element, $"id[@root]", 5, trResultCount, 1));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCodeSystem ='"); scriptBuilder.Append(lr.GetTableReference(element, $"id[@root]", 5, trResultCount, 2));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamPanelDescription ='"); scriptBuilder.Append(lr.GetTableReference(element, $"id[@root]", 5, trResultCount, 3).Replace("'", "''"));
                scriptBuilder.Append("'");
                var DateOrdereds = lr.GetTableReference(element, $"id[@root]", 5, trResultCount, 4);
                if (!string.IsNullOrEmpty(DateOrdereds))
                {                    
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamDateOrdered ='"); scriptBuilder.Append(CommonLib.UpdateDateFormated(DateOrdereds));
                    scriptBuilder.Append("'");
                }

                scriptBuilder.Append(","); scriptBuilder.Append("@ParamNotes ='"); scriptBuilder.Append(lr.GetTableReference(element, $"id[@root]", 5, trResultCount, 5).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName ='"); scriptBuilder.Append(element.GetAttributeValue($"code[@displayName]", "displayName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName ='"); scriptBuilder.Append(element.GetAttributeValue($"code[@codeSystemName]", "codeSystemName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode ='"); scriptBuilder.Append(element.GetAttributeValue("code", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@root]", "root"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOID = '"); scriptBuilder.Append(element.GetAttributeValue($"code[@codeSystem]", "codeSystem"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@extension]", "extension"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID,");
                scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
                scriptBuilder.Append("@ParamPatientId = @PatientId,");
                scriptBuilder.Append("@ParamFileName = @FileName");
                scriptBuilder.Append(" ;\n");
                trResultCount++;
            }
        }


        public string getcomponentDatevalue(string node)
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
                        return returndate.ToString();
                    }
                    else
                    {
                        return null;
                    }


                }

            }
            catch (Exception ex)
            {
                return returndate.ToString(); 
            }
            return returndate.ToString();
        }

    }
}
