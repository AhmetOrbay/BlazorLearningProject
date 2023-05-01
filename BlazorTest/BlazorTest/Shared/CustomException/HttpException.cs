using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Shared.CustomException
{
    public class HttpException : Exception
    {
        public HttpException(String Message) : base(Message) { }

        public HttpException(String Message, Exception InnerException) : base(Message, InnerException) { }
    }
}
