using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.Helpers
{
    public static class StateHelper
    {
        public static Dictionary<string, object> SessionState { get; } = new Dictionary<string, object>();
    }
}
