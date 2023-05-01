using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Shared.Dto
{
    public class OrderItemsDto
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid CreateUserId { get; set; }

        public Guid SupplierId { get; set; }

        public Guid OrderId { get; set; }
        public string Description { get; set; }
        public string CreateUserFullName { get; set; }
        public string OrderName { get; set; }
    }
}
