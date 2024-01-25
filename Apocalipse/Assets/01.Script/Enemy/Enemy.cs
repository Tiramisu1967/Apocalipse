using System.Collections;
using UnityEngine;

public class Enemy : BaseCharacter
{
    public float Health = 3f;
    public float AttackDamage = 1f;
    bool bIsDead = false;
    public int isfreeze = 0;
    public bool bMustSpawnItem = false;
    public bool isdesyroy = false;
    private float freezetime = 3;

    public GameObject ExplodeFX;
    public float MoveSpeed = 2.0f;
    float tmpSpeed;
    void Start()
    {
        freezetime = 3;
        tmpSpeed = MoveSpeed;
    }

    void Update()
    {
        if (isfreeze == 1)
        {
            
            Debug.Log("실행 됨 true임 진짜 ");
            if (freezetime >= 0)
            {
                freezetime -= Time.deltaTime;
                Debug.Log(freezetime);
            } else
            {
                freezetime = 3;
                isfreeze = 0;
            }
            
        }
    }


    public void Dead()
    {
        if (!bIsDead)//bIsDead가 true일 때
        {
            GameManager.Instance.EnemyDies();

            if (!bMustSpawnItem)//Item 무조건 출력인지 아닌지 구별
                GameManager.Instance.ItemManager.SpawnRandomItem(0, 3, transform.position);
            else
                GameManager.Instance.ItemManager.SpawnRandomItem(transform.position);

            bIsDead = true;// 다시 true, 아이템이 한번만 나오게?

            Instantiate(ExplodeFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Health -= 1f;
            //GameManager.Instance.SoundManager.PlaySFX("EnemyHit");

            if (Health <= 0f)
            {
                Dead();
            }

            StartCoroutine(HitFlick());
            Destroy(collision.gameObject);
        }
    }
    IEnumerator HitFlick()
    {
        int flickCount = 0; // 깜박인 횟수를 기록하는 변수

        while (flickCount < 1) // 1번 깜박일 때까지 반복
        {
            GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);

            yield return new WaitForSeconds(0.1f); // 0.1초 대기

            GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1);

            yield return new WaitForSeconds(0.1f); // 0.1초 대기

            flickCount++; // 깜박인 횟수 증가
        }
    }
}