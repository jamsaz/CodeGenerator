namespace $safeprojectname$.GridView
{
    public class GridViewSetting
    {
        public GridViewSetting()
        {
            CanSort = false;
            CanGroup = false;
            CanExport = false;
            CanFilter = false;
            CanPaging = false;
            CanUserInsertRows = false;
            CanUserDeleteRows = false;
        }
        public bool CanSort { get; set; }
        public bool CanGroup { get; set; }
        public bool CanExport { get; set; }
        public bool CanFilter { get; set; }
        public bool CanPaging { get; set; }
        public bool CanUserDeleteRows { get; set; }
        public bool CanUserInsertRows { get; set; }
    }
}
