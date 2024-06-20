using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCDA_Parser.Parser.Common
{
    public static class Extension
    {
        public static void WriteToFile(string Message)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                if (!File.Exists(filepath))
                {
                    // Create a file to write to.   
                    using (StreamWriter sw = File.CreateText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
            }
            catch (Exception Ex)
            {
                Extension.WriteToFile("Exception :" + Ex.StackTrace + "Source" + Ex.Source + "InnerEXP:" + Ex.Message);
            }
        }

        public static void WriteToExceptionLog(string Message)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\ExceptionLog";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\ExceptionLog\\FileError" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                if (!File.Exists(filepath))
                {
                    // Create a file to write to.   
                    using (StreamWriter sw = File.CreateText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
            }
            catch (Exception Ex)
            {
                Extension.WriteToFile("Exception :" + Ex.StackTrace + "Source" + Ex.Source + "InnerEXP:" + Ex.Message);
            }
        }

        public static void WriteToScriptLog(string Message, string _FacilityCode)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Script";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Script\\" + _FacilityCode + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                if (!File.Exists(filepath))
                {
                    // Create a file to write to.   
                    using (StreamWriter sw = File.CreateText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
            }
            catch (Exception Ex)
            {
                Extension.WriteToFile("Exception :" + Ex.StackTrace + "Source" + Ex.Source + "InnerEXP:" + Ex.Message);
            }
        }

        //This fncction is used to genrate MPI number with 4 Parameter and 4th parameter is Gender
        public static string GenratePatientKMPINumber(string firstName, string lastName, string dob, string gender)
        {
            string comContent = string.Concat(firstName.ToLower(), lastName.ToLower(), dob, gender.ToLower().Substring(0, 1));

            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(comContent));
                var result = Extension.ToHex(hash, true);
                return result;
            }

        }
        public static string ToHex(this byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }

        public static void MoveFile(string SourceFilePath, string DestinationFilePath)
        {
            // Extension.WriteToFile("moving Files" );
            if (File.Exists(DestinationFilePath))
            {
                System.IO.File.Delete(DestinationFilePath);
            }
            System.IO.File.Copy(SourceFilePath, DestinationFilePath);
            System.IO.File.Delete(SourceFilePath);
        }
    }
}
