using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    //Here we implement the generic interface we set up. Since
    //we're going to use this interface for Categories we'll pass
    //the Category type into IRepository to be distributed
    //throughout its methods.
    public interface ICategoryRepository : IRepository<Category>
    {
        //Adding additional methods in this interface since we
        //know what object type we'll be working with.
        void Update(Category obj);

        //Moved to IUnitOfWork.cs
        //void Save();
    }
}
