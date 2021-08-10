using System;
using System.Collections.Generic;

#nullable disable

namespace H5SS.Models.EFCore
{
    public partial class Person
    {
        public int Id { get; set; }
        public string Navn { get; set; }
        public string Password { get; set; }
    }
}
