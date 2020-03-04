using System;
using System.Collections.Generic;

namespace Semestrovka.Models.DBModels
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int AuthorId { get; set; }
        public string ProductAdvantages { get; set; }
        public string ProductDisadvantages { get; set; }
        public string ReviewComment { get; set; }
        public float? ProductRating { get; set; }

        public virtual Product Product { get; set; }
    }
}
