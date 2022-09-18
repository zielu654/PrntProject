using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prntProject;
public class Tables
{
    public char this[int i]
    {
        get
        {
            if (i >= 0 && i <= 9)
                return (char)(i + 48);
            else if (i <= 35)
                return (char)(i - 10 + 97);
            else
                return ' ';
        }
    }
}
