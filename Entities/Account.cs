using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ShopAPI.Entities
{
    public partial class Account
    {
        public Account()
        {
            Order = new HashSet<Order>();
        }

        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public string Name { get; set; }
        public bool? Gender { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreateAt { get; set; }
        public string AvatarUrl { get; set; }
        public string Phone { get; set; }


        public Role Role { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
