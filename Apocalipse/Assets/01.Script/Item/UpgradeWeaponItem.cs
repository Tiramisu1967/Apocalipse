using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWeaponItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)// �θ� Ŭ�������� ������ �Լ��� �ٽ� ������ �ϰ� �̰� �ִ�.
    {
        if (characterManager != null && characterManager.Player)// ���� characterManager�� null�� �ƴϰ� Player��� ����
        {
            PlayerCharacter playerCharacter = characterManager.Player.GetComponent<PlayerCharacter>();//PlayerCharacter ����

            int currentLevel = playerCharacter.CurrentWeaponLevel;//���ϴ� Level ���� playerCharacter�� ���Ͽ� �����ϰ� �ִ�.
            int maxLevel = playerCharacter.MaxWeaponLevel;//Max Level

            if (currentLevel >= maxLevel)//MaxLevel ���� ũ��
            {
                //GameManager.Instance.AddScore(30);
                return;
            }

            playerCharacter.CurrentWeaponLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);// ���� ���� UP
            GameInstance.instance.CurrentPlayerWeaponLevel = playerCharacter.CurrentWeaponLevel;
        }
    }
}