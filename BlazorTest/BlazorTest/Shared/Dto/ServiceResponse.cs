using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Shared.Dto
{
    public class ServiceResponse<T> : BaseResponse
    {
        public T Data { get; set; }
       
        
    }
     
    public class BaseResponse
    {
        public BaseResponse()
        {
            Success = true;
        }
        public string? Message { get; set; }
        public bool Success { get; set; }

        public void SetException(Exception exception)
        {
            Success = false;
            Message = exception.Message;
        }
    }
}
