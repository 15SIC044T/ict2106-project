using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added
using BookStore.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BookStore.DAL
{
    public class DataGateway<T> : IDataGateway<T> where T : class
    {
        internal BookStoreContext db = new BookStoreContext();
        internal DbSet<T> data = null;

        public DataGateway()
        {
            this.data = db.Set<T>();
        }

        public T Delete(int? id)
        {
            T obj = data.Find(id);
            data.Remove(obj);
            db.SaveChanges();
            return obj;
        }

        public void Insert(T obj)
        {
            data.Add(obj);
            db.SaveChanges();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public IEnumerable<T> SelectAll()
        {
            return data.ToList();
        }

        public T SelectById(int? id)
        {
            return data.Find(id);
        }

        public void Update(T obj)
        {
            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IEnumerable<T> Query()
        {
            return data.AsQueryable();
        }
    }
}