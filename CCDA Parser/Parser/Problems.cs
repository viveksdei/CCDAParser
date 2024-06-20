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
    public class Problems
    {
        string obspath = "act/entryRelationship/observation";
        string obsPathAct = "act/";
        public void ProblemsReading(XElement element, StringBuilder scriptBuilder, string externalMappingID, string FileName, string _APUID)
        {
            var patientproblem = element.XPathSelectElements("entry");
            int i = 1;
            foreach (var s in patientproblem)
            {
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientDiagnosis ");
                scriptBuilder.Append("@ParamSectionName = '");
                scriptBuilder.Append(element.XPathSelectElement("title").Value);
                scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDiagnosisDate = '"); scriptBuilder.Append(CommonLib.UpdateDateFormated(StartDates(s)));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamResolveDate = '"); scriptBuilder.Append(CommonLib.UpdateDateFormated(EndDates(s)));
                scriptBuilder.Append("'");

                var Confidential = element.GetElementValue("text/paragraph");
                if (!string.IsNullOrEmpty(Confidential))
                {                    
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamIsConfidential = 'true'"); 
                }
                else
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamIsConfidential = 'false'");
                }
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamComment = '"); scriptBuilder.Append(getIcd(s).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatus = '"); scriptBuilder.Append(s.GetAttributeValue($"{obsPathAct}/statusCode[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatusCode = '"); scriptBuilder.Append(s.GetAttributeValue($"act/entryRelationship/observation/statusCode[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@Paramicd10masterdiagnosisid = '"); scriptBuilder.Append(s.GetAttributeValue($"{obspath}/value/translation[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSnomedCode = '"); scriptBuilder.Append(SnomedCode(s));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamProblem = '"); scriptBuilder.Append(s.GetAttributeValue($"{obspath}/value/translation[@displayName]", "displayName").Replace("'", "''"));
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
                i++;
            }

        }

        private string SnomedCode(XElement s)
        {
            try
            {
                string DiagnosisStartDate = string.Empty;

                var refvalue = s.GetAttributeValue("act/id[@root]", "root");
                var Sdate = s.XPathSelectElement($"//tr[@ID='{refvalue}']");
                DiagnosisStartDate = Sdate.GetElementValue("td[1]");                

                return DiagnosisStartDate;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string StartDates(XElement s)
        {
            try
            {
                string DiagnosisStartDate = string.Empty;

                var refvalue = s.GetAttributeValue("act/id[@root]", "root");
                var Sdate = s.XPathSelectElement($"//tr[@ID='{refvalue}']");
                DiagnosisStartDate = Sdate.GetElementValue("td[last()-1]");
                
                return DiagnosisStartDate;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string EndDates(XElement s)
        {
            try
            {
                string DiagnosisStartDate = string.Empty;

                var refvalue = s.GetAttributeValue("act/id[@root]", "root");
                var Sdate = s.XPathSelectElement($"//tr[@ID='{refvalue}']");
                DiagnosisStartDate = Sdate.GetElementValue("td[last()]");               

                return DiagnosisStartDate;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string getIcd(XElement s)
        {
            try
            {
                object obj = new object();
                lock (obj)
                {
                    string desc = "";
                    //var ent = s.XPathSelectElement($"{obspath}/value/translation[@codeSystemName='ICD-10']");
                    var ent = s.XPathSelectElement($"{obspath}/value/translation");
                    if (ent != null)
                    {

                        desc = ent.GetAttributeValue("displayName");
                    }
                    else
                    {
                        var entsnomed = s.XPathSelectElement($"{obspath}/value[@code]");
                        if (entsnomed != null)
                        {

                            var displayattr = entsnomed.Attribute("displayName");
                            if (displayattr != null)
                            {
                                desc = displayattr.Value;
                            }
                        }
                        else
                        {
                            desc = s.FindContentValue("ID", s.XPathSelectElement(obspath).GetReference());
                        }
                    }

                    return desc;

                }

            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
