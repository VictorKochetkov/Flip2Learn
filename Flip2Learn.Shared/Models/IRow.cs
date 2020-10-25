using System;
namespace Flip2Learn.Shared.Models
{
    public interface IRow : IIdentity
    {
        string Title { get; }
        string Subtitle { get; }
    }
}
