
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)
    {
        characterManager.Player.GetComponent<PlayerCharacter>().SetInvincibility(true);//characterManager�� ���Ͽ� Player Componet�� �����Ͽ� PlayerCharacter�� SetInvincibility�� true ���� ���ϰ� �ִ�.
    }
}