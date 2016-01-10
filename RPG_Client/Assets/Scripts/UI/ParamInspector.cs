using UnityEngine;
using System.Collections;
[System.Serializable]
public class ParamInspector {
    [SerializeField]
    private  string key;
    [SerializeField]
    private string value;

    private const string INT = "Int";
    private const string FLOAT = "Float";
    private const string BOOL = "Bool";


    public System.Object Value
    {
        get{

            if (UITools.isValidString(value))
            {
                if (value.StartsWith(INT))
                {
                    return int.Parse(value.Substring(3).Trim());
                }else if(value.StartsWith(FLOAT)){
                    return float.Parse(value.Substring(5).Trim());
                }
                else if (value.StartsWith(BOOL))
                {
                    return bool.Parse(value.Substring(4).Trim());
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public string Key
    {
        get{
            return key;
        }
    }

}
