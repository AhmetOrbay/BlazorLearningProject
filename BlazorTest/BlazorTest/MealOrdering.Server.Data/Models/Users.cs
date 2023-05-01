using System.ComponentModel.DataAnnotations;

namespace MealOrdering.Server.Data.Models
{

    //Create extension if not EXISTS "uuid-ossp" bununla guidi actif ediyoruz postgresqlde
    public class Users
    {
        public Guid Id{ get; set; }
        public DateTime CreateTime { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [EmailAddress]
        public string EmailAdress { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Order> Orders { get; set; }
        public string Password { get; set; }
    }
}