using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Shared.Dto
{
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string WebUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
