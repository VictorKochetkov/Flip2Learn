using System;
using Flip2Learn.Shared.Application;

namespace Flip2Learn.Shared.Models
{
    public abstract class BaseDisplay<T>
    {
        protected ICrossApplication app => CrossApplication.instance;
        public readonly T Source;

        public BaseDisplay(T source)
        {
            this.Source = source;
        }
    }
}
