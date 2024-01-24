using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AddOnItem : BaseItem
{
    public static GameObject Add;
    public override void OnGetItem(CharacterManager characterManager)
    {
         base.OnGetItem(characterManager);
         SpawnAddOn(characterManager.Player.GetComponent<PlayerCharacter>().AddOnPos[GameInstance.instance.CurrentAddOnCount].transform.position, Add);
        if (GameInstance.instance.CurrentAddOnCount < 2)
        {
            GameInstance.instance.CurrentAddOnCount += 1;
        }

    }


    public static void SpawnAddOn(Vector3 pos, GameObject Add)
    {
       
        GameObject instance = Instantiate(Add, pos, Quaternion.identity);
        AddOn addOn = instance.AddComponent<AddOn>();
        addOn.count = GameInstance.instance.CurrentAddOnCount - 1;

    }
}
