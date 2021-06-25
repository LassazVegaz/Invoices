using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Repositories;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DAL.Repositories
{
    public abstract class Repository<EType,IDType> : IRepository<EType, IDType> where EType : class
    {

        protected readonly InvoicesDBContext _context;

        public Repository(InvoicesDBContext context)
        {
            _context = context;
        }


        public List<EType> GetAll() => _context.Set<EType>().ToList();

        public void Delete(EType item)
        {
            _context.Set<EType>().Remove(item);
            _context.SaveChanges();
        }

        public void Update(EType item)
        {
            _context.Set<EType>().Update(item);
            _context.SaveChanges();
        }

        public EntityEntry<EType> Create(EType item)
        {
            var savedItem = _context.Set<EType>().Add(item);
            _context.SaveChanges();
            return savedItem;
        }

        public EType GetByID(IDType id)
        {
            var res = _context.Set<EType>().Find(id);
            _context.ChangeTracker.Clear();
            return res;
        }
    }
}
