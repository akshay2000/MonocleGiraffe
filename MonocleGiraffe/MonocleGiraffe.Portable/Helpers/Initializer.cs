using MonocleGiraffe.Portable.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.Portable.Helpers
{
    public static class Initializer
    {
        public static void Init(IRoamingDataHelper roamingDataHelper)
        {
            RoamingDataHelper = roamingDataHelper;
        }

        public static IRoamingDataHelper RoamingDataHelper { get; private set; }
    }
}
