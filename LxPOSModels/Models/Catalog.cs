using System;
using System.Collections.Generic;

namespace LxPOSModels.Models
{
    public partial class Catalog
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Subcat { get; set; }
        public string Value { get; set; }
    }
}
