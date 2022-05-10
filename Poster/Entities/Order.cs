using System;

namespace Poster.Entities
{
    public class Order
    {
        public User User { get; set; }
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public double Discount { get; set; }
        public double Cost { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
