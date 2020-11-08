using System;

namespace Flip2Learn.Shared.Application.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class PurchaseNotFoundException : Exception
    {
        public PurchaseNotFoundException()
        {
        }

        public PurchaseNotFoundException(string message) : base(message)
        {
        }

        public PurchaseNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
