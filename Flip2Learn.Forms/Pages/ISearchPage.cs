using System;
using System.Collections.Generic;
using System.Text;

namespace Flip2Learn.Forms.Pages
{
    public interface ISearchPage
    {
        void OnSearchBarTextChanged(string text);
        event EventHandler<string> SearchBarTextChanged;

        string SearchPlaceholder { get; }
    }
}
