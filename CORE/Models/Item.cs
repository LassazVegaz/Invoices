using System;
using System.Collections.Generic;

#nullable disable

namespace CORE.Models
{
    public partial class Item
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }

    }
}
