namespace PocoDemo.Data
{
    using System;
    using System.Collections.Generic;

    public partial class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int? CategoryId { get; set; }

        public decimal? UnitPrice { get; set; }

        public bool Discontinued { get; set; }

        public byte[] RowVersion { get; set; }

        public Category Category { get; set; }
    }
}
