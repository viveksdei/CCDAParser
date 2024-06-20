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
    public class Medications
    {
        string startpath = "substanceAdministration";
        Dictionary<string, string> paths = new Dictionary<string, string>();

        public Medications()
        {
            paths.Add("consumable", $"{startpath}/consumable/manufacturedProduct/manufacturedMaterial");
            paths.Add("Rxcode", $"{paths["consumable"]}/code[@codeSystemName='RxNorm']");
            paths.Add("Name", $"{paths["consumable"]}/code[@displayName]");
            paths.Add("Textref", $"{startpath}/consumable/manufacturedProduct/id[@root]");
            paths.Add("MedicationName", $"{paths["consumable"]}/code/translation[@displayName]");
        }
        public void MedicationsReading(XElement element, StringBuilder scriptBuilder, string externalMappingID, string FileName, string _APUID)
        {
            var PatientMedication = element.XPathSelectElements("entry");
            foreach (var s in PatientMedication)
            {
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientMedication ");
                scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStartDate = '"); scriptBuilder.Append(CommonLib.UpdateDateFormated(getcomponentDatevalue(s.GetAttributeValue("substanceAdministration/effectiveTime/low[@value]", "value"))));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEndDate = '"); scriptBuilder.Append(CommonLib.UpdateDateFormated(getcomponentDatevalue(s.GetAttributeValue("substanceAdministration/effectiveTime/high[@value]", "value"))));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamINSTRUCTIONS = '"); scriptBuilder.Append(s.GetComment(startpath).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamMedication = '"); scriptBuilder.Append(GetMedicineName(s).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStrength = '"); scriptBuilder.Append(GetStrength(s).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDose = '"); scriptBuilder.Append(GetMedicineDose(s).Replace("'", "''"));
                scriptBuilder.Append("'");
                var doseValue = s.GetAttributeValue("substanceAdministration/doseQuantity[@value]", "value");
                var valueMe = Convert.ToDouble(!string.IsNullOrEmpty(doseValue) ? doseValue:null);
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDoseValue = '"); scriptBuilder.Append(Math.Round(valueMe, 2).ToString());
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamDoseUnit = '"); scriptBuilder.Append(s.FindContentValue("ID", s.GetReference($"{startpath}/entryRelationship[@typeCode='COMP']/substanceAdministration/text")));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamFrequency = '"); scriptBuilder.Append(s.FindContentValue("ID", s.GetReference($"{startpath}/entryRelationship[@typeCode='COMP']/substanceAdministration/text")));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamGenericmed = '"); scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamProviderName = '"); scriptBuilder.Append(s.GetName($"{startpath}/author[1]/assignedAuthor/assignedPerson/name").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamProviderFirstName = '"); scriptBuilder.Append(s.GetName($"{startpath}/author[1]/assignedAuthor/assignedPerson/name").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamProviderMiddleName = '"); scriptBuilder.Append(s.GetName($"{startpath}/author[1]/assignedAuthor/assignedPerson/name/given").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamProviderLastName = '"); scriptBuilder.Append(s.GetName($"{startpath}/author[1]/assignedAuthor/assignedPerson/name/family").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamRxNorm = '"); scriptBuilder.Append(s.GetAttributeValue($"{paths["Rxcode"]}", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatus= '"); scriptBuilder.Append(GetStatus(s));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStatusCode= '"); scriptBuilder.Append(s.GetAttributeValue($"substanceAdministration/statusCode[@code]", "code"));
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
                scriptBuilder.Append("@ParamPatientId = @PatientId,");  //@PatientID,
                scriptBuilder.Append("@ParamFileName = @FileName");
                scriptBuilder.Append(" ;\n");
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
        private string GetStrength(XElement s)
        {
            try
            {
                var refvalue = s.GetAttributeValue(paths["Textref"], "root");
                var medicationNodes = s.XPathSelectElement($"//tr[@ID='{refvalue}']");
                var Strength = medicationNodes.GetElementValue("td[2]");
                           
                return Strength;
            }
            catch (Exception ex)
            {
                return "";
            }
        }   
        private string GetMedicineName(XElement s)
        {
            try
            {
                string MedicationName = s.GetAttributeValue($"{paths["Name"]}", "displayName");
                //string MedicationName = string.Empty;
                if (string.IsNullOrEmpty(MedicationName))
                {
                    MedicationName = s.GetAttributeValue($"{paths["MedicationName"]}", "displayName"); 
                }
                else if (string.IsNullOrEmpty(MedicationName))
                {
                    var refvalue = s.GetAttributeValue(paths["Textref"], "root");
                    var medicationNodes = s.XPathSelectElement($"//tr[@ID='{refvalue}']");
                    MedicationName = medicationNodes.GetElementValue("td[1]");
                    var v2 = medicationNodes.GetElementValue("td[2]");
                    var v3 = medicationNodes.GetElementValue("td[last()-2]");                    
                }
                return MedicationName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private string GetMedicineDose(XElement s)
        {
            try
            {
                var name = s.GetAttributeValue($"{paths["Name"]}", "displayName");
                string MedicationDose = string.Empty;
                if (string.IsNullOrEmpty(name))
                {
                    var refvalue = s.GetAttributeValue(paths["Textref"], "root");
                    var medicationNodes = s.XPathSelectElement($"//tr[@ID='{refvalue}']");
                    MedicationDose = medicationNodes.GetElementValue("td[3]");                    
                }
                return MedicationDose;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private string GetStatus(XElement s)
        {
            try
            {
                var name = s.GetAttributeValue($"{paths["Name"]}", "displayName");
                string MedicineStatus = string.Empty;
                if (string.IsNullOrEmpty(name))
                {
                    var refvalue = s.GetAttributeValue(paths["Textref"], "root");
                    var Status = s.XPathSelectElement($"//tr[@ID='{refvalue}']");
                    MedicineStatus = Status.GetElementValue("td[last()]");
                    
                }
                return MedicineStatus;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
