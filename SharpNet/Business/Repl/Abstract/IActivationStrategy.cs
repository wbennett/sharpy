namespace SharpNet.Business.Repl.Abstract
{
    public interface IActivationStrategy<T>
    {
        void Activate(T entity);
    }
}