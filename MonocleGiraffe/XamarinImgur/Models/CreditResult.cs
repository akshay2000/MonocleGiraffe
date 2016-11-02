using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinImgur.Models
{
    public class CreditResult
    {
        public int UserLimit { get; set; }
        public int UserRemaining { get; set; }
        public int UserReset { get; set; }
        public int ClientLimit { get; set; }
        public int ClientRemaining { get; set; }
    }
}
