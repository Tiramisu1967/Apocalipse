using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AddOn : MonoBehaviour
{
    private float ShootCycleTime;
    private float MaxShootCycleTime = 3.0f;
    public GameObject Projectile;
    public int count;
    private Vector3 Pos;
    // Start is called before the first frame update
    void Start()
    {
        ShootCycleTime = MaxShootCycleTime;
        PlayerCharacter character = GetComponent<PlayerCharacter>();
        Pos = character.gameObject.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerCharacter character = GetComponent<PlayerCharacter>();
        transform.position = Vector3.Lerp(gameObject.transform.position, character.gameObject.transform.position - Pos, 0.05f);


        if (ShootCycleTime < MaxShootCycleTime)
        {
            ShootCycleTime += Time.deltaTime;
        }else
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject taget = GameObject.FindWithTag("Enemy");
        if (taget == null)
        {

            GameObject instance = Instantiate(Projectile, transform.position, Quaternion.identity);
            Projectile projectile = instance.GetComponent<Projectile>();
            projectile.MoveSpeed = 10;
            ShootCycleTime = 0;
        } else
        {
            GameObject instance = Instantiate(Projectile, transform.position, Quaternion.identity);
            Projectile projectile = instance.GetComponent<Projectile>();
            projectile.MoveSpeed = 10;
            if (Projectile != null)
            {
               
                projectile.SetDirection((taget.transform.position - transform.position).normalized.normalized);
                ShootCycleTime = 0;
            }
           
            
        }
            
    } 
}
