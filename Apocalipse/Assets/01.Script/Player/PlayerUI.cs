using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image[] HealthImages = new Image[3];//����� HP �̹��� �迭�� ����
    public Image RepairSkill;//���� ��ų �̹��� �ޱ�
    public Image BombSkill;//��ź ��ų �̹��� �ޱ�
    public Image RecoverySkill;//ȸ�� ��ų �̹��� �ޱ�
    public Image FreezeSkill;//���� ��ų �̹��� �ޱ�
    public Slider FuelSlider;//���� �����̵� �ޱ�

    public TextMeshProUGUI SkillCooldownNoticeText;// CoolTime ǥ���� �̹��� �ޱ�

    private Dictionary<EnumTypes.PlayerSkill, TextMeshProUGUI> _coolDownTexts = new Dictionary<EnumTypes.PlayerSkill, TextMeshProUGUI>();//Dictionary�� �پ��� �ڷ������� ���� �� �ִ� �ڷ����̴�. �̰��� ����Ͽ� ������ �ڷ������� Text�� CoolTime���� �����Ͽ� �������� �´� �ؽ�Ʈ�� ����ϱ����ؼ� ���Ǿ���.

    private void Start()
    {
        _coolDownTexts[EnumTypes.PlayerSkill.Repair] = RepairSkill.GetComponentInChildren<TextMeshProUGUI>();//�̹��� ���� �ؽ�Ʈ�� �޾ƿ� �� ��ų�� ������ ������ ������ ����(Ű�� ���� �Է�)
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
        int health = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerHPSystem>().Health;//GameManager�� instance �Ʒ� GetPlayerCharacter�� �Լ�����  Player Hp Systen �Լ��� Health ������ ������ �´�

        for (int i = 0; i < HealthImages.Length; i++) //Health �̹����� ���� ��ŭ �ݺ��ϱ�
        {
            HealthImages[i].gameObject.SetActive(i < health); // Health �̹��� �迭 i��°�� i�� health ���� ���� ��� �����ְ�(true ��ȯ) Ŭ��� false �ؼ� ��ȯ�Ѵ�.
        }
    }

    private void UpdateSkills() // skill �� ȣ��
    {
        UpdateSkill(EnumTypes.PlayerSkill.Repair);
        UpdateSkill(EnumTypes.PlayerSkill.Bomb);
        UpdateSkill(EnumTypes.PlayerSkill.Recovery);
        UpdateSkill(EnumTypes.PlayerSkill.freeze);
    }

    private void UpdateSkill(EnumTypes.PlayerSkill skill)
    {
        bool isCoolDown = GameManager.Instance.GetPlayerCharacter().Skills[skill].bIsCoolDown;//��Ÿ���� Ȯ���ϴ� bool ���� �� ��ų���� ��Ÿ�� ���࿩�θ� bool ������ �޴´�.
        float currentTime = GameManager.Instance.GetPlayerCharacter().Skills[skill].CurrentTime;//�� ��ų���� ��Ÿ���� �޴´�

        _coolDownTexts[skill].gameObject.SetActive(isCoolDown);//?
        _coolDownTexts[skill].text = $"{Mathf.RoundToInt(currentTime)}";//�޾ƿ� ��Ÿ���� �̹��� ���� �ؽ�Ʈ ���뿡 �ִ´�.
    }

    private void UpdateFuel()
    {
        FuelSlider.GetComponent<Slider>().value = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerFuelSystem>().Fuel / 100f;// �����̵忡 ���� ���� Fuel ������ ������ �ٲ۴�(�����Ѵ�.)
    }

    public void NoticeSkillCooldown(EnumTypes.PlayerSkill playerSkill)
    {
        StartCoroutine(NoticeText(playerSkill));//NoticeText(playerSkill) �ڷ�ƾ ����
    }

    IEnumerator NoticeText(EnumTypes.PlayerSkill playerSkill)
    {
        SkillCooldownNoticeText.gameObject.SetActive(true);// �ؽ�Ʈ Ȱ��ȭ(����ȭ)
        SkillCooldownNoticeText.text = $"{playerSkill.ToString()} Skill is Cooldown";// Skill is Cooldown ���

        yield return new WaitForSeconds(3);// 3�� ��ٸ� �� ����

        SkillCooldownNoticeText.gameObject.SetActive(false);//�񰡽�ȭ
    }
}