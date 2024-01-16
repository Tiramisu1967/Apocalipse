using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    protected CharacterManager _characterManager;
    public float CooldownTime;
    public float CurrentTime;
    public bool bIsCoolDown = false;

    /*
     시작할 때 GameManager에서부터 여러 클래스들을 통하여 PlayerCharacter에 Init 함수를 호출하여 InitializeSkills 함수을 실행하고 InitializeSkill함수의 for문을 통하여 반복적으로 호출되는 AddSkill에서 호출 되고 있다. 
    이때 characterManager를 입력받아 _characterManager에 저장하고 있으며 최종적으로 시작시 skill 실행의 클래스들을 불러올 때 알맞는 skill prefeb을 정리하기 위해 실행된다.
     */
    public void Init(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public void Update()
    {
        if (bIsCoolDown)
        {
            CurrentTime -= Time.deltaTime;
            if (CurrentTime <= 0)
            {
                bIsCoolDown = false;
            }
        }
    }

    public bool IsAvailable()
    {
        // 스킬이 쿨다운 중인지 확인
        return !bIsCoolDown;
    }

    public virtual void Activate()
    {
        bIsCoolDown = true;
        CurrentTime = CooldownTime;
    }

    public void InitCoolDown()
    {
        bIsCoolDown = false;
        CurrentTime = 0;
    }

}
