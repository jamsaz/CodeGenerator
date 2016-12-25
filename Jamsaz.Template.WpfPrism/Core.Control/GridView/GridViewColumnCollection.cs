using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace $safeprojectname$.GridView
{
    [Serializable]
    public class GridViewColumnCollection : ObservableCollection<GridViewColumn>
    {
        private List<GridViewColumn> _columns = new List<GridViewColumn>();


    }

}
