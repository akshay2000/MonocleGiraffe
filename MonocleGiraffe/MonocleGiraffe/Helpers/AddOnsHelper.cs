using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Foundation.Metadata;
using Windows.Services.Store;
using XamarinImgur.Models;

namespace MonocleGiraffe.Helpers
{
    public class AddOnsHelper
    {
        private bool? isStoreContextTypePresent;
        private bool IsStoreContextTypePresent
        {
            get
            {
                if (isStoreContextTypePresent == null)
                    isStoreContextTypePresent = ApiInformation.IsTypePresent("Windows.Services.Store.StoreContext");
                return (bool)isStoreContextTypePresent;
            }
        }

        public async Task<Response<List<AddOnItem>>> GetAllAddOns()
        {
            if (IsStoreContextTypePresent)
            {
                StoreContext storeContext = StoreContext.GetDefault();
                Response<List<AddOnItem>> response = new Response<List<AddOnItem>>();
                string[] productKinds = { "Durable", "Consumable", "UnmanagedConsumable" };
                StoreProductQueryResult queryResult = await storeContext.GetAssociatedStoreProductsAsync(productKinds);
                if (queryResult.ExtendedError != null)
                {
                    response.IsError = true;
                    response.Message = queryResult.ExtendedError.Message;
                    response.Error = queryResult.ExtendedError;
                }
                else
                {
                    List<AddOnItem> ret = new List<AddOnItem>();
                    IReadOnlyDictionary<string, StoreLicense> licenses = await GetAddOnLicenses();

                    foreach (KeyValuePair<string, StoreProduct> item in queryResult.Products)
                    {
                        AddOnItem addOn = new AddOnItem(item.Value);
                        var matchingPair = licenses.FirstOrDefault(p => p.Key.StartsWith(item.Key));
                        StoreLicense license = matchingPair.Value;
                        addOn.IsActive = license?.IsActive ?? false;
                        addOn.ExpiryDate = license?.ExpirationDate ?? default(DateTimeOffset);
                        ret.Add(addOn);
                    }
                    response.Content = ret;
                }
                return response;
            }
            else
            {
                return new Response<List<AddOnItem>>();
            }
        }

        public async Task<IReadOnlyDictionary<string, StoreLicense>> GetAddOnLicenses()
        {
            StoreContext storeContext = StoreContext.GetDefault();
            StoreAppLicense appLicense = await storeContext.GetAppLicenseAsync();
            if (appLicense == null)
                return null;
            return appLicense.AddOnLicenses;
        }

    }
}
