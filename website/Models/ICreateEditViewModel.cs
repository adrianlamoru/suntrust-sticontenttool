using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st1001.website.Models
{
    public interface ICreateEditViewModel<T> where T : class
    {
        void CopyToModelForCreate(T model);
        void CopyToModelForEdit(data.st1001Entities db, T model);
    }
}
