using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace st1001.data
{
    public partial class st1001Entities : DbContext
    {

        public st1001Entities(string cnnString)
            : base(cnnString)
        {
        }
    }
}
