using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Shared.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid CreateUserId { get; set; }
        public Guid SupplierId { get; set; }
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }
        public DateTime ExpireTime { get; set; }
        public string CreateUserFullName { get; set; }
        public string SupplierName { get; set; }

    }
}
