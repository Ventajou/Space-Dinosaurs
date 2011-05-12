// FireFoxFourStyles.cs
//

using System;
using System.Collections;

namespace Vtj.Compat
{
    internal class FireFoxFourStyles : IVendorStyles
    {
        #region IVendorStyles Members

        public string Transform
        {
            get { return "-moz-transform"; }
        }

        #endregion
    }
}
