namespace $safeprojectname$.GridView
{
    public class GridViewButton
    {
        public GridViewButton()
        {
            Content = "";
            GridViewButtonTypes = GridViewButtonTypes.ExportGridViewButton;
            Parameter = "";
        }

        public GridViewButton(string content, GridViewButtonTypes type, string parameter = null)
        {
            Content = content;
            GridViewButtonTypes = type;
            Parameter = parameter;
        }

        public string Content { get; set; }
        public GridViewButtonTypes GridViewButtonTypes { get; set; }
        public string Parameter { get; set; }
    }
}
