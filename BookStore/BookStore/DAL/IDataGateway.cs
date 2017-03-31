using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL
{
    interface IDataGateway<T> where T :class
    {
        IEnumerable<T> SelectAll();
        T SelectById(int? id);
        void Insert(T obj);
        void Update(T obj);
        T Delete(int? id);
        void Save();
        IEnumerable<T> Query();
    }
}
