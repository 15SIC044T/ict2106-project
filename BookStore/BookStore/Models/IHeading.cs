using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public interface IHeading
    {
        // get a string containing the heading
        string GetString();

        // get the level of the heading
        int GetLevel();
    }
}
