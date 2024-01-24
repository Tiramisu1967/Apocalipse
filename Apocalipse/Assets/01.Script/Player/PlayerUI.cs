using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image[] HealthImages = new Image[3];//ü�� �̹��� �ޱ�
    public Image RepairSkill;// ��ų �̹���
    public Image BombSkill;// ��ų �̹���
    public Image RecoverySkill;// ��ų �̹���
    public Image FreezeSkill;// ��ų �̹���
    public Slider FuelSlider;// ���� ������

    public TextMeshProUGUI SkillCooldownNoticeText;

    private Dictionary<EnumTypes.PlayerSkill, TextMeshProUGUI> _coolDownTexts = new Dictionary<EnumTypes.PlayerSkill, TextMeshProUGUI>();//������ ������ Ű�� �ϰ� �ؽ�Ʈ�� ������ �ϴ� ��ųʸ� ����

    private void Start()
    {
        _coolDownTexts[EnumTypes.PlayerSkill.Repair] = RepairSkill.GetComponentInChildren<TextMeshProUGUI>();//Dictionary�� �� ��ų ������ ������ �´� �ؽ�Ʈ 
        _coolDownTexts[EnumTypes.PlayerSkill.Bomb] = BombSkill.GetComponentInChildren<TextMeshProUGUI>();//Dictionary�� �� ��ų ������ ������ �´� �ؽ�Ʈ 
        _coolDownTexts[EnumTypes.PlayerSkill.Recovery] = RecoverySkill.GetComponentInChildren<TextMeshProUGUI>();//Dictionary�� �� ��ų ������ ������ �´� �ؽ�Ʈ 
        _coolDownTexts[EnumTypes.PlayerSkill.freeze] = FreezeSkill.GetComponentInChildren<TextMeshProUGUI>();//Dictionary�� �� ��ų ������ ������ �´� �ؽ�Ʈ 
    }

    private void Update()
    {
        UpdateHealth();
        UpdateSkills();
        UpdateFuel();
    }

    private void UpdateHealth()
    {
        int health = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerHPSystem>().Health;//PlayerHpSystem���� Health ���� �޾ƿ� health�� �ֱ�(����)

        for (int i = 0; i < HealthImages.Length; i++)//health ���� i ���� ���Ͽ� i�� ���� ��� �̹��� �迭���� i��° �̹����� True ����ȭ �Ѵ�.
        {
            HealthImages[i].gameObject.SetActive(i < health);
        }
    }

    private void UpdateSkills()
    {
        UpdateSkill(EnumTypes.PlayerSkill.Repair);//�ش� �Լ��� ������ �Լ� �ֱ� - ��Ÿ�� ����ȭ
        UpdateSkill(EnumTypes.PlayerSkill.Bomb);//�ش� �Լ��� ������ �Լ� �ֱ� -��Ÿ�� ����ȭ
        UpdateSkill(EnumTypes.PlayerSkill.Recovery);//�ش� �Լ��� ������ �Լ� �ֱ� - ��Ÿ�� ����ȭ
        UpdateSkill(EnumTypes.PlayerSkill.freeze);//�ش� �Լ��� ������ �Լ� �ֱ� - ��Ÿ�� ����ȭ
    }

    private void UpdateSkill(EnumTypes.PlayerSkill skill)//���� ������ �Լ��� ����Ͽ� PlayerCharacter�� skills�� ���� �� ��ų�� ��Ÿ�� ���� ���ο� ��Ÿ�� �ð� �� ������ ����, ������ �� ���������� ����Ͽ� ��Ÿ�� ����ȭ, �ؽ�Ʈ ������ �ϰ� �ִ�.
    {
        bool isCoolDown = GameManager.Instance.GetPlayerCharacter().Skills[skill].bIsCoolDown;
        float currentTime = GameManager.Instance.GetPlayerCharacter().Skills[skill].CurrentTime;

        _coolDownTexts[skill].gameObject.SetActive(isCoolDown);
        _coolDownTexts[skill].text = $"{Mathf.RoundToInt(currentTime)}";
    }

    private void UpdateFuel()
    {
        FuelSlider.GetComponent<Slider>().value = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerFuelSystem>().Fuel / 100f;//Fuel ������ ���� �����̴��� value ���� �ִ� ���� �°� ����, value�� �ִ밪�� 1, Fuel�� ���۰�(�ִ밪)�� 100
    }

    public void NoticeSkillCooldown(EnumTypes.PlayerSkill playerSkill)
    {
        StartCoroutine(NoticeText(playerSkill));//NOticeText �ڷ�ƾ ����
    }

    IEnumerator NoticeText(EnumTypes.PlayerSkill playerSkill)
    {
        SkillCooldownNoticeText.gameObject.SetActive(true);//���� ��Ÿ�� ���̶�� Text�� ����ȭ
        SkillCooldownNoticeText.text = $"{playerSkill.ToString()} Skill is Cooldown";// '' ����ȭ ����

        yield return new WaitForSeconds(3);//3�� ��ٸ���

        SkillCooldownNoticeText.gameObject.SetActive(false); // �񰡽�ȭ
    }
}
