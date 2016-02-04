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

    public static void addRange<T>(this List<T> list , IEnumerable<T> collection, int startIndex , int len)
    {
        int endIndex = startIndex + len -1;
        if(endIndex >= collection.Count<T>() || startIndex < 0)
        {
            UITools.logError("addRange index is outosRange");
            return;
        }
        for (int i = startIndex; i <= endIndex; i++)
        {
           list.Add(collection.ElementAt<T>(i));
        }
    }
}

