namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts
{
    public interface IConfigProvider<out TResult> : IReadableDataProvider<TResult>, IWriteableDataProvider
    {

    }
}