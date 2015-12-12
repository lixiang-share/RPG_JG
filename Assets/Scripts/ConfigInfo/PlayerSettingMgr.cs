using UnityEngine;
using System.Collections;

public class PlayerSettingMgr {

    private float btnVolume = 0.5F;
    public float PanelDuration = 0.3f;


    private static PlayerSettingMgr instance;

    public static PlayerSettingMgr Instance
    {
        get{

            if(instance == null) 
                instance = new PlayerSettingMgr();
            return instance;
        }
    }

    public float BtnVolume
    {
        set
        {
            btnVolume = Mathf.Clamp01(value);
        }
        get
        {
            return btnVolume;
        }
    }
}
