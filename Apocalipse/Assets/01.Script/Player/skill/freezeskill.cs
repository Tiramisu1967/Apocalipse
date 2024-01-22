using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class freezeskill : BaseSkill
{
   
    public override void Activate()

    {

        base.Activate();

        // ��� Enemy ã��
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in enemies)
        {
            if (obj != null)
            {
                if (obj.GetComponent<BossA>())
                    return;
                Debug.Log("!!!");
                Enemy enemy = obj.GetComponent<Enemy>();
                if (enemy != null)
                {

                    Debug.Log("?");
                    enemy.isfreeze = 1;
                }
            }
        }

    }

}