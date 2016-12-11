using MonocleGiraffe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Store;
using XamarinImgur.Models;

namespace MonocleGiraffe.Helpers
{
    public class AddOnsHelper
    {
        private readonly StoreContext storeContext = StoreContext.GetDefault();

        public async Task<Response<List<AddOnItem>>> GetAllAddOns()
        {
            string[] productKinds = { "Durable", "Consumable", "UnmanagedConsumable" };
            StoreProductQueryResult queryResult = await storeContext.GetAssociatedStoreProductsAsync(productKinds);
            Response<List<AddOnItem>> response = new Response<List<AddOnItem>>();
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
                    ret.Add(addOn);

                    // Use members of the product object to access info for the product...
                }
                response.Content = ret;
            }
            await GetAddOnLicenses();
            return response;
        }

        public async Task<IReadOnlyDictionary<string, StoreLicense>> GetAddOnLicenses()
        {
            StoreAppLicense appLicense = await storeContext.GetAppLicenseAsync();
            if (appLicense == null)
                return null;
            return appLicense.AddOnLicenses;
        }

    }
}
