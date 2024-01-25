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
    public EnumTypes.PlayerSkill Skill;// ������ ����
    public TextMeshProUGUI Text;// Text Ÿ��
}

public class PlayerUI : MonoBehaviour
{
    public Image[] HealthImages = new Image[3];//����� HP �̹��� �迭�� ����
    public Image RepairSkill;//���� ��ų �̹��� �ޱ�
    public Image BombSkill;//��ź ��ų �̹��� �ޱ�
    public Image RecoverySkill;//ȸ�� ��ų �̹��� �ޱ�
    public Image FreezeSkill;//���� ��ų �̹��� �ޱ�
    public Slider FuelSlider;//���� �����̵� �ޱ�

    public TextMeshProUGUI SkillCooldownNoticeText;// CoolTime ǥ���� �̹��� �ޱ�
    public List<CoolDownText> SkillCooldownTexts;// ����Ʈ�� ��Ÿ�� �ؽ�Ʈ ����// ������ ���¸� CoolDownText���� �����ϰ� ����//����Ƽ���� �ڵ����� new�ؼ� add���ָ�, �� ���� ���� ����Ƽ ����Ʈ���� ����ڰ� ���� �����Ѵ�.

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
        int health = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerHPSystem>().Health;//GameManager�� instance �Ʒ� GetPlayerCharacter�� �Լ�����  Player Hp Systen �Լ��� Health ������ ������ �´�

        for (int i = 0; i < HealthImages.Length; i++) //Health �̹����� ���� ��ŭ �ݺ��ϱ�
        {
            HealthImages[i].gameObject.SetActive(i < health); // Health �̹��� �迭 i��°�� i�� health ���� ���� ��� �����ְ�(true ��ȯ) Ŭ��� false �ؼ� ��ȯ�Ѵ�.
        }
    }

    private void UpdateSkills()
    {
        foreach (var item in SkillCooldownTexts)//skillCooldownTexts�� �迭 ũ�� ��ŭ ����. var = �ӽ� ����, in �� ����Ʈ �Ǵ� �迭
        {
            bool isCoolDown = GameManager.Instance.GetPlayerCharacter().Skills[item.Skill].bIsCoolDown;//��Ÿ���� Ȯ���ϴ� bool ���� �� ��ų���� ��Ÿ�� ���࿩�θ� bool ������ �޴´�,CoolDownText���� ������ Ÿ���� �����Ǿ��־� CoolDownText Ŭ������ �����Ͽ� �ȿ� �ִ� ���� skill�� �����Ͽ� ������ �´�
            float currentTime = GameManager.Instance.GetPlayerCharacter().Skills[item.Skill].CurrentTime;//�� ��ų���� ��Ÿ���� �޴´�

            item.Text.gameObject.SetActive(isCoolDown);// ��Ÿ�� ���ο� ���� ����ȭ, �񰡽�ȭ,CoolDownText���� ������ Ÿ���� �����Ǿ��־� CoolDownText Ŭ������ �����Ͽ� �ȿ� �ִ� ���� Text�� �����Ͽ� ������ �´�.
            item.Text.text = $"{Mathf.RoundToInt(currentTime)}";//�޾ƿ� ��Ÿ���� �̹��� ���� �ؽ�Ʈ ���뿡 �ִ´�.//int������ �ø�//�Ҽ��� ���� ���� �ø�
        }
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
        SkillCooldownNoticeText.gameObject.SetActive(true); ;// �ؽ�Ʈ Ȱ��ȭ(����ȭ)//
        SkillCooldownNoticeText.text = $"{playerSkill.ToString()} Skill is Cooldown";// Skill is Cooldown ���//ToString: ��ü�� ������ �ִ� ������ ������ ���ڿ��� ����� �����ϴ� �޼ҵ�

        yield return new WaitForSeconds(3);// 3�� ��ٸ� �� ����

        SkillCooldownNoticeText.gameObject.SetActive(false);//�񰡽�ȭ
    }
}