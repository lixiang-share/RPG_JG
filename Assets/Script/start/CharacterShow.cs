using UnityEngine;
using System.Collections;

public class CharacterShow : MonoBehaviour {


    public void OnPress(bool isPress ) {
        if(isPress==false)
        StartmenuController._instance.OnCharacterClick(transform.parent.gameObject  );
    }

}
