using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity? GetById(int id);
        int Add(TEntity model);
        int Update(TEntity model);
        int Delete(TEntity model);
    }
}
