using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealOrdering.Server.Data.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }

        [ForeignKey(nameof(Users))]
        public Guid CreateUserId { get; set; }
        public Users Users { get; set; }
        [ForeignKey(nameof(Supplier))]
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }
        public DateTime ExpireTime { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }

    }
}
