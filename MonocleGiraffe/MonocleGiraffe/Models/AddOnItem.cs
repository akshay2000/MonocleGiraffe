using MonocleGiraffe.Portable.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Store;

namespace MonocleGiraffe.Models
{
    public class AddOnItem : BindableBase
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

        private bool isActive;
        public bool IsActive { get { return isActive; } set { Set(ref isActive, value); } }
                

        public async void Purchase()
        {
            var result = await product.RequestPurchaseAsync();
            switch (result.Status)
            {
                case StorePurchaseStatus.AlreadyPurchased:
                case StorePurchaseStatus.Succeeded:
                    IsActive = true;
                    break;
                default:
                    IsActive = false;
                    break;
            }
        }
    }
}
