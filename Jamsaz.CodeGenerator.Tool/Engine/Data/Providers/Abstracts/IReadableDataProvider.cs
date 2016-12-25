namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts
{
    public interface IReadableDataProvider<out TResult>
    {
        bool HasCash { set; get; }
        TResult GetDatas();
    }
}