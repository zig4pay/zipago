using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZREL.ZiPago.Libreria.Seguridad
{
    public sealed class Criptografia
    {
        private static readonly string strArchivoJson = "appsettings.json";
        private static readonly string strClave = "ZRELZiPago:ZZiPagoK";

        public static string Encriptar(string cadenaEncriptar)
        {
            return Encriptar(cadenaEncriptar, ObtenerKey());
        }

        public static string Desencriptar(string cadenaEncriptada)
        {            
            return Desencriptar(cadenaEncriptada, ObtenerKey());
        }

        public static string Encriptar(string cadenaEncriptar, byte[] bytPK)
        {
            Rijndael rijndael = Rijndael.Create();
            byte[] array = null;
            byte[] array2 = null;

            try
            {
                rijndael.Key = bytPK;
                rijndael.GenerateIV();
                byte[] bytes = Encoding.UTF8.GetBytes(cadenaEncriptar);
                array = rijndael.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
                array2 = new byte[rijndael.IV.Length + array.Length];
                rijndael.IV.CopyTo(array2, 0);
                array.CopyTo(array2, rijndael.IV.Length);
                return Convert.ToBase64String(array2);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                rijndael.Clear();
            }
        }

        public static string Desencriptar(string cadenaDesencriptar, byte[] bytPK)
        {
            Rijndael rijndael = Rijndael.Create();
            byte[] array = Convert.FromBase64String(cadenaDesencriptar);
            byte[] array2 = new byte[rijndael.IV.Length];
            byte[] array3 = new byte[array.Length - rijndael.IV.Length];
            string empty = string.Empty;
            try
            {
                rijndael.Key = bytPK;
                Array.Copy(array, array2, array2.Length);
                Array.Copy(array, array2.Length, array3, 0, array3.Length);
                rijndael.IV = array2;
                return Encoding.UTF8.GetString(rijndael.CreateDecryptor().TransformFinalBlock(array3, 0, array3.Length));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                rijndael.Clear();
            }
        }

        public static byte[] ObtenerKey()
        {
            string strDirectorio = Directory.GetCurrentDirectory();
            IConfigurationBuilder configurationBuild = new ConfigurationBuilder();

            try
            {
                configurationBuild = configurationBuild.SetBasePath(strDirectorio);
                configurationBuild = configurationBuild.AddJsonFile(strArchivoJson);

                IConfiguration configurationFile = configurationBuild.Build();
                return Encoding.UTF8.GetBytes(Decoder64(configurationFile[strClave].ToString()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Encoder64(string valor)
        {
            try
            {
                byte[] bytes = UnicodeEncoding.UTF8.GetBytes(valor);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Decoder64(string valor)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(valor);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
