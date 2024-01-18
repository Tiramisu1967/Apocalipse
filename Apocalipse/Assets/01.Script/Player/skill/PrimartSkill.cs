using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PrimarySkill : BaseSkill
{
    public float ProjectileMoveSpeed;
    public GameObject Projectile;

    private Weapon[] weapons;

    void Start()
    {
        CooldownTime = 0.2f;

        weapons = new Weapon[12]; // 6개에 함수를 weapons라는 인터페이스로 정의하기 위해서 각 함수만큼 메모리 공간을 분배하기 위해서

        weapons[0] = new Level1Weapon();
        weapons[1] = new Level2Weapon();
        weapons[2] = new Level3Weapon();
        weapons[3] = new Level4Weapon();
        weapons[4] = new Level5Weapon();
        weapons[5] = new Level6Weapon();
        weapons[6] = new Level1Weapon();
        weapons[7] = new Level2Weapon();
        weapons[8] = new Level3Weapon();
        weapons[9] = new Level4Weapon();
        weapons[10] = new Level5Weapon();
        weapons[11] = new Level6Weapon();
    }

    public override void Activate()
    {
        base.Activate();
        Debug.Log("Aㅏ");
        weapons[_characterManager.Player.GetComponent<PlayerCharacter>().CurrentWeaponLevel].Activate(this, _characterManager);
        //무기의 종류를 나타냄, 무기의 종류 클래스가 동적할당되어있는 클래스, 
        //_characterManger에 입력되어 있는 Player에 컴포넌트에 PlayerCharacter에 CurrentWeaponLevel를 가지고 와서 weapons의 종류를(배열값 입력) 정하고 
        //GameManager.Instance.SoundManager.playsfx("primaryskill");
    }

    public void ShootProjectile(Vector3 position, Vector3 direction)
    {
        GameObject instance = Instantiate(Projectile, position, Quaternion.identity);
        Projectile projectile = instance.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.MoveSpeed = ProjectileMoveSpeed;
            projectile.SetDirection(direction.normalized);
        }
    }
}

public interface Weapon
{
    void Activate(PrimarySkill primarySkill, CharacterManager characterManager);
}

public class Level1Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;
        primarySkill.ShootProjectile(position, Vector3.up);
    }
}

public class Level2Weapon : Weapon
{

    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;
        position.x -= 0.1f;

        for (int i = 0; i < 2; i++)
        {
            primarySkill.ShootProjectile(position, Vector3.up);
            position.x += 0.2f;
        }
    }
}

public class Level3Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;

        primarySkill.ShootProjectile(position, Vector3.up);
        primarySkill.ShootProjectile(position, new Vector3(0.3f, 1, 0));
        primarySkill.ShootProjectile(position, new Vector3(-0.3f, 1, 0));
    }
}

public class Level4Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;
        position.x -= 0.1f;

        for (int i = 0; i < 2; i++)
        {
            primarySkill.ShootProjectile(position, Vector3.up);
            position.x += 0.2f;
        }

        Vector3 position2 = characterManager.Player.transform.position;
        primarySkill.ShootProjectile(position2, new Vector3(0.3f, 1, 0));
        primarySkill.ShootProjectile(position2, new Vector3(-0.3f, 1, 0));
    }
}

public class Level5Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;

        for (int i = 0; i < 180; i += 10) // 360도를 10도씩 나눠서 총알 발사
        {
            /*
               180 degree = π radian
               1 degree = π / 180 radian
               x degree = x * π / 180 radian
 
               π radian = 180 degree
               1 radian = 180 / π degree
               x radian = x * 180 / π degree
             */

            // i = degree
            // Deg2Rad = 180 / π degree
            // Mathf 의 cos, sin 은 rad 를 넣어줘야 함.

            float angle = i * Mathf.Deg2Rad;
            Debug.Log(angle);
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

            primarySkill.ShootProjectile(position, direction);
        }
    }


  
}

public class Level6Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;

        for (int i = 0; i <= 180; i ++) // 360도를 10도씩 나눠서 총알 발사
        {
            /*
             pos = 
             */

            // i = degree
            // Deg2Rad = 180 / π degree
            // Mathf 의 cos, sin 은 rad 를 넣어줘야 함.

            float angle = i * Mathf.Deg2Rad;
            Debug.Log(angle);
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

            primarySkill.ShootProjectile(position, direction);
        }
    }
}

