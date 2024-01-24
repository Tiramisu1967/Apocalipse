using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AddOnItem : BaseItem
{

    
    public GameObject Prefab;
    public override void OnGetItem(CharacterManager characterManager)
    {
        
         base.OnGetItem(characterManager);
         
         
        if (GameInstance.instance.CurrentAddOnCount < 2)
        {
            SpawnAddOn(characterManager.Player.GetComponent<PlayerCharacter>().transform.position, Prefab, characterManager.Player.GetComponent<PlayerCharacter>().AddOnPos[GameInstance.instance.CurrentAddOnCount].transform);
            GameInstance.instance.CurrentAddOnCount += 1;
        } 
        else
        {
            Destroy(this);
        }

    }


    public static void SpawnAddOn(Vector3 pos, GameObject Add, Transform transform)
    {
        GameObject instance = Instantiate(Add, pos, Quaternion.identity);
        instance.GetComponent<AddOn>().PlayerPos = transform;
        
        
    }
}
