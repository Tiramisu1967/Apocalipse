using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProtactSkill : BaseSkill
{
    public float ProjectileMoveSpeed;
    public GameObject Protactile;
    public void ShootProjectile(Vector3 position, Vector3 direction)
    {
        GameObject instance = Instantiate(Protactile, position, Quaternion.identity);
        Protactile protactile = instance.GetComponent<Protactile>();

    }

    public override void Activate()
    {
        ProtactSkill protactSkill = this;
        CharacterManager characterManager = _characterManager;
        Vector3 pos = new (0f, 5f, 0f);
        Vector3 position = characterManager.Player.transform.position + pos;
        protactSkill.ShootProjectile(position, Vector3.up);
    }
    
}
