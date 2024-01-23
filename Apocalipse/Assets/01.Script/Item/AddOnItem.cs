using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AddOnItem : BaseItem
{
    public GameObject Add;
    public override void OnGetItem(CharacterManager characterManager)
    {

        if (GameInstance.instance.CurrentAddOnCount <= 2) 
        {
            Vector3 pos = characterManager.Player.GetComponent<PlayerCharacter>().AddOnPos[GameInstance.instance.CurrentAddOnCount - 1].transform.position;
            SpawnAddOn(pos, Add, characterManager); 
        }

    }


    static void SpawnAddOn(Vector3 pos, GameObject Add, CharacterManager characterManager)
    {
        GameInstance.instance.CurrentAddOnCount += 1;
        Debug.Log(GameInstance.instance.CurrentAddOnCount);
        GameObject instance = Instantiate(Add, pos, Quaternion.identity);
        AddOn addOn = instance.AddComponent<AddOn>();
        addOn.count = GameInstance.instance.CurrentAddOnCount - 1;

    }
}
