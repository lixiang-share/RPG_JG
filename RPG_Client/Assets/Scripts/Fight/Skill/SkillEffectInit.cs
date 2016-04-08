using UnityEngine;
using System.Collections;

public class SkillEffectInit : MonoBehaviour
{
    public int EffectType = 1;
    public void Start()
    {
        if (EffectType == 1)
        {
            InitEffect_1();
        }
    }
    public void InitEffect_1()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
         //   if (!go.activeSelf) go.SetActive(true);
            NcCurveAnimation[] ncs = go.GetComponentsInChildren<NcCurveAnimation>();
            foreach (NcCurveAnimation nc in ncs)
            {
                if (nc.enabled) nc.enabled = false;
            }
        }

    }
}
	
