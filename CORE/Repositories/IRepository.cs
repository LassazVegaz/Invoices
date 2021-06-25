using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CORE.Repositories
{
    public interface IRepository<EType, IDType> where EType : class
    {
        EntityEntry<EType> Create(EType item);
        void Delete(EType item);
        List<EType> GetAll();
        void Update(EType item);
        EType GetByID(IDType id);
    }
}