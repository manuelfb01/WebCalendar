using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Cryptography;
using System.Text;

namespace WebCalendar.Datos
{
    public class Utilidades
    {
        public static string EncriptarKeyPass(string key) 
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create()) { 
                Encoding enc = Encoding.UTF8;

                byte[] result = hash.ComputeHash(enc.GetBytes(key));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("X2")); //formatea la clave en hexadecimal

                }
            }

            return sb.ToString();
        }
    }
}
