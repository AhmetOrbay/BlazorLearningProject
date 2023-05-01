using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Shared.Extensions
{
    public class PasswordEncrypter
    {
        public static String Encrypt(String Password)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(Password);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
