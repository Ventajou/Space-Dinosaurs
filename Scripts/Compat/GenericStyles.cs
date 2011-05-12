// GenericStyles.cs
//

using System;
using System.Collections;

namespace Vtj.Compat
{
    internal class GenericStyles:IVendorStyles
    {
        #region IVendorStyles Members

        public string Transform
        {
            get { return "transform"; }
        }

        #endregion
    }
}
