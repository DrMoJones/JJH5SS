using System;
using System.Collections.Generic;

#nullable disable

namespace H5SS.Models
{
    public partial class Wishlist
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Wish { get; set; }
        public string Note { get; set; }
    }
}
