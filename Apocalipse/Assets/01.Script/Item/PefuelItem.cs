using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuelItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)//부모 클래스에서 정의한 함수를 재정의하고 있다.
    {
        PlayerFuelSystem system = characterManager.Player.GetComponent<PlayerFuelSystem>();//컴포넌트 연결
        if (system != null)
        {
            system.Fuel = system.MaxFuel;// 연료을 Max
            GameInstance.instance.CurrentPlayerFuel = system.Fuel;//
        }
    }
}