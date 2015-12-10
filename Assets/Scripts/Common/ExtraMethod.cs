using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class ExtraMethod
{
    public static bool isEmpty(this string str)
    {
        if (str.Trim().Length == 0)
        {
            return false;
        }
        return true ;
    }
}

