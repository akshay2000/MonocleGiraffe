using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.Interfaces
{
    public interface IRoamingDataHelper
    {
        Task<string> GetText(string fileName);
        Task StoreText(string textToWrite, string fileName);
    }
}
