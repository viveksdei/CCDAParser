using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCDA_Parser.Parser.Common
{
    public static class GenerateMPINumber
    {
        public static string GetPatientMPINumber(string patientSsnNumber)
        {
            return EncrptDecryptPassword.Encrypt(patientSsnNumber);
        }
        public static string GetPatientSSNFromMPI(string patientMPI)
        {
            return EncrptDecryptPassword.Decrypt(patientMPI);
        }
        //This fncction is used to genrate MPI number with 4 Parameter and 4th parameter is patientAccId
        public static Guid GetPatientMPI(string firstName, string lastName, string dob, string patientAccId)
        {
            string comContent = string.Concat(firstName.ToLower(), lastName, dob, patientAccId);

            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(comContent));
                Guid result = new Guid(hash);
                return result;
            }

        }
        //This fncction is used to genrate MPI number with 4 Parameter and 4th parameter is Gender
        public static string GenratePatientKMPINumber(string firstName, string lastName, string dob, string gender)
        {
            if (!string.IsNullOrEmpty(dob) && !string.IsNullOrEmpty(gender))
            {
                string comContent = string.Concat(firstName.ToLower(), lastName.ToLower(), dob, gender.ToLower().Substring(0, 1));

                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(comContent));
                    var result = GenerateMPINumber.ToHex(hash, true);
                    return result;
                }
            }
            else
            {
                string result = string.Empty;
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
    }
    public class EncrptDecryptPassword
    {
        #region Encrypt

        private static byte[] key = { };
        private static byte[] IV = { 38, 55, 206, 48, 28, 64, 20, 16 };
        private static string stringKey = "!5663a#KN";

        public static string Encrypt(string text)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));

                //des = new DESCryptoServiceProvider();
                Byte[] byteArray = Encoding.UTF8.GetBytes(text);

                //MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);

                cryptoStream.Write(byteArray, 0, byteArray.Length);
                cryptoStream.FlushFinalBlock();

                return Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                des.Dispose();
                memoryStream.Dispose();
            }
        }
        #endregion

        #region Decrypt

        public static string Decrypt(string text)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));


                Byte[] byteArray = Convert.FromBase64String(text);


                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                    des.CreateDecryptor(key, IV), CryptoStreamMode.Write);

                cryptoStream.Write(byteArray, 0, byteArray.Length);
                cryptoStream.FlushFinalBlock();

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                des.Dispose();
                memoryStream.Dispose();
            }
        }
        #endregion
    }
}
