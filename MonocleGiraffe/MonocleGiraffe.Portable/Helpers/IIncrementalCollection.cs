using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.Helpers
{
    public interface IIncrementalCollection
    {
        void LoadMoreAsync(uint count);

        bool HasMore { get; }
    }
}
