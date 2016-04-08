using UnityEngine;
using System.Collections;

public class FightGOBase : MonoBehaviour {

    private int ID;


    public void WaitForSec(float sec , DefAction action)
    {
        StartCoroutine(_WaitForSec(sec, action));
    }
    private IEnumerator _WaitForSec(float sec , DefAction action)
    {
        yield return new WaitForSeconds(sec);
        if (action != null) action();
    }
}
