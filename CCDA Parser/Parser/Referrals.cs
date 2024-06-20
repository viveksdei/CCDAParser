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
    public class Referrals
    {

        public void ReferralsReading(XElement element, StringBuilder scriptBuilder, string externalMappingID, string FileName, string _APUID, string FacilityCode)
        {
            var Referral = element.XPathSelectElements("text/paragraph");
            var tabledata = element.XPathSelectElements("text/table/tbody/tr");
            int trCount = 1;
            if (FacilityCode == "DSHA_FC")
            {
                foreach (var s in tabledata)
                {
                    scriptBuilder.Append("\n");
                    scriptBuilder.Append("EXEC usp_SaveUpdatePatientReferrals ");
                    scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                    scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");

                    string tblReferringProvider = element.GetReferralstable(element, $"GetProvider", 1, trCount, 1);
                    string[] ReferringProviderList = tblReferringProvider.Split(':');
                    if (ReferringProviderList.Length > 0)
                    {
                        string ReferringProviderdata = ReferringProviderList[0].Replace("tel", "").ToString();
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamReferringProvider = '"); scriptBuilder.Append(ReferringProviderdata.Replace("'", "''"));
                        scriptBuilder.Append("'");                       
                    }
                    if (ReferringProviderList.Length > 1)
                    {                       
                        scriptBuilder.Append(","); scriptBuilder.Append("@ParamReferringProviderNumber = '"); scriptBuilder.Append(ReferringProviderList[1].ToString());
                        scriptBuilder.Append("'");
                    }
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamDateofReferral = '"); scriptBuilder.Append(CommonLib.UpdateDateFormated(element.GetReferralstable(element, $"GetDateofReferral", 2, trCount, 2)));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamReasonforReferral = '"); scriptBuilder.Append(element.GetReferralstable(element, $"GetReasonfor", 3, trCount, 3).Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamTransitioningProvider = '"); scriptBuilder.Append(element.GetReferralstable(element, $"GetTransitioningProvider", 4, trCount, 4));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamLoincCode = '"); scriptBuilder.Append(element.GetAttributeValue($"code[@code]", "code"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamReferralsDescription = '"); scriptBuilder.Append(element.GetElementValue($"text/paragraph").Replace("'", "''"));
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

            else
            {
                foreach (var s in Referral)
                {
                    scriptBuilder.Append("\n");
                    scriptBuilder.Append("EXEC usp_SaveUpdatePatientReferrals ");
                    scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                    scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("section'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamLoincCode = '"); scriptBuilder.Append(element.GetAttributeValue($"code[@code]", "code"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamReferralsDescription = '"); scriptBuilder.Append(element.GetElementValue($"text/paragraph").Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamReferringProvider = '"); scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamReferringProviderNumber = '"); scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamDateofReferral = '"); scriptBuilder.Append("'");                  
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamReasonforReferral = '"); scriptBuilder.Append("'");                   
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamTransitioningProvider = '"); scriptBuilder.Append("'");
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
           
        }
    }
}
