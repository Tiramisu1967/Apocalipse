using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)// 부모 클래스에서 정의한 함수를 재정의하고 있다.
    {
        PlayerHPSystem system = characterManager.Player.GetComponent<PlayerHPSystem>();// 참조
        if (system != null)
        {
            system.Health += 1;// system이 null이 아닐 때 system의 Health 값을 +1한다
        }
    }
}