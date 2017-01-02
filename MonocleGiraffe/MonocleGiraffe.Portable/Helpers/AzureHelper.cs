using Microsoft.WindowsAzure.MobileServices;
using MonocleGiraffe.Portable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinImgur.Models;

namespace MonocleGiraffe.Portable.Helpers
{
    public class AzureHelper
    {
        private MobileServiceClient mobileServiceClient;
        public AzureHelper()
        {
            mobileServiceClient = new MobileServiceClient("https://monoclegiraffe.azurewebsites.net");
        }

        private IMobileServiceTable<AzureSubredditItem> table;
        private IMobileServiceTable<AzureSubredditItem> Table
        {
            get
            {
                table = table ?? mobileServiceClient.GetTable<AzureSubredditItem>();
                return table;
            }
        }

        public async Task<Response<List<AzureSubredditItem>>> GetTopN(int n)
        {
            Response<List<AzureSubredditItem>> response = new Response<List<AzureSubredditItem>>();
            try
            {
                var result = await Table
                    .OrderByDescending(e => e.Votes)
                    .ToListAsync();
                response.Content = result;
            }
            catch (Exception e)
            {
                response.IsError = true;
                response.Error = e;
                response.Message = e.Message;
            }
            return response;
        }

        public async Task UpsertItem(SubredditItem item)
        {
            try
            {
                var existingItem = await Table.LookupAsync(item.Url);
                existingItem.Votes++;
                await Table.UpdateAsync(existingItem);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                if (e.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var newItem = new AzureSubredditItem
                    {
                        Id = item.Url,
                        IsMature = item.IsMature,
                        Subscribers = item.Subscribers,
                        Title = item.Title,
                        Votes = 1,
                        Url = item.Url,
                        IsFavorited = item.IsFavorited
                    };
                    await Table.InsertAsync(newItem);
                }
            }
            catch
            {
                //swallow all other exceptions
            }
        }

        private static AzureHelper instance;
        public static AzureHelper Instance
        {
            get
            {
                instance = instance ?? new AzureHelper();
                return instance;
            }
        }
    }
}
