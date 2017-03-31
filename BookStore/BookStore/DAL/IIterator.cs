using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.DAL
{
    interface IIterator<T>
    {
        // get the current object in the iteration
        T current();

        // advance to the next object
        void Next();

        // is the iteration finished?
        bool IsDone();
    }
}