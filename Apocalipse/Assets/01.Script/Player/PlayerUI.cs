using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CoolDownText
{
    public EnumTypes.PlayerSkill Skill;// 열거형 변수
    public TextMeshProUGUI Text;// Text 타입
}

public class PlayerUI : MonoBehaviour
{
    public Image[] HealthImages = new Image[3];//사용할 HP 이미지 배열로 받음
    public Image RepairSkill;//수리 스킬 이미지 받기
    public Image BombSkill;//폭탄 스킬 이미지 받기
    public Image RecoverySkill;//회복 스킬 이미지 받기
    public Image FreezeSkill;//경직 스킬 이미지 받기
    public Slider FuelSlider;//연료 슬라이드 받기

    public TextMeshProUGUI SkillCooldownNoticeText;// CoolTime 표시할 이미지 받기
    public List<CoolDownText> SkillCooldownTexts;// 리스트로 쿨타임 텍스트 받음// 변수의 형태를 CoolDownText으로 지정하고 있음//유니티에서 자동으로 new해서 add해주며, 그 안의 값은 유니티 에딧트에서 사용자가 직접 지정한다.

    private void Start()
    {
    }

    private void Update()
    {
        UpdateHealth();
        UpdateSkills();
        UpdateFuel();
    }

    private void UpdateHealth()
    {
        int health = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerHPSystem>().Health;//GameManager에 instance 아래 GetPlayerCharacter의 함수에서  Player Hp Systen 함수에 Health 변수를 가지고 온다

        for (int i = 0; i < HealthImages.Length; i++) //Health 이미지에 길이 만큼 반복하기
        {
            HealthImages[i].gameObject.SetActive(i < health); // Health 이미지 배열 i번째가 i가 health 보다 작을 경우 보여주고(true 반환) 클경우 false 해서 반환한다.
        }
    }

    private void UpdateSkills()
    {
        foreach (var item in SkillCooldownTexts)//skillCooldownTexts에 배열 크기 만큼 돈다. var = 임시 변수, in 돌 리스트 또는 배열
        {
            bool isCoolDown = GameManager.Instance.GetPlayerCharacter().Skills[item.Skill].bIsCoolDown;//쿨타임을 확인하는 bool 값에 각 스킬들의 쿨타임 진행여부를 bool 값으로 받는다,CoolDownText으로 변수의 타입이 지정되어있어 CoolDownText 클레스로 접근하여 안에 있는 변수 skill에 접근하여 가지고 온다
            float currentTime = GameManager.Instance.GetPlayerCharacter().Skills[item.Skill].CurrentTime;//각 스킬들의 쿨타임을 받는다

            item.Text.gameObject.SetActive(isCoolDown);// 쿨타임 여부에 따라 가시화, 비가시화,CoolDownText으로 변수의 타입이 지정되어있어 CoolDownText 클레스로 접근하여 안에 있는 변수 Text에 접근하여 가지고 온다.
            item.Text.text = $"{Mathf.RoundToInt(currentTime)}";//받아온 쿨타임을 이미지 하위 텍스트 내용에 넣는다.//int형으로 올림//소수점 이하 전부 올림
        }
    }

    private void UpdateFuel()
    {
        FuelSlider.GetComponent<Slider>().value = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerFuelSystem>().Fuel / 100f;// 슬라이드에 변수 값을 Fuel 변수의 값으로 바꾼다(대입한다.)
    }

    public void NoticeSkillCooldown(EnumTypes.PlayerSkill playerSkill)
    {
        StartCoroutine(NoticeText(playerSkill));//NoticeText(playerSkill) 코루틴 시작
    }

    IEnumerator NoticeText(EnumTypes.PlayerSkill playerSkill)
    {
        SkillCooldownNoticeText.gameObject.SetActive(true); ;// 텍스트 활성화(가시화)//
        SkillCooldownNoticeText.text = $"{playerSkill.ToString()} Skill is Cooldown";// Skill is Cooldown 출력//ToString: 객체가 가지고 있는 정보나 값들을 문자열로 만들어 리턴하는 메소드

        yield return new WaitForSeconds(3);// 3초 기다린 후 진행

        SkillCooldownNoticeText.gameObject.SetActive(false);//비가시화
    }
}