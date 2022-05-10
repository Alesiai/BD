using System;

namespace Poster.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime DateOfBirth { get; set; }
        public string Status { get; set; } = "user";
        public bool IsDeleted { get; set; } = false;
    }
}
