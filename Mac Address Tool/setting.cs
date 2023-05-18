using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mac_Address_Tool
{
    internal class setting
    {
        public static MacAddressVendorLookup.MacVendorBinaryReader vendorInfoProvider { set; get; }
        public static MacAddressVendorLookup.AddressMatcher addressMatcher { set; get; }
    }
}
