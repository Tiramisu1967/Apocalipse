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
     ������ �� GameManager�������� ���� Ŭ�������� ���Ͽ� PlayerCharacter�� Init �Լ��� ȣ���Ͽ� InitializeSkills �Լ��� �����ϰ� InitializeSkill�Լ��� for���� ���Ͽ� �ݺ������� ȣ��Ǵ� AddSkill���� ȣ�� �ǰ� �ִ�. 
    �̶� characterManager�� �Է¹޾� _characterManager�� �����ϰ� ������ ���������� ���۽� skill ������ Ŭ�������� �ҷ��� �� �˸´� skill prefeb�� �����ϱ� ���� ����ȴ�.
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
        // ��ų�� ��ٿ� ������ Ȯ��
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
