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
    public class PlanofTreatment
    {
        Dictionary<string, string> paths = new Dictionary<string, string>();

        public PlanofTreatment()
        {
            paths.Add("allergyref", $"act / entryRelationship / observation / value / originalText ");
            paths.Add("cpt", $"code[@codeSystem='{CodeSystems.CPTCodesystemOID}']");
            paths.Add("imo", $"code/translation[@codeSystem='{CodeSystems.IMOCodeCodesystemOID}']");
            paths.Add("icd10cm", $"code/translation[@codeSystem='{CodeSystems.ICD10CMCodesystemOID}']");
            paths.Add("icd9cm", $"code/translation[@codeSystem='{CodeSystems.ICD9CMCodesystemOID}']");
            paths.Add("icd10pcs", $"code/translation[@codeSystem='{CodeSystems.ICD10PCSCodesystemOID}']");
            paths.Add("icd9pcs", $"code/translation[@codeSystem='{CodeSystems.ICD9PCSCodesystemOID}']");
            paths.Add("hcpcs", $"code/translation[@codeSystem='{CodeSystems.HCPCSCodesystemOID}']");
            paths.Add("loinc", $"code/translation[@codeSystem='{CodeSystems.LOINCCodesystemOID}']");
            paths.Add("snomed", $"code[@codeSystem='{CodeSystems.SNOMEDCTCodesystemOID}']");
            paths.Add("vcpt", $"value[@codeSystem='{CodeSystems.CPTCodesystemOID}']");
            paths.Add("vimo", $"value/translation[@codeSystem='{CodeSystems.IMOCodeCodesystemOID}']");
            paths.Add("vicd10cm", $"value/translation[@codeSystem='{CodeSystems.ICD10CMCodesystemOID}']");
            paths.Add("vicd9cm", $"value/translation[@codeSystem='{CodeSystems.ICD9CMCodesystemOID}']");
            paths.Add("vicd10pcs", $"value/translation[@codeSystem='{CodeSystems.ICD10PCSCodesystemOID}']");
            paths.Add("vicd9pcs", $"value/translation[@codeSystem='{CodeSystems.ICD9PCSCodesystemOID}']");
            paths.Add("vhcpcs", $"value/translation[@codeSystem='{CodeSystems.HCPCSCodesystemOID}']");
            paths.Add("vloinc", $"value/translation[@codeSystem='{CodeSystems.LOINCCodesystemOID}']");
            paths.Add("vsnomed", $"value[@codeSystem='{CodeSystems.SNOMEDCTCodesystemOID}']");
            paths.Add("performer", $"performer/assignedEntity/assignedPerson/name");
            paths.Add("des", $"code/originalText");

        }

        public void PlanofTreatmentReading(XElement element, StringBuilder scriptBuilder, string _ExternalMappingId, string FileName, string _APUID)
        {
            var planOfCare = element.XPathSelectElements("text/list/item");
            var planOfCare1 = element.XPathSelectElements("text/paragraph");
            var sectionDetails = element;
            string sectionName = sectionDetails.GetElementValue($"title");
            int i = 0;
            int j = 0;
            int k = 0;
            int trCount = 1;

            List<PlanOfCare> list = new List<PlanOfCare>();
            List<PlanOfCare> list2 = new List<PlanOfCare>();

            var DataEmpty = false;
            foreach (var items in planOfCare1)
            {
                PlanOfCare Plans = new PlanOfCare();
                var PlanData1 = items.IsEmpty;
                if (DataEmpty == PlanData1)
                {
                    Plans.Description = items.LastNode.ToString().Replace("'", "");
                    list.Add(Plans);
                    list2.AddRange(list);
                }
            }

            foreach (var item in planOfCare)
            {
                list = new List<PlanOfCare>();
                PlanOfCare AllPlans = new PlanOfCare();
                i++;
                j++;
                k++;
                var paragraph1 = item.XPathSelectElements("paragraph");


                var paragraph2 = item.XPathSelectElements("list/item/paragraph");
                if (paragraph2.Count() == 0)
                {
                    paragraph2 = item.XPathSelectElements("list/item");
                }

                var paragraph = ("text/list/item[" + i + "]/paragraph[" + j + "]");

                string mas = null;
                string MasTy = null;

                foreach (var Ma in paragraph1)
                {
                    PlanOfCare PlansMaster = new PlanOfCare();
                    Boolean available = false;
                    if (Ma.IsEmpty != true)
                    {
                        var vvv = Ma.LastNode.ToString().Replace("'", "");

                        if (string.IsNullOrEmpty(mas))
                        {
                            mas = Ma.GetElementValue("caption");
                            if (!string.IsNullOrEmpty(mas))
                            {
                                PlansMaster.Description = mas;
                                list.Add(PlansMaster);
                            }
                            available = true;
                        }
                        if (string.IsNullOrEmpty(mas))
                        {
                            mas = Ma.LastNode.ToString().Replace("'", "");
                            PlansMaster.Description = mas;
                            list.Add(PlansMaster);
                            available = true;
                        }

                        var SplitData1 = (Ma.LastNode.ToString()).Split('-');
                        if (SplitData1.Count() != 1)
                        {
                            var CodeData = (Ma.LastNode.ToString()).Split('-');
                            if (CodeData.Count() == 2)
                            {
                                var countdata = CodeData[0];
                                if ((CodeData[0].Trim()).Count() == 5)
                                {
                                    PlansMaster.Description = mas;
                                    PlansMaster.ICDCode = CodeData[0];
                                    PlansMaster.ICDCodedDescription = CodeData[1].Replace("'", ""); ;
                                    list.Add(PlansMaster);
                                    available = true;
                                }
                                else
                                {
                                    PlansMaster.Description = mas;
                                    PlansMaster.ICDCodedDescription = (Ma.LastNode.ToString()).Replace("'", "");
                                    list.Add(PlansMaster);
                                    available = true;
                                }
                            }
                        }
                        if (available == false)
                        {
                            PlansMaster.Description = mas;
                            PlansMaster.POCType = Ma.LastNode.ToString().Replace("'", "");
                            list.Add(PlansMaster);
                        }
                    }

                }

                string PocTy = null;
                foreach (var Ty in paragraph2)
                {
                    Boolean PocTyAvailable = false;
                    PlanOfCare PlansTypeCode = new PlanOfCare();
                    if (string.IsNullOrEmpty(PocTy))
                    {
                        PocTy = Ty.GetElementValue("caption");
                        PocTyAvailable = true;
                        
                        if (string.IsNullOrEmpty(PocTy))
                        {
                            if (Ty.IsEmpty != true)
                            {
                                var CodeData = (Ty.LastNode.ToString()).Split('-');
                                if (CodeData.Count() == 3 || CodeData.Count() == 2)
                                {
                                    PocTyAvailable = false;
                                }
                                else
                                {
                                    PlansTypeCode.POCType = (Ty.LastNode.ToString());
                                    list.Add(PlansTypeCode);
                                    PocTy = null;
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(PocTy))
                            {
                                PlansTypeCode.POCType = PocTy;
                                list.Add(PlansTypeCode);
                                PocTy = null;
                            }

                        }

                    }
                    var vwc = Ty.GetElementValue("paragraph");
                    var PlanData = Ty.IsEmpty;

                    if (PlanData != true)
                    {
                        if (PocTyAvailable == false)
                        {
                            var CodeData = (Ty.LastNode.ToString()).Split('-');
                            if (CodeData.Count() == 2)
                            {
                                var countdata = CodeData[0].Replace("<content>", "").Replace("*", "");
                                var bh = Ty.GetElementValue("content");
                                if (((CodeData[0].Replace("<content>", "")).Trim().Replace("*", "")).Count() == 5)
                                {
                                    PlansTypeCode.Description = mas;
                                    PlansTypeCode.POCType = PocTy;
                                    PlansTypeCode.ICDCode = CodeData[0].Replace("*", "");
                                    PlansTypeCode.ICDCodedDescription = CodeData[1].Replace("'", "");
                                    list.Add(PlansTypeCode);
                                }
                                else
                                {
                                    PlansTypeCode.Description = mas;
                                    PlansTypeCode.POCType = PocTy;
                                    PlansTypeCode.ICDCodedDescription = (Ty.LastNode.ToString()).Replace("'", "");
                                    list.Add(PlansTypeCode);
                                }

                            }
                            else if (CodeData.Count() == 3)
                            {
                                var countdata = CodeData[0].Replace("<content>", "").Replace("*", "");
                                var bh = Ty.GetElementValue("content");
                                if (((CodeData[0].Replace("<content>", "")).Trim().Replace("*", "")).Count() == 5)
                                {
                                    PlansTypeCode.Description = mas;
                                    PlansTypeCode.POCType = PocTy;
                                    PlansTypeCode.ICDCode = (CodeData[0].Replace("<content>", "")).Trim().Replace("*", "");
                                    PlansTypeCode.ICDCodedDescription = CodeData[1] + " " + CodeData[2].Replace("'", "");
                                    list.Add(PlansTypeCode);
                                }
                                else
                                {
                                    PlansTypeCode.Description = mas;
                                    PlansTypeCode.POCType = PocTy;
                                    PlansTypeCode.ICDCodedDescription = (Ty.LastNode.ToString()).Replace("'", "");
                                    list.Add(PlansTypeCode);
                                }
                            }
                            else
                            {
                                PlansTypeCode.Description = mas;
                                PlansTypeCode.POCType = PocTy;
                                list.Add(PlansTypeCode);
                            }
                        }
                    }
                }
                list2.AddRange(list);
            }

            foreach (var AllData in list2)
            {

                scriptBuilder.Append("\n");
                scriptBuilder.Append("EXEC usp_SaveUpdatePatientPlanOfCare ");
                scriptBuilder.Append("@ParamSectionName= '"); scriptBuilder.Append(element.XPathSelectElement("title").Value); scriptBuilder.Append("',");
                scriptBuilder.Append("@ParamSectionChildName= '"); scriptBuilder.Append("section'");
                if (!string.IsNullOrEmpty(AllData.Description))
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamDescription = '"); scriptBuilder.Append(AllData.Description.Replace("'", "''"));
                    scriptBuilder.Append("'");
                }
                else
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamDescription = '"); scriptBuilder.Append("'");
                }

                if (!string.IsNullOrEmpty(AllData.POCType))
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamPOCType = '"); scriptBuilder.Append(AllData.POCType.Replace("'", "''"));
                    scriptBuilder.Append("'");
                }
                else
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamPOCType = '"); scriptBuilder.Append("'");
                }

                scriptBuilder.Append(","); scriptBuilder.Append("@ParamICDCode = '"); scriptBuilder.Append(AllData.ICDCode);
                scriptBuilder.Append("'");

                if (!string.IsNullOrEmpty(AllData.ICDCodedDescription))
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamICDCodedDescription = '"); scriptBuilder.Append(AllData.ICDCodedDescription.Replace("'", "''"));
                    scriptBuilder.Append("'");
                }
                else
                {
                    scriptBuilder.Append(","); scriptBuilder.Append("@ParamICDCodedDescription = '"); scriptBuilder.Append("'");
                }

                scriptBuilder.Append(","); scriptBuilder.Append("@ParamType = '"); scriptBuilder.Append(AllData.Type);
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamLOINC = '"); scriptBuilder.Append(AllData.LOINC);
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamStartDate = '"); scriptBuilder.Append(AllData.StartDate);
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamEndDate = '"); scriptBuilder.Append(AllData.EndDate);
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCode = '"); scriptBuilder.Append(element.GetAttributeValue("code[@code]", "code"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionDisplayName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@displayName]", "displayName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionCodeSystemName = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystemName]", "codeSystemName"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionRoot = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@root]", "root"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionOID = '"); scriptBuilder.Append(element.GetAttributeValue("code[@codeSystem]", "codeSystem"));
                scriptBuilder.Append("'");
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionExtension = '"); scriptBuilder.Append(element.GetAttributeValue($"templateId[@extension]", "extension"));
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
