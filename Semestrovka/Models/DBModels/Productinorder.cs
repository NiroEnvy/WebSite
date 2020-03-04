namespace Semestrovka.Models.DBModels
{
    public partial class Productinorder
    {
        public int Id { get; set; }
        public int? Productid { get; set; }
        public int? Orderid { get; set; }

        public virtual Orders Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
