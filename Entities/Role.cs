using System;
using System.Collections.Generic;

namespace ShopAPI.Entities
{
    public partial class Role
    {
        public Role()
        {
            Account = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Account> Account { get; set; }
    }
}
