using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AddOn : MonoBehaviour
{
    private float ShootCycleTime;
    private float MaxShootCycleTime = 0.5f;
    private float Posdistance = float.MaxValue;
    private float TmpPosdistance;
    public GameObject Projectile;
    public Transform PlayerPos;
    public Vector3 TagetPos;
    // Start is called before the first frame update
    void Start()
    {
        ShootCycleTime = MaxShootCycleTime;
    }

    // Update is called once per frame
    void Update()
    {
        Move();


        if (ShootCycleTime < MaxShootCycleTime)
        {
            ShootCycleTime += Time.deltaTime;
        }
        else
        {
            detect();
        }
    }

    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, PlayerPos.position, 0.05f);
    }
    private void detect()
    {
        GameObject[] taget = GameObject.FindGameObjectsWithTag("Enemy");
        if(taget.Length <= 0) 
        {
            TagetPos = new Vector3(0, 100, 0);
        }
        foreach (GameObject ta in taget)
        {
            if (ta.GetComponent<Meteor>())
                continue;
            if (ta != null)
            {
                TmpPosdistance = Vector3.Distance(this.transform.position, ta.transform.position);
                if(Posdistance > TmpPosdistance)
                {
                    Posdistance = TmpPosdistance;
                    TagetPos = ta.transform.position;
                }
            }
        }
        Debug.Log(TagetPos);
        shoot(TagetPos);
        Posdistance = float.MaxValue;
        //총알 만들기
        // 총알 방향 설정 
    }

    private void shoot(Vector3 TagetPos)
    {
        
        GameObject instance = Instantiate(Projectile, transform.position, Quaternion.identity);
        Projectile projectile = instance.GetComponent<Projectile>();
        projectile.MoveSpeed = 10;


        if (Projectile != null || TagetPos != new Vector3(0, 0, 0) )
        {
            projectile.SetDirection((TagetPos - transform.position).normalized);
        } 
        else
        {
            TagetPos = new Vector3(0, 50, 0);
            projectile.SetDirection((TagetPos - transform.position).normalized);
        }
        ShootCycleTime = 0;
    }
}
