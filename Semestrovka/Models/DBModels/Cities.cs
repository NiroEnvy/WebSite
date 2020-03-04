using System;
using System.Collections.Generic;

namespace Semestrovka.Models.DBModels
{
    public partial class Cities
    {
        public Cities()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
