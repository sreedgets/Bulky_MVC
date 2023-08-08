using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        //Adding additional methods in this interface since we
        //know what object type we'll be working with.
        void Update(Category obj);

        void Save();
    }
}
