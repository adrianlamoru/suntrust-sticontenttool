using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st1001.website.Models
{
    public interface IListItemViewModel<TModel, TViewModel>
    {
        TViewModel CopyFromModel(TModel model);
    }
}
