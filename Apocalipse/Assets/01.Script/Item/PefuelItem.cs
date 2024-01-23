using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuelItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)//�θ� Ŭ�������� ������ �Լ��� �������ϰ� �ִ�.
    {
        PlayerFuelSystem system = characterManager.Player.GetComponent<PlayerFuelSystem>();//������Ʈ ����
        if (system != null)
        {
            system.Fuel = system.MaxFuel;// ������ Max
            GameInstance.instance.CurrentPlayerFuel = system.Fuel;//
        }
    }
}