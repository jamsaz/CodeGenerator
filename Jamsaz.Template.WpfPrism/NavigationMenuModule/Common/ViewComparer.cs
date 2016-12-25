using System.Collections.Generic;
using $saferootprojectname$.Core.Controls;

namespace $safeprojectname$.Common
{
    class ViewComparer : IEqualityComparer<object>
    {
        public bool Equals(object x, object y)
        {
            return ((Form)x).Name == ((Form)y).Name;
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }

}
