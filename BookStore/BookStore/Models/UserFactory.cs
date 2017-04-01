using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{ 

    static class Factory
    {
        /// <summary>
        /// Decides which class to instantiate.
        /// </summary>
        public static User Get(bool id)
        {
            switch (id)
            {
                case true:
                    return new PremiumUser();
                default:
                    return new BasicUser();
            }
        }
    }
}