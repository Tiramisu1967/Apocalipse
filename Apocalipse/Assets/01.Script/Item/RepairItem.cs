using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)// �θ� Ŭ�������� ������ �Լ��� �������ϰ� �ִ�.
    {
        PlayerHPSystem system = characterManager.Player.GetComponent<PlayerHPSystem>();// ����
        if (system != null)
        {
            system.Health += 1;// system�� null�� �ƴ� �� system�� Health ���� +1�Ѵ�
        }
    }
}