using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class freezeskill : BaseSkill
{
   
    public override void Activate()

    {

        base.Activate();

        // 모든 Enemy 찾기
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