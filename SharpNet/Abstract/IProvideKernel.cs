using Ninject;

namespace SharpNet.Abstract
{
    public interface IProvideKernel
    {
        IKernel Kernel { get; } 
    }
}