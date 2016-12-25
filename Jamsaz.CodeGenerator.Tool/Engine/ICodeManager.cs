namespace Jamsaz.CodeGenerator.Tool.Engine
{
    public interface ICodeManager
    {
        object CodeObjects { get; set; }
        bool InitializeProject();
        bool RealoadProject();
        void SaveProjectSetting(object newObjects);
    }
}