using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Shared.Dto
{
    public class UserLoginResponseDTO
    {
        public string ApiToken { get; set; }
        public UserDto User { get; set; }
    }
}
