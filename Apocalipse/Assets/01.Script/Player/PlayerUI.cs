using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image[] HealthImages = new Image[3];//체력 이미지 받기
    public Image RepairSkill;// 스킬 이미지
    public Image BombSkill;// 스킬 이미지
    public Image RecoverySkill;// 스킬 이미지
    public Image FreezeSkill;// 스킬 이미지
    public Slider FuelSlider;// 연료 게이지

    public TextMeshProUGUI SkillCooldownNoticeText;

    private Dictionary<EnumTypes.PlayerSkill, TextMeshProUGUI> _coolDownTexts = new Dictionary<EnumTypes.PlayerSkill, TextMeshProUGUI>();//열거형 변수를 키로 하고 텍스트를 값으로 하는 딕셔너리 선언

    private void Start()
    {
        _coolDownTexts[EnumTypes.PlayerSkill.Repair] = RepairSkill.GetComponentInChildren<TextMeshProUGUI>();//Dictionary로 각 스킬 열거형 변수에 맞는 텍스트 
        _coolDownTexts[EnumTypes.PlayerSkill.Bomb] = BombSkill.GetComponentInChildren<TextMeshProUGUI>();//Dictionary로 각 스킬 열거형 변수에 맞는 텍스트 
        _coolDownTexts[EnumTypes.PlayerSkill.Recovery] = RecoverySkill.GetComponentInChildren<TextMeshProUGUI>();//Dictionary로 각 스킬 열거형 변수에 맞는 텍스트 
        _coolDownTexts[EnumTypes.PlayerSkill.freeze] = FreezeSkill.GetComponentInChildren<TextMeshProUGUI>();//Dictionary로 각 스킬 열거형 변수에 맞는 텍스트 
    }

    private void Update()
    {
        UpdateHealth();
        UpdateSkills();
        UpdateFuel();
    }

    private void UpdateHealth()
    {
        int health = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerHPSystem>().Health;//PlayerHpSystem에서 Health 값을 받아와 health에 넣기(대입)

        for (int i = 0; i < HealthImages.Length; i++)//health 값과 i 값을 비교하여 i가 작을 경우 이미지 배열에서 i번째 이미지를 True 가시화 한다.
        {
            HealthImages[i].gameObject.SetActive(i < health);
        }
    }

    private void UpdateSkills()
    {
        UpdateSkill(EnumTypes.PlayerSkill.Repair);//해당 함수에 열거형 함수 넣기 - 쿨타임 가시화
        UpdateSkill(EnumTypes.PlayerSkill.Bomb);//해당 함수에 열거형 함수 넣기 -쿨타임 가시화
        UpdateSkill(EnumTypes.PlayerSkill.Recovery);//해당 함수에 열거형 함수 넣기 - 쿨타임 가시화
        UpdateSkill(EnumTypes.PlayerSkill.freeze);//해당 함수에 열거형 함수 넣기 - 쿨타임 가시화
    }

    private void UpdateSkill(EnumTypes.PlayerSkill skill)//받은 열거형 함수를 사용하여 PlayerCharacter에 skills에 대입 각 스킬에 쿨타임 실행 여부와 쿨타임 시간 값 가지고 오기, 가지고 온 변수값들을 사용하여 쿨타임 가시화, 텍스트 변경을 하고 있다.
    {
        bool isCoolDown = GameManager.Instance.GetPlayerCharacter().Skills[skill].bIsCoolDown;
        float currentTime = GameManager.Instance.GetPlayerCharacter().Skills[skill].CurrentTime;

        _coolDownTexts[skill].gameObject.SetActive(isCoolDown);
        _coolDownTexts[skill].text = $"{Mathf.RoundToInt(currentTime)}";
    }

    private void UpdateFuel()
    {
        FuelSlider.GetComponent<Slider>().value = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerFuelSystem>().Fuel / 100f;//Fuel 변수의 값을 슬라이더의 value 값에 최대 값에 맞게 조정, value의 최대값은 1, Fuel의 시작값(최대값)은 100
    }

    public void NoticeSkillCooldown(EnumTypes.PlayerSkill playerSkill)
    {
        StartCoroutine(NoticeText(playerSkill));//NOticeText 코루틴 시작
    }

    IEnumerator NoticeText(EnumTypes.PlayerSkill playerSkill)
    {
        SkillCooldownNoticeText.gameObject.SetActive(true);//아직 쿨타임 중이라는 Text를 가시화
        SkillCooldownNoticeText.text = $"{playerSkill.ToString()} Skill is Cooldown";// '' 가시화 내용

        yield return new WaitForSeconds(3);//3초 기다리기

        SkillCooldownNoticeText.gameObject.SetActive(false); // 비가시화
    }
}
