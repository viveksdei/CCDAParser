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
    public class Demography
    {
        private Dictionary<string, string> paths = new Dictionary<string, string>();
        private string? APUID, MobilePhone, HomePhone, EmergencyContact, WorkPhone, Email, SSN, MRN, NPI;
        string startingPath = "assignedEntity/assignedPerson";
        string AddstartingPath = "assignedEntity/addr[1]";
        string otherPath = "assignedEntity/addr[2]";
        string ResultstartingPath = "playingEntity";
        string AddResultstartingPath = "addr[1]";
        string otherAddResultstartingPath = "addr[2]";
        string AddResultstartingPath1 = "assignedEntity/addr[1]";
        string otherstartingPath = "addr[2]";
        string otherstartingPath1 = "assignedEntityaddr[2]";
        string ProStartingPath = "assignedPerson";
        string ProStartingPath1 = "assignedEntity/assignedPerson";
        public Demography()
        {
            paths.Add("PRole", "recordTarget/patientRole");
            paths.Add("ExternalMappingId", $"{paths["PRole"]}/id");//new
            paths.Add("AssigningAuthorityName", $"{paths["PRole"]}/id[@assigningAuthorityName]");//new
            paths.Add("PatientPrefix", $"{paths["PRole"]}/patient/name/prefix");
            paths.Add("FirstName", $"{paths["PRole"]}/patient/name/given[1]");
            paths.Add("MiddleName", $"{paths["PRole"]}/patient/name/given[2]");
            paths.Add("LastName", $"{paths["PRole"]}/patient/name/family");
            paths.Add("suffix", $"{paths["PRole"]}/patient/name/suffix");
            paths.Add("given2", $"{paths["PRole"]}/patient/name/given[2]");
            paths.Add("Gender", $"{paths["PRole"]}/patient/administrativeGenderCode[@code]");
            paths.Add("DOB", $"{paths["PRole"]}/patient/birthTime[@value]");
            paths.Add("race", $"{paths["PRole"]}/patient/raceCode[@displayName]");
            paths.Add("ethnic", $"{paths["PRole"]}/patient/ethnicGroupCode[@displayName]");
            paths.Add("Languagecode", $"{paths["PRole"]}/patient/languageCommunication/languageCode[@code]");
            paths.Add("MaritalStatus", $"{paths["PRole"]}/patient/maritalStatusCode[@displayName]");

            paths.Add("Address1", $"{paths["PRole"]}/addr[1]/streetAddressLine[1]");
            paths.Add("Address2", $"{paths["PRole"]}/addr[1]/streetAddressLine[2]");
            paths.Add("city", $"{paths["PRole"]}/addr[1]/city");
            paths.Add("state", $"{paths["PRole"]}/addr[1]/state");
            paths.Add("postalCode", $"{paths["PRole"]}/addr[1]/postalCode");
            paths.Add("country", $"{paths["PRole"]}/addr[1]/country");
            paths.Add("OtherAddress1", $"{paths["PRole"]}/addr[2]/streetAddressLine[1]");
            paths.Add("OtherAddress2", $"{paths["PRole"]}/addr[2]/streetAddressLine[2]");
            paths.Add("Othercity", $"{paths["PRole"]}/addr[2]/city");
            paths.Add("Otherstate", $"{paths["PRole"]}/addr[2]/state");
            paths.Add("OtherpostalCode", $"{paths["PRole"]}/addr[2]/postalCode");
            paths.Add("Othercountry", $"{paths["PRole"]}/addr[2]/country");


            paths.Add("Phone", $"{paths["PRole"]}/telecom[0][@value]");            
            paths.Add("Email", $"{paths["PRole"]}/telecom[5][@value]");
            paths.Add("OrganizationName", $"{paths["PRole"]}/providerOrganization/name");
            paths.Add("OrganizationAddr", $"{paths["PRole"]}/providerOrganization/addr/streetAddressLine");
            paths.Add("Organizationcity", $"{paths["PRole"]}/providerOrganization/addr/city");
            paths.Add("Organizationstate", $"{paths["PRole"]}/providerOrganization/addr/state");
            paths.Add("Organizationtelecom", $"{paths["PRole"]}/providerOrganization/telecom[1][@value]");
            paths.Add("OrganizationFax", $"{paths["PRole"]}/providerOrganization/telecom[2][@value]");
            paths.Add("MailId", $"{paths["PRole"]}/providerOrganization/telecom[3][@value]");
            paths.Add("OrganizationpostalCode", $"{paths["PRole"]}/providerOrganization/addr/postalCode");
            paths.Add("OrganizationpostalstreetAddressLine", $"{paths["PRole"]}/providerOrganization/addr/streetAddressLine");
            paths.Add("Organizationcountry", $"{paths["PRole"]}/providerOrganization/addr/country");
            //participant
            paths.Add("participant", "participant/associatedEntity[@classCode='ECON']");
            paths.Add("participantPrefix", $"associatedEntity/associatedPerson/name/prefix");
            paths.Add("participantFirstname", $"associatedEntity/associatedPerson/name/given[1]");
            paths.Add("participantMiddlename", $"associatedEntity/associatedPerson/name/given[2]");
            paths.Add("participantLastname", $"associatedEntity/associatedPerson/name/family");
            paths.Add("participantsuffixname", $"associatedEntity/associatedPerson/name/suffix");
            paths.Add("participantAddr", $"associatedEntity/addr[1]/streetAddressLine[1]");
            paths.Add("participantcity", $"associatedEntity/addr[1]/city");
            paths.Add("participantstate", $"associatedEntity/addr[1]/state");
            paths.Add("participantpostalCode", $"associatedEntity/addr[1]/postalCode");
            paths.Add("participantcountry", $"associatedEntity/addr[1]/country");
            paths.Add("participanttelecom", $"associatedEntity/telecom[@value]");

            //ClinicInformation
            paths.Add("ClinicInformation", "custodian/assignedCustodian/representedCustodianOrganization");
            paths.Add("ClinicName", $"{paths["ClinicInformation"]}/name");
            paths.Add("ClinicWorkPhoneNumber", $"{paths["ClinicInformation"]}/telecom[@value]");
            paths.Add("ClinicAddress1", $"{paths["ClinicInformation"]}/addr[1]/streetAddressLine[1]");
            paths.Add("ClinicAddress2", $"{paths["ClinicInformation"]}/addr[1]/streetAddressLine[2]");
            paths.Add("ClinicCity", $"{paths["ClinicInformation"]}/addr[1]/city");
            paths.Add("ClinicState", $"{paths["ClinicInformation"]}/addr[1]/state");
            paths.Add("ClinicPostalCode", $"{paths["ClinicInformation"]}/addr[1]/postalCode");
            paths.Add("ClinicCountry", $"{paths["ClinicInformation"]}/addr[1]/country");
            paths.Add("ChildRoot", $"{paths["ClinicInformation"]}/id[@root]");

            //Performers
            paths.Add("Performers", "documentationOf/serviceEvent/performer/assignedEntity");
            paths.Add("Child", $"recordTarget");
            paths.Add("SecId", $"ClinicalDocument");


            paths.Add("FirstName1", $"{startingPath}/name/given");
            paths.Add("MiddleName1", $"{startingPath}/name/given[2]");
            paths.Add("LastName1", $"{startingPath}/name/family");
            paths.Add("Suffix1", $"{startingPath}/name/suffix");
            paths.Add("PrefixPath1", $"{startingPath}/name/prefix");
            paths.Add("StreetAddressLine11", $"{AddstartingPath}/streetAddressLine[1]");           
            paths.Add("StreetAddressLine12", $"{AddstartingPath}/streetAddressLine[2]");           
            paths.Add("City1", $"{AddstartingPath}/city");
            paths.Add("State1", $"{AddstartingPath}/state");
            paths.Add("Country1", $"{AddstartingPath}/country");
            paths.Add("PostalCode1", $"{AddstartingPath}/postalCode");
            paths.Add("NPI12", "id[@extension]");
            paths.Add("NPI1", "assignedEntity/id[@extension]");            
            paths.Add("PhoneNumber1", "assignedEntity/telecom[@value]");            
            paths.Add("ResultFirstName", $"{ResultstartingPath}/name/given");
            paths.Add("ResultMiddleName", $"{ResultstartingPath}/name/given[2]");
            paths.Add("ResultLastName", $"{ResultstartingPath}/name/family");
            paths.Add("ResultSuffix", $"{ResultstartingPath}/name/suffix");

            paths.Add("ResultStreetAddressLine1", $"{AddResultstartingPath}/streetAddressLine[1]");
            paths.Add("ResultStreetAddressLine2", $"{AddResultstartingPath}/streetAddressLine[2]");
            paths.Add("ResultCity", $"{AddResultstartingPath}/city");
            paths.Add("ResultState", $"{AddResultstartingPath}/state");
            paths.Add("ResultCountry", $"{AddResultstartingPath}/country");
            paths.Add("ResultPostalCode", $"{AddResultstartingPath}/postalCode");
            paths.Add("ResultPhoneNumber", "telecom[@value]");
            paths.Add("ResultPhoneNumber1", "assignedEntity/telecom[@value]");
            paths.Add("Fax", "telecom[2][@value]");
            paths.Add("Website", "telecom[3][@value]");

            paths.Add("ResultStreetAddressLine1_1", $"{AddResultstartingPath1}/streetAddressLine[1]");
            paths.Add("ResultStreetAddressLine2_1", $"{AddResultstartingPath1}/streetAddressLine[2]");
            paths.Add("ResultCity1", $"{AddResultstartingPath1}/city");
            paths.Add("ResultState1", $"{AddResultstartingPath1}/state");
            paths.Add("ResultCountry1", $"{AddResultstartingPath1}/country");
            paths.Add("ResultPostalCode1", $"{AddResultstartingPath1}/postalCode");

            paths.Add("ProFirstName", $"{ProStartingPath}/name/given");
            paths.Add("ProMiddleName", $"{ProStartingPath}/name/given[2]");
            paths.Add("ProLastName", $"{ProStartingPath}/name/family");
            paths.Add("ProSuffix", $"{ProStartingPath}/name/suffix");
            paths.Add("prefix", $"{ProStartingPath}/name/prefix");

            paths.Add("ProFirstName1", $"{ProStartingPath1}/name/given");
            paths.Add("ProMiddleName1", $"{ProStartingPath1}/name/given[2]");
            paths.Add("ProLastName1", $"{ProStartingPath1}/name/family");
            paths.Add("ProSuffix1", $"{ProStartingPath1}/name/suffix");
            paths.Add("prefix1", $"{ProStartingPath1}/name/prefix");

        }


        public void DemographyReading(XElement element, StringBuilder scriptBuilder, string externalMappingId, string APUID, long orgId, string _fileName, string FacilityCode, string _AssigningAuthorityName,int PatientId)
        {
            scriptBuilder.Append("DECLARE @PatientId BIGINT,");            
            scriptBuilder.Append("@MedicAIzePatientId INT,");            
            scriptBuilder.Append("@CDWOrganizationId BIGINT,");
            scriptBuilder.Append("@ClinicId BIGINT,");
            scriptBuilder.Append("@FileName NVARCHAR(200),");
            scriptBuilder.Append("@APUID NVARCHAR(200),");            
            scriptBuilder.Append("@ExternalMappingId NVARCHAR(200) ; \n");
            scriptBuilder.Append("SET @ExternalMappingId = '"); scriptBuilder.Append(externalMappingId); scriptBuilder.Append("' ;\n");
            scriptBuilder.Append("SET @CDWOrganizationId= "); scriptBuilder.Append(orgId); scriptBuilder.Append(" ;\n");
            scriptBuilder.Append("SET @FileName = '"); scriptBuilder.Append(_fileName); scriptBuilder.Append("' ;\n");
            scriptBuilder.Append("SET @MedicAIzePatientId = "); scriptBuilder.Append(PatientId);
            scriptBuilder.Append("SET @APUID = '"); scriptBuilder.Append(APUID);
            scriptBuilder.Append("'\n");            

            scriptBuilder.Append("\n");
            scriptBuilder.Append("EXEC usp_SaveUpdatePatientOrganizationClinics ");
            scriptBuilder.Append("@OutputClinicId = @ClinicId OUTPUT "); scriptBuilder.Append(",");
            scriptBuilder.Append("@ParamClinicName = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["ClinicName"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamClinicAddress1 = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["ClinicAddress1"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamClinicAddress2 = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["ClinicAddress2"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamCity = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["ClinicCity"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamState= '"); scriptBuilder.Append(element.GetElementValue($"{ paths["ClinicState"] }"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamCountry = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["ClinicCountry"] }"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamZIP = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["ClinicPostalCode"] }"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherAddress1 = '"); scriptBuilder.Append(element.GetElementValue($"{paths["ClinicInformation"]}/addr[2]/streetAddressLine[1]").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherAddress2 = '"); scriptBuilder.Append(element.GetElementValue($"{paths["ClinicInformation"]}/addr[2]/streetAddressLine[2]").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherCity = '"); scriptBuilder.Append(element.GetElementValue($"{paths["ClinicInformation"]}/addr[2]/city").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherState= '"); scriptBuilder.Append(element.GetElementValue($"{paths["ClinicInformation"]}/addr[2]/state"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherCountry = '"); scriptBuilder.Append(element.GetElementValue($"{paths["ClinicInformation"]}/addr[2]/country"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherZip = '"); scriptBuilder.Append(element.GetElementValue($"{paths["ClinicInformation"]}/addr[2]/postalCode"));
            scriptBuilder.Append("'");

            if (!string.IsNullOrEmpty(element.GetAttributeValue($"{paths["ClinicInformation"]}/addr[1][@use]", "use")))
            {
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAddressType = '"); scriptBuilder.Append(element.GetAttributeValue($"{paths["ClinicInformation"]}/addr[1][@use]", "use"));
                scriptBuilder.Append("'");
            }
            else
            {
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAddressType = '"); scriptBuilder.Append(element.GetAttributeValue($"{paths["ClinicInformation"]}/addr[2][@use]", "use"));
                scriptBuilder.Append("'");
            }

            scriptBuilder.Append(","); scriptBuilder.Append("@ParamNPI= '"); scriptBuilder.Append(element.GetAttributeValue("custodian/assignedCustodian/representedCustodianOrganization/id[@extension]", "extension"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamClinicWorkPhoneNumber = '"); scriptBuilder.Append(RemoveTel(element.GetAttributeValue($"{ paths["ClinicWorkPhoneNumber"] }", "value"))); scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append(element.GetAttributeValue("custodian/assignedCustodian/code[@displayName]", "displayName"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append("custodian'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("assignedCustodian'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append(element.GetAttributeValue("custodian/assignedCustodian/code[@code]", "code"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystemName]", "codeSystemName"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(element.GetAttributeValue("custodian/assignedCustodian/representedCustodianOrganization/id[@root]", "root"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOId = '"); scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID");
            scriptBuilder.Append(" ;\n");

            scriptBuilder.Append("\n");
            scriptBuilder.Append("EXEC usp_SaveUpdatePatientDemographics ");
            scriptBuilder.Append("@OutputPatientId = @PatientId OUTPUT ");
            scriptBuilder.Append(",");
            scriptBuilder.Append("@ParamAPUID =  @APUID,");
            scriptBuilder.Append("@ParamCDWOrganizationId =  @CDWOrganizationId,");
            scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
            scriptBuilder.Append("@ParamFileName =  @FileName,");
            scriptBuilder.Append("@ParamMedicAIzePatientId =  @MedicAIzePatientId,");
            scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append("recordTarget"); scriptBuilder.Append("',");
            scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append("patientRole',");
            scriptBuilder.Append("@ParamAssigningAuthorityName = '"); scriptBuilder.Append(_AssigningAuthorityName);
            scriptBuilder.Append("'");

            var Number = element.XPathSelectElements($"{paths["PRole"]}/telecom");
            MobilePhone = string.Empty;
            HomePhone = string.Empty;
            EmergencyContact = string.Empty;
            WorkPhone = string.Empty;
            Email = string.Empty;
            foreach (var item in Number)
            {
                var xzy = item.GetElementValue($"telecom[@use]");
                xzy = item.GetAttributeValue($"telecom[@use]", "use");
                var Use = item.FirstAttribute.Value;
                var Value = item.LastAttribute.Value;

                if (Use == "HP")
                {
                    HomePhone = RemoveTel(item.LastAttribute.Value);                    
                }
                else if (Use == "MC")
                {
                    MobilePhone = RemoveTel(item.LastAttribute.Value);                    
                }
                else if (Use == "EC")
                {
                    EmergencyContact = RemoveTel(item.LastAttribute.Value);                    
                }
                else if (Use == "WP")
                {
                    WorkPhone = RemoveTel(item.LastAttribute.Value);                   
                }
                else
                {                    
                    Email = item.LastAttribute.Value.Contains(':') ? item.LastAttribute.Value.Split(':')[1] : "";
                }
            }

            scriptBuilder.Append(","); scriptBuilder.Append("@ParamFirstName = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["FirstName"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamMiddleName = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["MiddleName"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamLastName = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["LastName"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamPatientPrefix = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["PatientPrefix"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamAddress1 = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["Address1"] }").Replace("'", "''").Replace("UNK", ""));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamAddress2 = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["Address2"] }").Replace("'", "''").Replace("UNK", ""));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamCity = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["city"] }").Replace("'", "''").Replace("UNK", "")); 
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamCountry = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["country"] }").Replace("'", "''").Replace("UNK", ""));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamState = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["state"] }").Replace("'", "''").Replace("UNK", ""));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamDOB = '"); scriptBuilder.Append(element.GetDate($"{ paths["DOB"] }", "value"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherAddress1 = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["OtherAddress1"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherAddress2 = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["OtherAddress2"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherCity = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["Othercity"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherCountry = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["Othercountry"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherState = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["Otherstate"] }").Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherZip = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["OtherpostalCode"] }"));
            scriptBuilder.Append("'");            

            if (!string.IsNullOrEmpty(element.GetAttributeValue($"{paths["PRole"]}/addr[1][@use]", "use")))
            {                
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAddressType = '"); scriptBuilder.Append(element.GetAttributeValue($"{paths["PRole"]}/addr[1][@use]", "use"));
                scriptBuilder.Append("'");
            }
            else
            {
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAddressType = '"); scriptBuilder.Append(element.GetAttributeValue($"{paths["PRole"]}/addr[2][@use]", "use"));
                scriptBuilder.Append("'");               
            }
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamOrganizationName = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["OrganizationName"] }").Replace("'", "''"));
            scriptBuilder.Append("'");            
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamMaritalStatus = '"); scriptBuilder.Append(element.GetAttributeValue($"{ paths["MaritalStatus"] }", "displayName"));
            scriptBuilder.Append("'");           
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamEthnicity = '"); scriptBuilder.Append(element.GetAttributeValue($"{ paths["ethnic"] }", "displayName"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamLanguageCommunication = '"); scriptBuilder.Append(element.GetAttributeValue($"{ paths["Languagecode"] }", "code"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamRace = '"); scriptBuilder.Append(element.GetAttributeValue($"{ paths["race"] }", "displayName"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamGender = '"); scriptBuilder.Append(element.GetAttributeValue($"{ paths["Gender"] }", "displayName"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamZip = '"); scriptBuilder.Append(element.GetElementValue($"{ paths["postalCode"] }"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamPrimaryHomeContactNo = '"); scriptBuilder.Append(HomePhone.Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamMobilePlaceContactNo = '"); scriptBuilder.Append(MobilePhone.Replace("'", "''"));
            scriptBuilder.Append("'");            
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamWorkPlaceContactNo = '"); scriptBuilder.Append(WorkPhone.Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamEmail = '"); scriptBuilder.Append(Email.Replace("'", "''"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamFacilityCode = '"); scriptBuilder.Append(FacilityCode);
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSSN = '"); scriptBuilder.Append("'");             
            string KMPI = GenerateMPINumber.GenratePatientKMPINumber(element.GetElementValue($"{ paths["FirstName"] }").Replace("'", "''"), element.GetElementValue($"{ paths["LastName"] }").Replace("'", "''"), element.GetAttributeValue($"{ paths["DOB"] }", "value"), element.GetAttributeValue($"{ paths["Gender"] }", "displayName"));
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamKMPI = '"); scriptBuilder.Append(KMPI);
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension ='"); scriptBuilder.Append(element.GetAttributeValue($"recordTarget/patientRole/id[1][@extension]", "extension"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot ='"); scriptBuilder.Append(element.GetAttributeValue($"recordTarget/patientRole/id[@root]", "root"));
            scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOId = '"); scriptBuilder.Append("'");            
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append("'");            
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append("'");          
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamClinicID = @ClinicId");
            scriptBuilder.Append(" ;\n");

         
            
            var emergencyContactsData = element.XPathSelectElements("participant");
            foreach (var ecd in emergencyContactsData)
            {                
                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdateEmergencyContacts ");
                scriptBuilder.Append("@ParamSectionName = 'participant',");
                scriptBuilder.Append("@ParamSectionChildName = 'associatedEntity',");
                scriptBuilder.Append("@ParamEmergencyContactType = '"); scriptBuilder.Append(ecd.GetAttributeValue($"associatedEntity[@classCode]", "classCode").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(",");scriptBuilder.Append("@ParamPrefix = '"); scriptBuilder.Append(ecd.GetElementValue($"{ paths["participantPrefix"] }").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEmergencyContactFirstName = '"); scriptBuilder.Append(ecd.GetElementValue($"{ paths["participantFirstname"] }").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEmergencyContactMiddleName = '"); scriptBuilder.Append(ecd.GetElementValue($"{ paths["participantMiddlename"] }").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEmergencyContactLastName = '"); scriptBuilder.Append(ecd.GetElementValue($"{ paths["participantLastname"] }").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCity = '"); scriptBuilder.Append(ecd.GetElementValue($"{ paths["participantcity"] }").Replace("UNK", "").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamState = '"); scriptBuilder.Append(ecd.GetElementValue($"{ paths["participantstate"] }").Replace("UNK", "").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCountry = '"); scriptBuilder.Append(ecd.GetElementValue($"{ paths["participantcountry"] }").Replace("UNK", "").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamZip = '"); scriptBuilder.Append(ecd.GetElementValue($"{ paths["participantpostalCode"] }").Replace("UNK", "").Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStreetAddress1 = '"); scriptBuilder.Append(ecd.GetElementValue($"{ paths["participantAddr"] }").Replace("'", "''").Replace("UNK", ""));
                scriptBuilder.Append("'");

                scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherAddress1 = '"); scriptBuilder.Append(ecd.GetElementValue($"associatedEntity/addr[2]/streetAddressLine[1]").Replace("'", "''").Replace("UNK", ""));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherAddress2 = '"); scriptBuilder.Append(ecd.GetElementValue($"associatedEntity/addr[2]/streetAddressLine[2]").Replace("'", "''").Replace("UNK", ""));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherCity = '"); scriptBuilder.Append(ecd.GetElementValue($"associatedEntity/addr[2]/city").Replace("'", "''").Replace("UNK", ""));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherState = '"); scriptBuilder.Append(ecd.GetElementValue($"associatedEntity/addr[2]/state").Replace("'", "''").Replace("UNK", ""));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherCountry = '"); scriptBuilder.Append(ecd.GetElementValue($"associatedEntity/addr[2]/country").Replace("'", "''").Replace("UNK", ""));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherZip = '"); scriptBuilder.Append(ecd.GetElementValue($"associatedEntity/addr[2]/postalCode").Replace("'", "''").Replace("UNK", ""));
                scriptBuilder.Append("'"); 
                
                if (!string.IsNullOrEmpty(ecd.GetAttributeValue($"associatedEntity/addr[1][@use]", "use")))
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamAddressType = '"); scriptBuilder.Append(ecd.GetAttributeValue($"associatedEntity/addr[1][@use]", "use").Replace("'", "''").Replace("UNK", ""));
                    scriptBuilder.Append("'");
                }
                else
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamAddressType = '"); scriptBuilder.Append(ecd.GetAttributeValue($"associatedEntity/addr[2][@use]", "use").Replace("'", "''").Replace("UNK", ""));
                    scriptBuilder.Append("'");                    
                }


                MobilePhone = string.Empty;
                HomePhone = string.Empty;
                EmergencyContact = string.Empty;
                WorkPhone = string.Empty;
                Email = string.Empty;
                var ContactNumberDetails = ecd.XPathSelectElements($"associatedEntity/telecom");
                foreach (var item in ContactNumberDetails)
                {   
                    var Use = item.FirstAttribute.Value;
                    var Value = item.LastAttribute.Value;

                    if (Use == "HP")
                    {
                        HomePhone = RemoveTel(item.LastAttribute.Value);
                    }
                    else if (Use == "MC")
                    {
                        MobilePhone = RemoveTel(item.LastAttribute.Value);

                    }
                    else if (Use == "EC")
                    {
                        EmergencyContact = RemoveTel(item.LastAttribute.Value);
                    }
                    else if (Use == "WP")
                    {
                        WorkPhone = RemoveTel(item.LastAttribute.Value);

                    }
                    else
                    {
                        Email = (item.LastAttribute.Value.Contains(':') ? item.LastAttribute.Value.Split(':')[1] : "");
                    }
                }

                scriptBuilder.Append(","); scriptBuilder.Append("@ParamHomeContactPhone = '"); scriptBuilder.Append(HomePhone.Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamMobileContactPhone = '"); scriptBuilder.Append(MobilePhone.Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEmergencyContactPhone = '"); scriptBuilder.Append(EmergencyContact.Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamWorkPlaceContactNo = '"); scriptBuilder.Append(WorkPhone.Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEmail = '"); scriptBuilder.Append(Email.Replace("'", "''"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append(ecd.GetAttributeValue("participant/associatedEntity/code[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append(ecd.GetAttributeValue("participant/associatedEntity/code[@codeSystemName]", "codeSystemName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append(ecd.GetAttributeValue("participant/associatedEntity/custodian/assignedCustodian/code[@displayName]", "displayName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(element.GetAttributeValue("participant/associatedEntity/representedCustodianOrganization/id[@root]", "root"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOID = '"); scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamPatientId = @PatientID");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID,");
                scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId,");
                scriptBuilder.Append("@ParamFileName = @FileName");
                scriptBuilder.Append(" ;\n");                
            }
                        

            List<Providers> providerLists = new List<Providers>();
            var personprovider = element.XPathSelectElements($"documentationOf/serviceEvent/performer");
            foreach (var s in personprovider)
            {
                var NPIData = s.XPathSelectElements("assignedEntity/id");
                NPI = string.Empty;
                foreach (var nd in NPIData)
                {
                    if (nd.GetAttributeValue("assigningAuthorityName") == "NPI")
                    {
                        NPI = nd.GetAttributeValue("extension");
                    }
                    else if (nd.GetAttributeValue("assigningAuthorityName") == "National Provider Identifier (NPI) Number")
                    {
                        NPI = nd.GetAttributeValue("extension");
                    }
                }
                //if (!string.IsNullOrEmpty(s.GetElementValue("assignedEntity/assignedPerson/name/given")) && !string.IsNullOrEmpty(NPI))
                //{
                    Providers pl_Documentation = new Providers();                   
                    pl_Documentation.FirstName = s.GetElementValue("assignedEntity/assignedPerson/name/given").Replace("'", "''");
                    pl_Documentation.MiddleName = s.GetElementValue("assignedEntity/assignedPerson/name/given[2]").Replace("'", "''");
                    pl_Documentation.LastName = s.GetElementValue("assignedEntity/assignedPerson/name/family").Replace("'", "''");
                    pl_Documentation.Sufix = s.GetElementValue("assignedEntity/assignedPerson/name/suffix").Replace("'", "''");
                    pl_Documentation.StreetAddressLine1 = s.GetElementValue("assignedEntity/addr[1]/streetAddressLine[1]").Replace("'", "''");
                    pl_Documentation.StreetAddressLine2 = s.GetElementValue("assignedEntity/addr[1]/streetAddressLine[2]").Replace("'", "''");
                    pl_Documentation.Country = s.GetElementValue("assignedEntity/addr[1]/country");
                    pl_Documentation.State = s.GetElementValue("assignedEntity/addr[1]/state");
                    pl_Documentation.City = s.GetElementValue("assignedEntity/addr[1]/city");
                    pl_Documentation.PostalCode = s.GetElementValue("assignedEntity/addr[1]/postalCode");

                    pl_Documentation.OtherAddress1 = s.GetElementValue($"assignedEntity/addr[2]/streetAddressLine[1]").Replace("'", "''");
                    pl_Documentation.OtherAddress2 = s.GetElementValue($"assignedEntity/addr[2]/streetAddressLine[2]").Replace("'", "''");
                    pl_Documentation.OtherCity = s.GetElementValue($"assignedEntity/addr[2]/city");
                    pl_Documentation.OtherCountry = s.GetElementValue($"assignedEntity/addr[2]/country");
                    pl_Documentation.OtherState = s.GetElementValue($"assignedEntity/addr[2]/state");
                    pl_Documentation.OtherZip = s.GetElementValue($"assignedEntity/addr[2]/postalCode");

                    if (!string.IsNullOrEmpty(s.GetAttributeValue($"assignedEntity/addr[1][@use]", "use")))
                    {
                        pl_Documentation.AddressType = s.GetAttributeValue($"assignedEntity/addr[1][@use]", "use");
                    }
                    else
                    {
                        pl_Documentation.AddressType = s.GetAttributeValue($"assignedEntity/addr[2][@use]", "use");
                    }

                    pl_Documentation.Phone = s.GetAttributeValue("assignedEntity/telecom[@value]", "value").Replace("tel:", "");
                    pl_Documentation.NPI = NPI;
                    pl_Documentation.SectionName = "DocumentationOf";
                    pl_Documentation.SectionChildName = "performer";                    
                    pl_Documentation.SectionOID = s.GetAttributeValue("assignedEntity/code[@code]", "code");
                    pl_Documentation.SectionExtension = s.GetAttributeValue($"templateId[@extension]", "extension");
                    pl_Documentation.SectionRoot = s.GetAttributeValue($"templateId[@root]", "root");
                    pl_Documentation.SectionDisplayName = s.GetAttributeValue("code[@displayName]", "displayName");
                    pl_Documentation.SectionCode = s.GetAttributeValue("code[@code]", "code");
                    pl_Documentation.SectionCodeSystemName = s.GetAttributeValue("code[@codeSystemName]", "codeSystemName");                

                    providerLists.Add(pl_Documentation);
                //}
            }



            var maingroup = element.XPathSelectElements("component/structuredBody").Elements().Where(w => w.Name.LocalName == "component");
            foreach (var item in maingroup)
            {
               
                var ename = item.Name.LocalName;                
                var nodes = item.XPathSelectElement("section");                
                var title = nodes.XPathSelectElement("title").Value;               
                switch (title)
                {
                    case "Problems":
                       var datagroup = nodes.XPathSelectElements("entry/act/entryRelationship[1]/observation/informant/assignedEntity");
                        foreach (var dg in datagroup)
                        {                           
                            Providers pro = new Providers();
                            pro.SectionName = title;
                            pro.SectionChildName = "assignedEntity";
                            pro.FirstName = dg.GetElementValue(paths["ProFirstName"]);
                            pro.MiddleName = dg.GetElementValue(paths["ProMiddleName"]);
                            pro.LastName = dg.GetElementValue(paths["ProLastName"]);
                            pro.Sufix = dg.GetElementValue(paths["ProSuffix"]);
                            pro.Prefix = dg.GetElementValue(paths["prefix"]);
                            pro.StreetAddressLine1 = dg.GetElementValue(paths["ResultStreetAddressLine1"]);
                            pro.StreetAddressLine2 = dg.GetElementValue(paths["ResultStreetAddressLine2"]);
                            pro.City = dg.GetElementValue(paths["ResultCity"]);
                            pro.State = dg.GetElementValue(paths["ResultState"]);
                            pro.Country = dg.GetElementValue(paths["ResultCountry"]);
                            pro.PostalCode = dg.GetElementValue(paths["ResultPostalCode"]);
                            pro.Phone = RemoveTel(dg.GetAttributeValue(paths["ResultPhoneNumber"], "value"));                             
                            pro.NPI = FindNPI(dg.GetAttributeValue(paths["NPI12"], "extension"), dg.GetAttributeValue("id[@assigningAuthorityName]", "assigningAuthorityName"));

                            pro.OtherAddress1 = dg.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            pro.OtherAddress2 = dg.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            pro.OtherCity = dg.GetElementValue($"{otherstartingPath}/city");
                            pro.OtherCountry = dg.GetElementValue($"{otherstartingPath}/country");
                            pro.OtherState = dg.GetElementValue($"{otherstartingPath}/state");
                            pro.OtherZip = dg.GetElementValue($"{otherstartingPath}/postalCode");

                            if (!string.IsNullOrEmpty(dg.GetAttributeValue($"addr[1][@use]", "use")))
                            {
                                pro.AddressType = dg.GetAttributeValue($"addr[1][@use]", "use");
                            }
                            else
                            {
                                pro.AddressType = dg.GetAttributeValue($"addr[2][@use]", "use");
                            }

                            providerLists.Add(pro);

                        }
                        break;

                    case "Medications":
                        datagroup = nodes.XPathSelectElements("entry/substanceAdministration/informant/assignedEntity");

                        foreach (var med in datagroup)
                        {                            
                            Providers pm = new Providers();
                            pm.SectionName = title;
                            pm.SectionChildName = "assignedEntity";
                            pm.FirstName = med.GetElementValue(paths["ProFirstName"]);
                            pm.MiddleName = med.GetElementValue(paths["ProMiddleName"]);
                            pm.LastName = med.GetElementValue(paths["ProLastName"]);
                            pm.Sufix = med.GetElementValue(paths["ProSuffix"]);
                            pm.Prefix = med.GetElementValue(paths["prefix"]);
                            pm.StreetAddressLine1 = med.GetElementValue(paths["ResultStreetAddressLine1"]);
                            pm.StreetAddressLine2 = med.GetElementValue(paths["ResultStreetAddressLine2"]);
                            pm.City = med.GetElementValue(paths["ResultCity"]);
                            pm.State = med.GetElementValue(paths["ResultState"]);
                            pm.Country = med.GetElementValue(paths["ResultCountry"]);
                            pm.PostalCode = med.GetElementValue(paths["ResultPostalCode"]);
                            pm.Phone = RemoveTel(med.GetAttributeValue(paths["ResultPhoneNumber"], "value"));                               
                            pm.NPI = FindNPI(med.GetAttributeValue(paths["NPI12"], "extension"), med.GetAttributeValue("id[@assigningAuthorityName]", "assigningAuthorityName"));

                            pm.OtherAddress1 = med.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            pm.OtherAddress2 = med.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            pm.OtherCity = med.GetElementValue($"{otherstartingPath}/city");
                            pm.OtherCountry = med.GetElementValue($"{otherstartingPath}/country");
                            pm.OtherState = med.GetElementValue($"{otherstartingPath}/state");
                            pm.OtherZip = med.GetElementValue($"{otherstartingPath}/postalCode");

                            if (!string.IsNullOrEmpty(med.GetAttributeValue($"addr[1][@use]", "use")))
                            {
                                pm.AddressType = med.GetAttributeValue($"addr[1][@use]", "use");
                            }
                            else
                            {
                                pm.AddressType = med.GetAttributeValue($"addr[2][@use]", "use");
                            }

                            providerLists.Add(pm);
                        }

                        break;

                    case "Allergies":
                        datagroup = nodes.XPathSelectElements("entry/act/entryRelationship/observation/informant/assignedEntity");
                        foreach (var data in datagroup)
                        {
                            Providers pa = new Providers();
                            pa.SectionName = title;
                            pa.SectionChildName = "assignedEntity";
                            pa.FirstName = data.GetElementValue(paths["ProFirstName"]);
                            pa.MiddleName = data.GetElementValue(paths["ProMiddleName"]);
                            pa.LastName = data.GetElementValue(paths["ProLastName"]);
                            pa.Sufix = data.GetElementValue(paths["ProSuffix"]);
                            pa.Prefix = data.GetElementValue(paths["prefix"]);
                            pa.StreetAddressLine1 = data.GetElementValue(paths["ResultStreetAddressLine1"]);
                            pa.StreetAddressLine2 = data.GetElementValue(paths["ResultStreetAddressLine2"]);
                            pa.City = data.GetElementValue(paths["ResultCity"]);
                            pa.State = data.GetElementValue(paths["ResultState"]);
                            pa.Country = data.GetElementValue(paths["ResultCountry"]);
                            pa.PostalCode = data.GetElementValue(paths["ResultPostalCode"]);
                            pa.Phone = RemoveTel(data.GetAttributeValue(paths["ResultPhoneNumber"], "value"));                            
                            pa.NPI = FindNPI(data.GetAttributeValue(paths["NPI12"], "extension"), data.GetAttributeValue("id[@assigningAuthorityName]", "assigningAuthorityName"));

                            pa.OtherAddress1 = data.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            pa.OtherAddress2 = data.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            pa.OtherCity = data.GetElementValue($"{otherstartingPath}/city");
                            pa.OtherCountry = data.GetElementValue($"{otherstartingPath}/country");
                            pa.OtherState = data.GetElementValue($"{otherstartingPath}/state");
                            pa.OtherZip = data.GetElementValue($"{otherstartingPath}/postalCode");

                            if (!string.IsNullOrEmpty(data.GetAttributeValue($"addr[1][@use]", "use")))
                            {
                                pa.AddressType = data.GetAttributeValue($"addr[1][@use]", "use");
                            }
                            else
                            {
                                pa.AddressType = data.GetAttributeValue($"addr[2][@use]", "use");
                            }

                            providerLists.Add(pa);
                        }

                        break;

                    case "Procedures":
                        datagroup = nodes.XPathSelectElements("entry/procedure/performer/assignedEntity");
                        foreach (var dgr in datagroup)
                        {
                            Providers pp = new Providers();
                            pp.SectionName = title;
                            pp.SectionChildName = "assignedEntity";
                            pp.FirstName = dgr.GetElementValue(paths["ProFirstName"]);
                            pp.MiddleName = dgr.GetElementValue(paths["ProMiddleName"]);
                            pp.LastName = dgr.GetElementValue(paths["ProLastName"]);
                            pp.Sufix = dgr.GetElementValue(paths["ProSuffix"]);
                            pp.Prefix = dgr.GetElementValue(paths["prefix"]);
                            pp.StreetAddressLine1 = dgr.GetElementValue(paths["ResultStreetAddressLine1"]);
                            pp.StreetAddressLine2 = dgr.GetElementValue(paths["ResultStreetAddressLine2"]);
                            pp.City = dgr.GetElementValue(paths["ResultCity"]);
                            pp.State = dgr.GetElementValue(paths["ResultState"]);
                            pp.Country = dgr.GetElementValue(paths["ResultCountry"]);
                            pp.PostalCode = dgr.GetElementValue(paths["ResultPostalCode"]);
                            pp.Phone = RemoveTel(dgr.GetAttributeValue(paths["ResultPhoneNumber"], "value"));                            
                            pp.NPI = FindNPI(dgr.GetAttributeValue(paths["NPI12"], "extension"), dgr.GetAttributeValue("id[@assigningAuthorityName]", "assigningAuthorityName"));
                            pp.OtherAddress1 = dgr.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            pp.OtherAddress2 = dgr.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            pp.OtherCity = dgr.GetElementValue($"{otherstartingPath}/city");
                            pp.OtherCountry = dgr.GetElementValue($"{otherstartingPath}/country");
                            pp.OtherState = dgr.GetElementValue($"{otherstartingPath}/state");
                            pp.OtherZip = dgr.GetElementValue($"{otherstartingPath}/postalCode");

                            if (!string.IsNullOrEmpty(dgr.GetAttributeValue($"addr[1][@use]", "use")))
                            {
                                pp.AddressType = dgr.GetAttributeValue($"addr[1][@use]", "use");
                            }
                            else
                            {
                                pp.AddressType = dgr.GetAttributeValue($"addr[2][@use]", "use");
                            }

                            providerLists.Add(pp);

                        }


                        break;

                    case "Immunizations":
                        datagroup = nodes.XPathSelectElements("entry/substanceAdministration/performer");
                        foreach (var itemdata in datagroup)
                        {
                            Providers ppi = new Providers();
                            ppi.SectionName = title;
                            ppi.SectionChildName = "performer";
                            ppi.FirstName = itemdata.GetElementValue(paths["FirstName1"]);
                            ppi.MiddleName = itemdata.GetElementValue(paths["MiddleName1"]);
                            ppi.LastName = itemdata.GetElementValue(paths["LastName1"]);
                            ppi.Sufix = itemdata.GetElementValue(paths["Suffix1"]);
                            ppi.Prefix = itemdata.GetElementValue(paths["PrefixPath1"]);
                            ppi.StreetAddressLine1 = itemdata.GetElementValue(paths["StreetAddressLine11"]);
                            ppi.StreetAddressLine2 = itemdata.GetElementValue(paths["StreetAddressLine12"]);
                            ppi.City = itemdata.GetElementValue(paths["City1"]);
                            ppi.State = itemdata.GetElementValue(paths["State1"]);
                            ppi.Country = itemdata.GetElementValue(paths["Country1"]);
                            ppi.PostalCode = itemdata.GetElementValue(paths["PostalCode1"]);
                            ppi.Phone = RemoveTel(itemdata.GetAttributeValue(paths["PhoneNumber1"], "value"));                            
                            ppi.NPI = FindNPI(itemdata.GetAttributeValue(paths["NPI12"], "extension"), itemdata.GetAttributeValue("id[@assigningAuthorityName]", "assigningAuthorityName"));

                            ppi.OtherAddress1 = itemdata.GetElementValue($"{otherPath}/streetAddressLine[1]");
                            ppi.OtherAddress2 = itemdata.GetElementValue($"{otherPath}/streetAddressLine[1]");
                            ppi.OtherCity = itemdata.GetElementValue($"{otherPath}/city");
                            ppi.OtherCountry = itemdata.GetElementValue($"{otherPath}/country");
                            ppi.OtherState = itemdata.GetElementValue($"{otherPath}/state");
                            ppi.OtherZip = itemdata.GetElementValue($"{otherPath}/postalCode");

                            if (!string.IsNullOrEmpty(itemdata.GetAttributeValue($"assignedEntity/addr[1][@use]", "use")))
                            {
                                ppi.AddressType = itemdata.GetAttributeValue($"assignedEntity/addr[1][@use]", "use");
                            }
                            else
                            {
                                ppi.AddressType = itemdata.GetAttributeValue($"assignedEntity/addr[2][@use]", "use");
                            }

                            providerLists.Add(ppi);
                        }

                        break;

                    case "Vital Signs":
                        datagroup = nodes.XPathSelectElements("entry/organizer/author/assignedAuthor");
                        foreach (var s in datagroup)
                        {
                            Providers ppv = new Providers();
                            ppv.SectionName = title;
                            ppv.SectionChildName = "assignedAuthor";
                            ppv.FirstName = s.GetElementValue(paths["ProFirstName"]);
                            ppv.MiddleName = s.GetElementValue(paths["ProMiddleName"]);
                            ppv.LastName = s.GetElementValue(paths["ProLastName"]);
                            ppv.Sufix = s.GetElementValue(paths["ProSuffix"]);
                            ppv.Prefix = s.GetElementValue(paths["prefix"]);
                            ppv.StreetAddressLine1 = s.GetElementValue(paths["ResultStreetAddressLine1"]);
                            ppv.StreetAddressLine2 = s.GetElementValue(paths["ResultStreetAddressLine2"]);
                            ppv.City = s.GetElementValue(paths["ResultCity"]);
                            ppv.State = s.GetElementValue(paths["ResultState"]);
                            ppv.Country = s.GetElementValue(paths["ResultCountry"]);
                            ppv.PostalCode = s.GetElementValue(paths["ResultPostalCode"]);
                            ppv.Phone = RemoveTel(s.GetAttributeValue(paths["ResultPhoneNumber"], "value"));                            
                            ppv.NPI = FindNPI(s.GetAttributeValue(paths["NPI12"], "extension"), s.GetAttributeValue("id[@assigningAuthorityName]", "assigningAuthorityName"));

                            ppv.OtherAddress1 = s.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            ppv.OtherAddress2 = s.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            ppv.OtherCity = s.GetElementValue($"{otherstartingPath}/city");
                            ppv.OtherCountry = s.GetElementValue($"{otherstartingPath}/country");
                            ppv.OtherState = s.GetElementValue($"{otherstartingPath}/state");
                            ppv.OtherZip = s.GetElementValue($"{otherstartingPath}/postalCode");

                            if (!string.IsNullOrEmpty(s.GetAttributeValue($"addr[1][@use]", "use")))
                            {
                                ppv.AddressType = s.GetAttributeValue($"addr[1][@use]", "use");
                            }
                            else
                            {
                                ppv.AddressType = s.GetAttributeValue($"addr[2][@use]", "use");
                            }

                            providerLists.Add(ppv);
                        }

                        break;

                    case "Lab Test & Results":
                        datagroup = nodes.XPathSelectElements("entry/organizer/participant/participantRole");
                        foreach (var s in datagroup)
                        {
                            Providers ppls = new Providers();
                            ppls.SectionName = title;
                            ppls.SectionChildName = "participantRole";
                            ppls.FirstName = s.GetElementValue(paths["ResultFirstName"]);
                            ppls.LastName = s.GetElementValue(paths["ResultLastName"]);
                            ppls.Sufix = s.GetElementValue(paths["ResultSuffix"]);
                            ppls.Prefix = s.GetElementValue(paths["prefix"]);
                            ppls.StreetAddressLine1 = s.GetElementValue(paths["ResultStreetAddressLine1"]);
                            ppls.StreetAddressLine2 = s.GetElementValue(paths["ResultStreetAddressLine2"]);
                            ppls.City = s.GetElementValue(paths["ResultCity"]);
                            ppls.State = s.GetElementValue(paths["ResultState"]);
                            ppls.Country = s.GetElementValue(paths["ResultCountry"]);
                            ppls.PostalCode = s.GetElementValue(paths["ResultPostalCode"]);
                            ppls.Phone = RemoveTel(s.GetAttributeValue(paths["ResultPhoneNumber"], "value"));                            
                            ppls.NPI = FindNPI(s.GetAttributeValue(paths["NPI12"], "extension"), s.GetAttributeValue("id[@assigningAuthorityName]", "assigningAuthorityName"));
                            ppls.OtherAddress1 = s.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            ppls.OtherAddress2 = s.GetElementValue($"{otherstartingPath}/streetAddressLine[1]");
                            ppls.OtherCity = s.GetElementValue($"{otherstartingPath}/city");
                            ppls.OtherCountry = s.GetElementValue($"{otherstartingPath}/country");
                            ppls.OtherState = s.GetElementValue($"{otherstartingPath}/state");
                            ppls.OtherZip = s.GetElementValue($"{otherstartingPath}/postalCode");

                            if (!string.IsNullOrEmpty(s.GetAttributeValue($"addr[1][@use]", "use")))
                            {
                                ppls.AddressType = s.GetAttributeValue($"addr[1][@use]", "use");
                            }
                            else
                            {
                                ppls.AddressType = s.GetAttributeValue($"addr[2][@use]", "use");
                            }

                            providerLists.Add(ppls);

                        }

                        break;
                    default:
                        //throw new Exception("new property found");
                        break;
                }
            }


            var GproviderList = providerLists.GroupBy(item => new {
                item.FirstName,
                item.MiddleName,
                item.LastName,
                item.Prefix,
                item.Sufix,
                item.StreetAddressLine1,
                item.StreetAddressLine2,
                item.Country,
                item.State
            }).
            Select(group => group.FirstOrDefault()).ToList();

            foreach (var plist in GproviderList)
            {
                //if(!string.IsNullOrEmpty(plist.NPI))
                //{
                    scriptBuilder.Append("\n");
                    scriptBuilder.Append("EXEC usp_SaveUpdatePatientProvider ");
                    scriptBuilder.Append("@ParamSectionName = '"); scriptBuilder.Append(plist.SectionName); scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionChildName = '"); scriptBuilder.Append(plist.SectionChildName);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamFirstName = '"); scriptBuilder.Append(plist.FirstName.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamMiddleName  = '"); scriptBuilder.Append(plist.MiddleName.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamLastName = '"); scriptBuilder.Append(plist.LastName.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamSufix = '"); scriptBuilder.Append(plist.Sufix);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamPhone = '"); scriptBuilder.Append(plist.Phone);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamCity = '"); scriptBuilder.Append(plist.City.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamState = '"); scriptBuilder.Append(plist.State.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamCountry = '"); scriptBuilder.Append(plist.Country.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamPostalCode = '"); scriptBuilder.Append(plist.PostalCode);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamStreetAddressLine1 = '"); scriptBuilder.Append(plist.StreetAddressLine1.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamStreetAddressLine2 = '"); scriptBuilder.Append(plist.StreetAddressLine2.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherAddress1 = '"); scriptBuilder.Append(plist.OtherAddress1.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherAddress2 = '"); scriptBuilder.Append(plist.OtherAddress2.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherCity = '"); scriptBuilder.Append(plist.OtherCity.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherState = '"); scriptBuilder.Append(plist.OtherState.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherCountry = '"); scriptBuilder.Append(plist.OtherCountry.Replace("'", "''"));
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamOtherZip = '"); scriptBuilder.Append(plist.OtherZip);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamAddressType = '"); scriptBuilder.Append(plist.AddressType);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamNPI = '"); scriptBuilder.Append(plist.NPI);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append(plist.SectionCodeSystemName);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append(plist.SectionCode);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append(plist.SectionDisplayName);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(plist.SectionRoot);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOID = '"); scriptBuilder.Append(plist.SectionOID);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append(plist.SectionExtension);
                    scriptBuilder.Append("'");
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID =  @APUID,");
                    scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId");
                    scriptBuilder.Append(" ;\n");
                //}                
            }
        }



        private string RemoveTel(string Tel)
        {
            if (!string.IsNullOrEmpty(Tel))
                return Tel.Contains(':') ? Tel.Split(':')[1] : "";
            else
                return "";
        }

        private string FindNPI(string NPI, string DisplayNpi)
        {
            if (DisplayNpi == "National Provider Identifier (NPI) Number")
            {

                if (NPI == "")
                {
                    return null;
                }
                else
                {
                    return NPI;

                }
            }
            else
            {
                return null;
            }

        }
    }
}
