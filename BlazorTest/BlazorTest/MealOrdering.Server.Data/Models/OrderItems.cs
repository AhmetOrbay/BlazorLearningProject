using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealOrdering.Server.Data.Models
{
    public class OrderItems
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }

        [ForeignKey(nameof(Users))]
        public Guid CreateUserId { get; set; }
        public Users Users { get; set; }

        [ForeignKey(nameof(Supplier))]
        public Guid SupplierId { get; set; }
        public Supplier IsActive { get; set; }

        [ForeignKey(nameof(Order))]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
