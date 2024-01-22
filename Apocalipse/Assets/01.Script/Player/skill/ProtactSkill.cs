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

        if (protactile != null)
        {
            protactile.MoveSpeed = ProjectileMoveSpeed;
            protactile.SetDirection(direction.normalized);
        }
    }

    public override void Activate()
    {
        ProtactSkill protactSkill = this;
        CharacterManager characterManager = _characterManager;
        Vector3 position = characterManager.Player.transform.position;
        protactSkill.ShootProjectile(position, Vector3.up);
    }
    
}
