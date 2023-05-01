using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealOrdering.Server.Data.Models
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string WebUrl { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
