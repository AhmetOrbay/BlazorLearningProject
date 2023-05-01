using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Shared.CustomException
{
    public class ApiException : Exception
    {
        public ApiException(string Message,Exception InnerException) : base(Message , InnerException)
        {

        }

        public ApiException(string Message)
        {

        }
    }
}
