using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image[] HealthImages = new Image[3];//사용할 HP 이미지 배열로 받음
    public Image RepairSkill;//수리 스킬 이미지 받기
    public Image BombSkill;//폭탄 스킬 이미지 받기
    public Image RecoverySkill;//회복 스킬 이미지 받기
    public Image FreezeSkill;//경직 스킬 이미지 받기
    public Slider FuelSlider;//연료 슬라이드 받기

    public TextMeshProUGUI SkillCooldownNoticeText;// CoolTime 표시할 이미지 받기

    private Dictionary<EnumTypes.PlayerSkill, TextMeshProUGUI> _coolDownTexts = new Dictionary<EnumTypes.PlayerSkill, TextMeshProUGUI>();//Dictionary는 다양한 자료형으로 쓰일 수 있는 자료형이다. 이것을 사용하여 열거형 자료형으로 Text를 CoolTime으로 선언하여 열거형에 맞는 텍스트를 출력하기위해서 사용되었다.

    private void Start()
    {
        _coolDownTexts[EnumTypes.PlayerSkill.Repair] = RepairSkill.GetComponentInChildren<TextMeshProUGUI>();//이미지 하위 텍스트를 받아와 그 스킬과 연관된 열거형 변수에 대입(키와 값을 입력)
        _coolDownTexts[EnumTypes.PlayerSkill.Bomb] = BombSkill.GetComponentInChildren<TextMeshProUGUI>();
        _coolDownTexts[EnumTypes.PlayerSkill.Recovery] = RecoverySkill.GetComponentInChildren<TextMeshProUGUI>();
        _coolDownTexts[EnumTypes.PlayerSkill.freeze] = FreezeSkill.GetComponentInChildren<TextMeshProUGUI>();
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

    private void UpdateSkills() // skill 들 호출
    {
        UpdateSkill(EnumTypes.PlayerSkill.Repair);
        UpdateSkill(EnumTypes.PlayerSkill.Bomb);
        UpdateSkill(EnumTypes.PlayerSkill.Recovery);
        UpdateSkill(EnumTypes.PlayerSkill.freeze);
    }

    private void UpdateSkill(EnumTypes.PlayerSkill skill)
    {
        bool isCoolDown = GameManager.Instance.GetPlayerCharacter().Skills[skill].bIsCoolDown;//쿨타임을 확인하는 bool 값에 각 스킬들의 쿨타임 진행여부를 bool 값으로 받는다.
        float currentTime = GameManager.Instance.GetPlayerCharacter().Skills[skill].CurrentTime;//각 스킬들의 쿨타임을 받는다

        _coolDownTexts[skill].gameObject.SetActive(isCoolDown);//?
        _coolDownTexts[skill].text = $"{Mathf.RoundToInt(currentTime)}";//받아온 쿨타임을 이미지 하위 텍스트 내용에 넣는다.
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
        SkillCooldownNoticeText.gameObject.SetActive(true);// 텍스트 활성화(가시화)
        SkillCooldownNoticeText.text = $"{playerSkill.ToString()} Skill is Cooldown";// Skill is Cooldown 출력

        yield return new WaitForSeconds(3);// 3초 기다린 후 진행

        SkillCooldownNoticeText.gameObject.SetActive(false);//비가시화
    }
}