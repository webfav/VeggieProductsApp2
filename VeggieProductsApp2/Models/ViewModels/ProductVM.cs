using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeggieProductsApp2.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        public IEnumerable<Category> Categories { get; set; }

    }
}
