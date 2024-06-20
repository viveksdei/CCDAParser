using CCDA_Parser.Parser;
using CCDA_Parser.Parser.Common;
using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CCDA_Parser.DAL
{
    public class CCDAParserDAL
    {
        public async Task<int> XmlDataParhing(int PatientId, string FileName, string ccdaFile, string con)
        {
            IDbConnection db = new SqlConnection(con);
            //HWS.Common.DBLink DBObject;
            string FacilityCode= "KPCA_FC";
            int orgID=3;
            string FullName = ccdaFile; //FileName;

            int _result = 2;
            try
            {
                _result = 1;
                string finalScript = string.Empty;
                string _APUID = string.Empty;
                string _AssigningAuthorityName = string.Empty;
                string _ExternalMappingId = string.Empty;
                XmlDocument document = new XmlDocument();
                //document.Load(FullName);
                document.LoadXml(FullName);

                string xmltext = document.InnerXml;
                xmltext = xmltext.Replace("xmlns=\"urn:hl7-org:v3\"", "").Replace("xsi:type=\"CD\"", "");
                XDocument doc = XDocument.Parse(xmltext);
                var Demography = doc.XPathSelectElement("ClinicalDocument");
                document.LoadXml(xmltext);

                var Encounter = doc.XPathSelectElement("ClinicalDocument/componentOf");
                 var xmlcheck = document.LastChild.LastChild.LastChild;

                document = null;
                xmltext = string.Empty;
                var maingroup = doc.XPathSelectElement("ClinicalDocument/component/structuredBody").Elements().Where(w => w.Name.LocalName == "component");
                doc = null;
                StringBuilder scriptBuilder = new StringBuilder();
                scriptBuilder.Append("BEGIN \nSET XACT_ABORT ON \nBEGIN TRANSACTION \n");

                _APUID = string.Empty;
                if (FacilityCode == "PTCKY_FC" || FacilityCode == "DSHA_FC")
                {
                    _APUID = Demography.GetAttributeValue("recordTarget/patientRole/providerOrganization/id[@extension]", "extension");

                    var ids = Demography.XPathSelectElements($"recordTarget/patientRole/id");
                    foreach (var item in ids)
                    {
                        if (item.GetAttributeValue("assigningAuthorityName").ToUpper() == "E-MDS")
                        {
                            _ExternalMappingId = item.GetAttributeValue("extension");

                            _AssigningAuthorityName = item.GetAttributeValue("assigningAuthorityName");
                        }
                    }
                }
                else
                {
                    var AuthorInfo = Demography.XPathSelectElements($"author");
                    foreach (var Auhtor in AuthorInfo)
                    {
                        _APUID = "NULL";//CommonLib.AuthorInfos(Auhtor);
                        if (!string.IsNullOrWhiteSpace(_APUID))
                            break;
                    }

                    _ExternalMappingId = Demography.GetAttributeValue($"recordTarget/patientRole/id[@extension]", "extension");
                    if (string.IsNullOrEmpty(_ExternalMappingId))
                    {
                        throw new ArgumentException("_ExternalMappingId is null");
                    }
                    _AssigningAuthorityName = Demography.GetAttributeValue($"recordTarget/patientRole/id", "assigningAuthorityName");
                }

                new Demography().DemographyReading(Demography, scriptBuilder, _ExternalMappingId, _APUID, orgID, FileName, FacilityCode, _AssigningAuthorityName, PatientId);

                var encountersNode = Demography.XPathSelectElement("componentOf");
                if (encountersNode != null)
                {
                    new Encounters().EncountersReading(encountersNode, scriptBuilder, _ExternalMappingId, orgID, FileName, _APUID);
                }


                try
                {
                    foreach (var element in maingroup)
                    {
                        var ename = element.Name.LocalName;
                        var nodes = element.XPathSelectElement("section");
                        //var title1 = nodes.XPathSelectElement("title").Value;

                        var codeNode = nodes.Descendants("code").FirstOrDefault();
                        var title = codeNode.Attribute("code").Value;

                        switch (title)
                        {
                            case "11450-4": //problems
                                new Problems().ProblemsReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            case "10160-0": //medications
                                new Medications().MedicationsReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            case "48765-2": //allergies
                                new Allergies().AllergiesReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            case "47519-4": //Procedures
                                new Procedures().ProceduresReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            case "29762-2": //social history
                                new SocialHistory().SocialHistoryReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            case "8716-3": //vital signs
                                new VitalSigns().VitalSignsReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            case "30954-2": //lab test & results": //30954-2
                                new LabTestAndResults().LabTestAndResultsReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            case "18776-5": //plan of treatment
                                new PlanofTreatment().PlanofTreatmentReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            case "11369-6": //Immunizations
                                new Immunizations().ImmunizationsReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            case "47420-5": //assessment
                                new Assessment().AssessmentReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            case "10190-7": //mental status
                                new MentalStatus().MentalStatusReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            //case "functional status": //47420-5
                            //    new FunctionalStatus().FunctionalStatusReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                            //    break;

                            //case "Interventions": //62387-6
                            //    new Interventions().InterventionsReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                            //    break;

                            //case "health concerns":
                            //    new Health_Concerns().HealthConcernsReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                            //    break;

                            //case "referrals":
                            //    new Referrals().ReferralsReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID, FacilityCode);
                            //    break;

                            //case "health status evaluations/outcomes":
                            //    new HealthStatusAndEvaluationsOutcomes().HealthStatusAndEvaluationsOutcomesReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                            //    break;

                            case "61146-7": //goals
                                new Goals().GoalsReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            case "46240-8": //encounter diagnosis
                                new EncounterDiagnosis().EncounterDiagnosisReading(nodes, scriptBuilder, _ExternalMappingId, FileName, _APUID);
                                break;

                            default:
                                break;
                        }

                      }

                    scriptBuilder.Append("\nCOMMIT Transaction \nEND \n");

                    finalScript = scriptBuilder.ToString();

                        var parameter = new DynamicParameters();
                    var importPatientList = await db.QueryMultipleAsync(finalScript, null,null, null, CommandType.Text);

                    //DBObject.GetDatasetFor(finalScript);
                }

                catch (Exception ex)
                {
                    Extension.WriteToExceptionLog(DateTime.Now.ToString() + "|XMLDataProcessing|" + ex.ToString());
                    _result = 0;
                }

                if (finalScript == "" || _result == 0)
                {
                    Extension.WriteToScriptLog(finalScript, FacilityCode);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);

                Extension.WriteToExceptionLog(DateTime.Now.ToString() + "|XMLDataProcessing|" + ex.ToString());
                _result = 0;
            }
            return _result;
        }

    }
}
