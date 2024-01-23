using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWeaponItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)// 부모 클래스에서 정의한 함수를 다시 재정의 하고 이고 있다.
    {
        if (characterManager != null && characterManager.Player)// 만약 characterManager가 null이 아니고 Player라면 실행
        {
            PlayerCharacter playerCharacter = characterManager.Player.GetComponent<PlayerCharacter>();//PlayerCharacter 참조

            int currentLevel = playerCharacter.CurrentWeaponLevel;//변하는 Level 값을 playerCharacter를 통하여 정의하고 있다.
            int maxLevel = playerCharacter.MaxWeaponLevel;//Max Level

            if (currentLevel >= maxLevel)//MaxLevel 보다 크면
            {
                //GameManager.Instance.AddScore(30);
                return;
            }

            playerCharacter.CurrentWeaponLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);// 공격 레밸 UP
            GameInstance.instance.CurrentPlayerWeaponLevel = playerCharacter.CurrentWeaponLevel;
        }
    }
}