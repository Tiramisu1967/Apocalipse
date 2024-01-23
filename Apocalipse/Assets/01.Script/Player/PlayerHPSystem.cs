using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerHPSystem : MonoBehaviour
{
    public int Health;
    public int MaxHealth;

    void Start()
    {
        Health = GameInstance.instance.CurrentPlayerHP;
    }
    public void InitHealth()
    {
        Health = MaxHealth;
        GameInstance.instance.CurrentPlayerHP = Health;
    }

    IEnumerator HitFlick()
    {
        int flickCount = 0; // ������ Ƚ���� ����ϴ� ����

        while (flickCount < 5) // 5�� ������ ������ �ݺ�
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f); // ��������Ʈ�� ���� 0.5�� ����

            yield return new WaitForSeconds(0.1f); // 0.1�� ���

            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // ��������Ʈ�� ���� ������ ����

            yield return new WaitForSeconds(0.1f); // 0.1�� ���

            flickCount++; // ������ Ƚ�� ����
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")
            && !GameManager.Instance.GetPlayerCharacter().Invincibility
            && !GameManager.Instance.bStageCleared)
        { 
            Health -= 1;
            StartCoroutine(HitFlick());
            if (Health <= 0)
            {
                GameManager.Instance.GetPlayerCharacter().DeadProcess();
            }
            //GameManager.Instance.SoundManager.PlaySFX("Hit");
            Enemy enemy = GetComponent<Enemy>();
            if (enemy.isdesyroy == false)    
                Destroy(collision.gameObject);
            
            
        }

        if (collision.gameObject.CompareTag("Item"))
        {
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }

        }

        GameInstance.instance.CurrentPlayerHP = Health;
    }
}