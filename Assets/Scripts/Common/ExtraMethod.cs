using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class ExtraMethod
{
    public static bool isValid(this string str)
    {
        if (str == null || str.Length == 0)
        {
            return false;
        }
        return true ;
    }
}

