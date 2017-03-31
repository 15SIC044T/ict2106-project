using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.DAL
{
    public class IteratorGeneric<T> : IIterator<T>
    {

        // the collection being iterated through
        List<T> collection;

        // counters
        private int i, j;

        // constructor
        public IteratorGeneric(List<T> collectionIn)
        {
            collection = collectionIn;
            i = 0; 
        }

        // get the current element of the iteration
        public T current()
        {
            return collection[i];
        }

        // move to the next element of the iteration
        public void Next()
        {
            i++;
            if (i >= collection.Count)
            { 
            } 
        }

        // test whether or not the iteration has finished
        public bool IsDone()
        {
            return i >= collection.Count;
        }
    }
}