using System.Collections.Generic;

namespace Semestrovka.Models.DBModels
{
    public partial class Product
    {
        public Product()
        {
            Productimages = new HashSet<Productimages>();
            Productinorder = new HashSet<Productinorder>();
            Review = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public int? Category { get; set; }
        public string Producer { get; set; }
        public int? Mainpictureurl { get; set; }
        public string Characteristics { get; set; }
        public float? ProductRating { get; set; }

        public virtual Categories CategoryNavigation { get; set; }
        public virtual Images MainpictureurlNavigation { get; set; }
        public virtual ICollection<Productimages> Productimages { get; set; }
        public virtual ICollection<Productinorder> Productinorder { get; set; }
        public virtual ICollection<Review> Review { get; set; }
    }
}
