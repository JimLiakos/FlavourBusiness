
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXReportPringTest
{
    /// <MetaDataID>{e72a2600-cde3-4d57-aadf-e930781fcf2e}</MetaDataID>
    public class Invoice
    {
        public Invoice()
        {

            Items = new List<Item> { new Item() { Name = "Nescafe", Quantity = 2, Price = 2.70M }, new Item() { Name = "Pizza sepcial", Quantity = 1, Price = 5.70M } }; 
            
            Name = "Microneme";
        }

        public string Name { get; set; }

        

        public List<Item> Items { get; }

        

    }


    public class Item
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

        public string Name { get; set; }
    }

}
