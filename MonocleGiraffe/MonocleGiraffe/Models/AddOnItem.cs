using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
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

        DateTimeOffset expiryDate = default(DateTimeOffset);
        public DateTimeOffset ExpiryDate
        {
            get { return expiryDate; }
            set
            {
                if (expiryDate != value)
                {
                    expiryDate = value;
                    ExpiresIn = DateTimeToDays(value);
                }
            }
        }

        string expiresIn = default(string);
        public string ExpiresIn { get { return expiresIn; } set { Set(ref expiresIn, value); } }          

        private string DateTimeToDays(DateTimeOffset dateTime)
        {
            var span = dateTime - DateTime.Now;
            switch(span.Days)
            {
                case 0:
                    return "Expires today";
                case 1:
                    return "Expires in one day";
                default:
                    return $"Expires in {span.Days} days";
            }
        }

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
