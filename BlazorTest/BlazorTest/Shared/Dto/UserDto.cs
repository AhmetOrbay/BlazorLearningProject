using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Shared.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [EmailAddress]
        public string EmailAdress { get; set; }
        public bool IsActive { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public string Password { get; set; }
    }
}
