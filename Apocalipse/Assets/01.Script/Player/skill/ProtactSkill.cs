using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProtactSkill : BaseSkill
{
    public GameObject Protactile;

    public IEnumerator ShootProjectile(Vector3 position, Vector3 direction)
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject instance = Instantiate(Protactile, position, Quaternion.identity);
            Protactile protactile = instance.GetComponent<Protactile>();
            yield return new WaitForSeconds(1.0f);
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
