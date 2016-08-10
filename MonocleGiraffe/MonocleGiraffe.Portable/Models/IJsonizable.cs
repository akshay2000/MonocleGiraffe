using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.Models
{
    public interface IJsonizable
    {
        string toJson();        
    }
}
