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
    public class Immunizations
    {
        string startpath = "substanceAdministration";
        Dictionary<string, string> paths = new Dictionary<string, string>();

        public Immunizations()
        {
            paths.Add("consumable", $"{startpath}/consumable/manufacturedProduct/manufacturedMaterial");
            paths.Add("CVX", $"{paths["consumable"]}/code[@codeSystemName='CVX']");
            paths.Add("Name", $"{paths["consumable"]}/code[@displayName]");
            paths.Add("Textref", $"{paths["consumable"]}/code/originalText");
            paths.Add("VaccineLotNumber", $"{paths["consumable"]}");
            paths.Add("EntryRelationship", $"{startpath}/entryRelationship/observation");
            paths.Add("VaccineStatus", $"{paths["EntryRelationship"]}/statusCode");
            paths.Add("ExternalOrganiZationid", $"substanceAdministration/performer/assignedEntity/representedOrganization/id[@extension]");
            paths.Add("TextrefOther", $"{startpath}/consumable/manufacturedProduct/manufacturedMaterial");
            paths.Add("PerformerName", $"substanceAdministration/performer/assignedEntity/assignedPerson/name");
        }

        public void ImmunizationsReading(XElement element, StringBuilder scriptBuilder, string externalMappingID, string FileName, string _APUID)
        {
            var Immunization = element.XPathSelectElements("entry"); 
            int trCount = 1;
            foreach (var s in Immunization)
            {
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientImmunizations ");
                scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");
                var DateVaccine = getcomponentDatevalue(s.GetAttributeValue("substanceAdministration/effectiveTime[@value]", "value")).ToString();                
                if (!string.IsNullOrEmpty(DateVaccine))
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamVaccineDate = '"); scriptBuilder.Append(CommonLib.UpdateDateFormated(DateVaccine));
                    scriptBuilder.Append("'");
                }
                else
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamVaccineDate = '"); scriptBuilder.Append("'");
                }
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCvxCode = '"); scriptBuilder.Append(s.GetAttributeValue("substanceAdministration/consumable/manufacturedProduct/manufacturedMaterial/code[@code]", "code").TrimStart('0'));
                scriptBuilder.Append("'");
                var AdministeredDose = s.GetAttributeValue($"{startpath}/doseQuantity[@value]", "value");                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAdministeredDose= '"); scriptBuilder.Append(GetUnit(AdministeredDose));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamExternalOrganizationId = '"); scriptBuilder.Append(s.GetAttributeValue($"{paths["ExternalOrganiZationid"]}", "extension"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAdministeredDoseUnits = '"); scriptBuilder.Append(s.GetAttributeValue($"{startpath}/doseQuantity[@unit]", "unit"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamVaccineLotNumber = '"); scriptBuilder.Append(s.GetElementValue($"{paths["TextrefOther"]}/lotNumberText"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamVaccineStatus = '"); scriptBuilder.Append(s.GetAttributeValue($"{paths["VaccineStatus"]}[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamPerformerName= '"); scriptBuilder.Append(s.GetName(paths["PerformerName"]).Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append(element.GetAttributeValue("code[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOId = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystem]", "codeSystem"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystemName]", "codeSystemName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@extension]", "extension"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@displayName]", "displayName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@root]", "root"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID,");
                scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
                scriptBuilder.Append("@ParamPatientId = @PatientID,");
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

        private string GetUnit(string str)
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
