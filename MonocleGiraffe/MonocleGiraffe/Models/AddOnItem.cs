using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Store;

namespace MonocleGiraffe.Models
{
    public class AddOnItem
    {
        StoreProduct product;

        public AddOnItem()
        { }

        public AddOnItem(StoreProduct product)
        {
            Title = product.Title;
            Description = product.Description;
            FormattedPrice = product.Price.FormattedPrice;
            this.product = product;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string FormattedPrice { get; set; }
        public bool IsPurchased { get; set; }

        public async void Purchase()
        {
            var result = await product.RequestPurchaseAsync();
        }
    }
}
