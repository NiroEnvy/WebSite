using System;
using System.Collections.Generic;

namespace Semestrovka.Models.DBModels
{
    public partial class Orders
    {
        public Orders()
        {
            Ordersofusers = new HashSet<Ordersofusers>();
            Productinorder = new HashSet<Productinorder>();
        }

        public int Id { get; set; }
        public int? Owner { get; set; }
        public int? Status { get; set; }
        public DateTime? Datecreated { get; set; }
        public int? Delivery { get; set; }
        public string Address { get; set; }
        public string PayType { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public virtual Deliveries DeliveryNavigation { get; set; }
        public virtual Users OwnerNavigation { get; set; }
        public virtual Statuses StatusNavigation { get; set; }
        public virtual ICollection<Ordersofusers> Ordersofusers { get; set; }
        public virtual ICollection<Productinorder> Productinorder { get; set; }
    }
}
