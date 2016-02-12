using UnityEngine;
using System.Collections;

public class ServerItem  {

    private int _id;
    private int _count;
    private string _name;
    private string _ip;

    public string Ip
    {
        get { return _ip; }
        set { _ip = value; }
    }

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
   

    public int Count
    {
        get { return _count; }
        set { _count = value; }
    }
}
