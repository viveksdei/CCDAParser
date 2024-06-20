using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CCDA_Parser.Parser.Common
{
    public static class CommonLib
    {
        public static void GetCode(XElement code, StringBuilder scriptBuilder, string externalMappingID, string sectionChildName, string sectionName, string description = null)
        {
            scriptBuilder.Append("\n");
            scriptBuilder.Append("EXEC usp_SaveUpdateCodeDetail ");
            if (code.Attribute("code") != null)
            { scriptBuilder.Append("@ParamCode  ='"); scriptBuilder.Append(code.GetAttributeValue("code")); scriptBuilder.Append("'"); }
            else
            {
                scriptBuilder.Append("@ParamCode  =null");
            }

            if (code.Attribute("codeSystemName") != null)
            { scriptBuilder.Append(","); scriptBuilder.Append("@ParamCodeSystemName  ='"); scriptBuilder.Append(code.GetAttributeValue("codeSystemName")); scriptBuilder.Append("'"); }
            else
            {
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCodeSystemName  =null");
            }

            if (code.Attribute("displayName") != null)
            { scriptBuilder.Append(","); scriptBuilder.Append("@ParamCodeDisplayName  ='"); scriptBuilder.Append(code.GetAttributeValue("displayName").Replace("'", "''")); scriptBuilder.Append("'"); }
            else
            {
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCodeDisplayName  =null");
            }

            if (code.Parent.Element("value") != null)
            {
                if (code.Parent.Element("value").Attribute("value") != null)
                { scriptBuilder.Append(","); scriptBuilder.Append("@ParamValue  ='"); scriptBuilder.Append(code.Parent.GetAttributeValue("value[@value]", "value")); scriptBuilder.Append("'"); }
                else
                { scriptBuilder.Append(","); scriptBuilder.Append("@ParamValue  =null"); }
            }
            else
            { scriptBuilder.Append(","); scriptBuilder.Append("@ParamValue  =null"); }

            if (code.Parent.Element("value") != null)
            {
                if (code.Parent.Element("value").Attribute("unit") != null)
                { scriptBuilder.Append(","); scriptBuilder.Append("@ParamUnit  ='"); scriptBuilder.Append(code.Parent.GetAttributeValue("value[@unit]", "unit")); scriptBuilder.Append("'"); }
                else
                { scriptBuilder.Append(","); scriptBuilder.Append("@ParamUnit  =null"); }
            }
            else
            { scriptBuilder.Append(","); scriptBuilder.Append("@ParamUnit  =null"); }
            if (string.IsNullOrEmpty(description))
            {
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCodeDescription  =null");
            }
            else
            {
                scriptBuilder.Append(","); scriptBuilder.Append("@ParamCodeDescription  ='"); scriptBuilder.Append(description); scriptBuilder.Append("'");
            }
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionName  ='"); scriptBuilder.Append(sectionName); scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamSectionChildName  = '"); scriptBuilder.Append(sectionChildName); scriptBuilder.Append("'");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamPatientid  =@PatientID");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamAPUID  =@APUID");
            scriptBuilder.Append(","); scriptBuilder.Append("@ParamExternalMappingId = @ExternalMappingId");
            scriptBuilder.Append(" ;\n");


        }

        public static string GetDate(this XElement node, string xpath, string attributename)
        {
            try
            {
                var date = node.GetAttributeValue(xpath, attributename);//observation/effectiveTime[@value]


                if (date.Length >= 4)
                {
                    if (date.Contains("-"))
                    {
                        date = date.Substring(0, date.IndexOf("-"));
                    }
                    string[] formats = new string[] { "yyyy", "yyyyMMdd", "yyyyMMddHHmmss" };
                    DateTime returndate = DateTime.ParseExact(date, formats, null, System.Globalization.DateTimeStyles.None);
                    string returnFormatedDate = returndate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    return returnFormatedDate;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string UpdateDateFormated(string DateValue)
        {
            try
            {
                DateTime dDate;
                if (DateTime.TryParse(DateValue, out dDate))
                {
                    var Data = DateTime.Parse(DateValue).ToString("yyyy-MM-dd");
                    return Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static string SHGetDate(this XElement node, string xpath, string attributename)
        {
            try
            {
                var date = node.GetAttributeValue(xpath, attributename);//observation/effectiveTime[@value]
                if (date == "")
                {
                    date = node.GetAttributeValue("observation/effectiveTime/low[@value]", attributename);
                }

                if (date.Length >= 4)
                {
                    if (date.Contains("-"))
                    {
                        date = date.Substring(0, date.IndexOf("-"));
                    }
                    string[] formats = new string[] { "yyyy", "yyyyMMdd", "yyyyMMddHHmmss" };
                    DateTime returndate = DateTime.ParseExact(date, formats, null, System.Globalization.DateTimeStyles.None);
                    string returnFormatedDate = returndate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    return returnFormatedDate;
                   
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string GetContentByReference(this XNode node, string path)
        {
            return node.FindContentValue("ID", node.GetReference(path));
        }
        public static string GetContentByReference(this XNode node, string path, string attr)
        {
            return node.FindContentValue(attr, node.GetReference(path));
        }
        public static string GetReference(this XNode node)
        {
            string result = "";
            var element = node.XPathSelectElement("text/reference[@value]");
            if (element != null)
            {
                result = element.GetAttributeValue("value").Replace("#", "");
            }
            return result;
        }
        public static string GetReference(this XNode node, string xpath)
        {
            string result = "";
            var element = node.XPathSelectElement($"{xpath}/reference[@value]");
            if (element != null)
            {
                result = element.GetAttributeValue("value").Replace("#", "");
            }
            return result;
        }
        public static string GetTableReference(this XNode node, XNode Mainnode, string xpath)
        {
            string result = "";
            var element = node.XPathSelectElement(xpath);
            if (element != null)
            {
                result = Mainnode.FindTableContentValue("ID", GetAttributeValue(element, "root"));
            }
            return result;
        }
        public static string FindContentValue(this XNode node, string attributename, string value)
        {
            string result = "";
            var element = node.XPathSelectElement($"//content[@{attributename}='{value}']");
            if (element != null && !element.HasElements)
            {
                result = element.Value;
            }
            return result;
        }

        public static string AuthorInfos(XElement element)
        {
            string result = "";
            var APUID = element.GetAttributeValue($"assignedAuthor/representedOrganization/id[@extension]", "extension");
            return APUID;

        }



        public static string FindTableContentValue(this XNode node, string attributename, string value)
        {
            string result = "";
            var element = node.XPathSelectElements($"text/table/tbody/tr[@{attributename}='{value}']/td[2]");
            if (element != null)
            {
                result = element.FirstOrDefault().Value;
            }
            return result;
        }

        public static string GetAttributeValue(this XElement node, string attributename)
        {
            string result = "";
            if (node != null)
            {
                result = node.Attribute(attributename) != null ? node.Attribute(attributename).Value : "";
            }
            return result;
        }
        public static string GetMedicineName(this XNode node, XNode Mainnode, string xpath, int type, int? trCount, int? tdCount)
        {
            return Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]").FirstOrDefault().Value;
        }



        public static string GetHealthStatusEvaluations_Outcomes(this XNode node, XNode Mainnode, string xpath, int type, int? trCount, int? tdCount)
        {
            string DataResult = "";
            if (type == 1)
            {
                DataResult = Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]").FirstOrDefault().Value;
            }
            if (type == 2)
            {
                DataResult = Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]/content[1]").FirstOrDefault().Value;
            }
            if (type == 3)
            {
                DataResult = Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]/content[2]").FirstOrDefault().Value;
            }
            return DataResult;
        }

        public static string GetNotes(this XNode node, XNode Mainnode, string xpath, int type, int? trCount, int? tdCount)
        {

            return Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]/content").FirstOrDefault().Value;
        }

        public static string GetSectionDetails(this XNode node, XNode Mainnode, string xpath, int type, int? trCount, int? tdCount)
        {

            return Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]").FirstOrDefault().Value;
        }

        public static string GetSectionNotes(this XNode node, XNode Mainnode, string xpath, int type, int? trCount, int? tdCount)
        {
            string InterventionsResult = "";
            var MainData = Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]/content");
            string DataR = "";
            foreach (var item in MainData)
            {
                InterventionsResult = item.Value;
                DataR = DataR + "" + InterventionsResult;
            }

            //if (type==2)
            //{
            //    InterventionsResult= Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]/content[1]").FirstOrDefault().Value;
            //}
            //if(type==3)
            //{
            //    InterventionsResult = Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]/content[2]").FirstOrDefault().Value;
            //}

            //if (type == 4)
            //{
            //    InterventionsResult = Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]/content[3]").FirstOrDefault().Value;
            //}
            return DataR;
        }



        public static string GetTableReferenceEncounterDiagnosis(this XNode node, XElement Mainnode, string xpath, int type, int? trCount, int? tdCount)
        {
            string result = "";
            var element = node.XPathSelectElement(xpath);

            if (type == 1)
                result = Mainnode.GetElementValue($"text/list/item[" + trCount + "]/paragraph[1]");
            else if (type == 2)
                result = Mainnode.GetElementValue($"text/list/item[" + trCount + "]/paragraph[2]");
            else if (type == 3)
                result = Mainnode.FindTableContentValueEncounterDiagnosis("", GetAttributeValue(element, ""), type, trCount, tdCount);
            return result;
        }

        public static string FindTableContentValueEncounterDiagnosis(this XNode node, string attributename, string value, int type, int? trCount, int? tdCount)
        {
            string result = "";
            int ElementCount;
            IEnumerable<XElement> element = null;
            if (type == 1)
            {
                element = node.XPathSelectElements($"text/list/item/tr[" + trCount + "]/th[" + tdCount + "]");

                //trCount++;
            }
            else if (type == 2)
            {
                element = node.XPathSelectElements($"text/table[1]/tbody/tr[" + trCount + "]/td[" + tdCount + "]");
                //element = node.XPathSelectElements($"text/table/tbody/tr[@{attributename}='{value}']/td[2]");
            }
            else if (type == 3)
            {
                element = node.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]");
            }
            //if (element != null)
            {
                if (element.Count() != 0)
                {
                    result = element.FirstOrDefault().Value;
                }
            }
            return result;
        }
        public static string GetTableReferencePlanOfCare(this XNode nodes, XNode Mainnode, string xpath, int type, int? trCount, int? tdCount, string node)
        {
            string result = "";
            var element = Mainnode.XPathSelectElement(xpath);

            if (type == 1)
                result = Mainnode.FindTableContentValuePlanOfCare("", GetAttributeValue(element, "root"), 1, trCount, tdCount, node);
            else if (type == 2)
                result = Mainnode.FindTableContentValuePlanOfCare("", GetAttributeValue(element, "root"), 2, trCount, tdCount, node);
            else if (type == 3)
                result = Mainnode.FindTableContentValuePlanOfCare("", GetAttributeValue(element, ""), type, trCount, tdCount, node);
            return result;
        }

        public static string FindTableContentValuePlanOfCare(this XNode node, string attributename, string value, int type, int? trCount, int? tdCount, string nodeData)
        {
            string result = "";
            int ElementCount;
            IEnumerable<XElement> element = null;
            if (type == 1)
            {
                element = node.XPathSelectElements($"text/table/thead/tr[" + 1 + "]/th[" + tdCount + "]");

                //trCount++;
            }
            else if (type == 2)
            {
                element = node.XPathSelectElements(nodeData);
                //element = node.XPathSelectElements($"text/table/tbody/tr[@{attributename}='{value}']/td[2]");
            }
            else if (type == 3)
            {
                element = node.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]");
            }
            //if (element != null)
            {
                if (element.Count() != 0)
                {
                    result = element.FirstOrDefault().Value;
                }
            }
            return result;
        }

        public static string GetTableReferenceVital(this XNode node, XNode Mainnode, string xpath, int type, int? trCount, int? tdCount)
        {
            string result = "";
            var element = node.XPathSelectElement(xpath);

            if (type == 1)
                result = Mainnode.FindTableContentValueVital("", GetAttributeValue(element, "root"), 1, trCount, tdCount);
            else if (type == 2)
                result = Mainnode.FindTableContentValueVital("", GetAttributeValue(element, "root"), 2, trCount, tdCount);
            else if (type == 3)
                result = Mainnode.FindTableContentValueVital("", GetAttributeValue(element, ""), type, trCount, tdCount);
            return result;
        }

        public static string FindTableContentValueVital(this XNode node, string attributename, string value, int type, int? trCount, int? tdCount)
        {
            string result = "";
            int ElementCount;
            IEnumerable<XElement> element = null;
            if (type == 1)
            {
                element = node.XPathSelectElements($"text/table/thead/tr[" + 1 + "]/th[" + tdCount + "]");

                //trCount++;
            }
            else if (type == 2)
            {
                element = node.XPathSelectElements($"text/table[1]/tbody/tr[" + trCount + "]/td[" + tdCount + "]");
                //element = node.XPathSelectElements($"text/table/tbody/tr[@{attributename}='{value}']/td[2]");
            }
            else if (type == 3)
            {
                element = node.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]");
            }
            //if (element != null)
            {
                if (element.Count() != 0)
                {
                    result = element.FirstOrDefault().Value;
                }
            }
            return result;
        }



        public static string GetTableReference(this XNode node, XNode Mainnode, string xpath, int type, int? trCount, int? tdCount)
        {
            string result = "";
            var element = node.XPathSelectElement(xpath);

            if (type == 1)
                result = Mainnode.FindTableContentValue("", GetAttributeValue(element, "root"), 1, trCount, tdCount);
            else if (type == 2)
                result = Mainnode.FindTableContentValue("", GetAttributeValue(element, "root"), 2, trCount, tdCount);
            else if (type == 3)
                result = Mainnode.FindTableContentValue("", GetAttributeValue(element, ""), type, trCount, tdCount);
            else if (type == 5)
                result = Mainnode.FindTableContentValue("", GetAttributeValue(element, "root"), 5, trCount, tdCount);

            return result;
        }

        public static string FindTableContentValue(this XNode node, string attributename, string value, int type, int? trCount, int? tdCount)
        {
            string result = "";
            int ElementCount;
            IEnumerable<XElement> element = null;
            if (type == 1)
            {
                element = node.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]");

                trCount++;
            }
            else if (type == 2)
            {
                element = node.XPathSelectElements($"text/table[1]/tbody/tr[" + trCount + "]/td[" + tdCount + "]");
                //element = node.XPathSelectElements($"text/table/tbody/tr[@{attributename}='{value}']/td[2]");
            }
            else if (type == 3)
            {
                element = node.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]");
            }
            else if (type == 5)
            {
                element = node.XPathSelectElements($"text/table[2]/tbody/tr[" + trCount + "]/td[" + tdCount + "]");
            }
            //if (element != null)
            {
                if (element.Count() != 0)
                {
                    result = element.FirstOrDefault().Value;
                }
            }
            return result;
        }
        public static string GetAttributeValue(this XElement node, string xpath, string attributename)
        {
            string result = "";
            if (node != null)
            {

                var ele = node.XPathSelectElement(xpath);
                if (ele != null)
                {
                    result = ele.GetAttributeValue(attributename);
                }

            }
            return result;

        }

        public static string GetReferralstable(this XNode node, XNode Mainnode, string xpath, int type, int? trCount, int? tdCount)
        {
            string DataResult = "";
            if (type == 1)
            {
                DataResult = Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]").FirstOrDefault().Value;
            }
            if (type == 2)
            {
                DataResult = Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]").FirstOrDefault().Value;
            }
            if (type == 3)
            {
                DataResult = Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]").FirstOrDefault().Value;
            }
            if (type == 4)
            {
                DataResult = Mainnode.XPathSelectElements($"text/table/tbody/tr[" + trCount + "]/td[" + tdCount + "]").FirstOrDefault().Value;
            }
            return DataResult;
        }



        public static string GetComment(this XNode node, string xpath)
        {
            return node.GetValueByReference(xpath, "SUBJ");
        }

        public static string GetStatus(this XNode node, string xpath)
        {
            return node.GetValueByReference(xpath, "REFR");
        }
        private static string GetValueByReference(this XNode node, string xpath, string typecode)
        {
            try
            {
                string commentref = "";
                //if (html != null)
                //{
                var ent = node.XPathSelectElement($"{xpath}/entryRelationship[@typeCode='{typecode}']");
                if (ent != null)
                {
                    if (typecode == "REFR")
                    {
                        var nameele = node.XPathSelectElement($"{xpath}/entryRelationship/value[@displayName]");
                        if (nameele != null)
                        {
                            commentref = nameele.GetAttributeValue("displayName");
                            if (commentref.Length > 0)
                            {
                                return commentref;
                            }
                        }
                    }
                    commentref = ent.FirstNode.GetReference();
                }
                commentref = node.FindContentValue("ID", commentref);
                //}
                return commentref;

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #region name section
        public static string GetName(this XNode node, string xpath)
        {
            try
            {
                string name = "";
                //name += node.GetElementValue($"{xpath}/family");
                //name += " " + node.GetElementValue($"{xpath}/given");
                //name += " " + node.GetElementValue($"{xpath}/suffix");


                name += node.GetElementValue($"{xpath}/given");
                if (node.GetElementValue($"{xpath}/given[2]") != "")
                {
                    name += " " + node.GetElementValue($"{xpath}/given[2]");
                }
                name += " " + node.GetElementValue($"{xpath}/family");
                if (node.GetElementValue($"{xpath}/suffix") != "")
                {
                    name += " " + node.GetElementValue($"{xpath}/suffix");
                }

                name = name.TrimStart().TrimEnd();
                return name;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        #endregion
        public static string GetElementValue(this XNode node, string xpath)
        {
            try
            {
                var result = node.XPathSelectElement(xpath);
                if (result != null)
                    return Convert.ToString(result.Value);
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static DateTime ConvertStringToDate(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return DateTime.MinValue;
            }
            DateTime returnVal = DateTime.MinValue;
            DateTime.TryParse(value, out returnVal);
            return returnVal;
        }
        public static int ConvertGenderStringToInt(this string value)
        {
            if (value?.ToLower() == "male")
            {
                return 1;
            }
            else if (value?.ToLower() == "female")
            {
                return 2;
            }
            else if (value?.ToLower() == "other")
            {
                return 3;
            }
            else if (value?.ToLower() == "unknown")
            {
                return 4;
            }
            else
            {
                return 4;
            }
        }
        public static int ConvertMaritalStatusStringToInt(this string value)
        {
            string maritalStatus = value?.ToLower();
            switch (maritalStatus)
            {
                case "single":
                    return 1;
                case "unknown":
                    return 5;
                case "divorced":
                    return 3;
                case "widowed":
                    return 4;
                case "married":
                    return 2;
                case "partner":
                    return 13;
                case "legally separated":
                    return 6;
                default:
                    return 5;
            }
        }
    }
}
