
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)
    {
        characterManager.Player.GetComponent<PlayerCharacter>().SetInvincibility(true);//characterManager를 통하여 Player Componet에 접근하여 PlayerCharacter에 SetInvincibility에 true 값을 전하고 있다.
    }
}