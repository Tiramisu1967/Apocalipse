using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : BaseManager
{
    public GameObject[] Enemys;
    public Transform[] EnemySpawnTransform;
    public float CoolDownTime;
    public int MaxSpawnEnemyCount;
    private int Level;
    private int _spawnCount = 0;
    public int BossSpawnCount = 10;

    private bool _bSpawnBoss = false;

    public GameObject[] Boss;
    private GameObject SpawnBoss;
    public GameObject meteor;

    public override void Init(GameManager gameManager)//상호 참조
    {
        base.Init(gameManager);
        StartCoroutine(Meteor());
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (!_bSpawnBoss)//보스가 스폰 되지 않았을 때
        {
            yield return new WaitForSeconds(CoolDownTime);//쿨타임 기다리기

            int spawnCount = Random.Range(1, EnemySpawnTransform.Length -1);//spawnCount int 함수에 랜덤 함수를 사용하여 1~EnemySpawnTransform의 길이 만큼 spawnCount에 저장
            List<int> availablePositions = new List<int>(EnemySpawnTransform.Length);//EnemySpawnTransform.Length를 리스트화

            for (int i = 0; i < EnemySpawnTransform.Length; i++)//i 값이 EnemiySpawnTransform.Length 값보다 작을 때 availablePositions에 Add에 i값을 넣고 호출 후i++/LIST에 정한다 -> 에너미의 숫자
            {
                availablePositions.Add(i); 
            }

            for (int i = 0; i < spawnCount; i++)//i 값이 spawnCount값보다 작을 때 아래의 명령어들을 호출하고 i++
            {
                int randomEnemy = Random.Range(0, Enemys.Length);//0~Enemys의 길이 사이에 랜덤한 값을 randomEnemy에 저장한다.
                int randomPositionIndex = Random.Range(0, availablePositions.Count - 1);//0에서 availablePositions의 Count-1한 숫자 사이에서 랜덤한 값을 randomPositionIndex에 저장한다.
                int randomPosition = availablePositions[randomPositionIndex];//List availablePositions에 randomPositionIndex에 있는 값을 randomPosition에 저장한다.

                availablePositions.RemoveAt(randomPositionIndex);//availablePositions에 RemoveAt에 randomPositionIndex을 넣어 호출

                Instantiate(Enemys[randomEnemy], EnemySpawnTransform[randomPosition].position, Quaternion.identity);// 무작위로 선정된 Pos에 무작위로 선정된 Enemy을 생성한다.
            }

            

            _spawnCount += spawnCount; //카운트 상승
            
            
            if (_spawnCount >= BossSpawnCount)//스폰 카운트가 보스 스폰 카운트보다 크거나 같고 stage가 1이면 보스A 소환
            {
                _bSpawnBoss = true;

                switch (GameInstance.instance.CurrentStageLevel)
                {
                    case (int)EnumTypes.Boss.Stage1 :
                        SpawnBoss = Boss[0];
                        break;

                    case (int)EnumTypes.Boss.Stage2:
                        SpawnBoss = Boss[1];
                        break;
                }
                Instantiate(SpawnBoss, new Vector3(EnemySpawnTransform[1].position.x, EnemySpawnTransform[1].position.y + 1, 0f), Quaternion.identity);
            }
        }

       
    }

    IEnumerator Meteor()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);//쿨타임 기다리기

            int spawnCount = Random.Range(1, EnemySpawnTransform.Length - 1);//spawnCount int 함수에 랜덤 함수를 사용하여 1~EnemySpawnTransform의 길이 만큼 spawnCount에 저장
            List<int> availablePositions = new List<int>(EnemySpawnTransform.Length);//EnemySpawnTransform.Length를 리스트화

            for (int i = 0; i < EnemySpawnTransform.Length; i++)//i 값이 EnemiySpawnTransform.Length 값보다 작을 때 availablePositions에 Add에 i값을 넣고 호출 후i++/LIST에 정한다 -> 에너미의 숫자
            {
                availablePositions.Add(i);
            }

            for (int i = 0; i < spawnCount; i++)//i 값이 spawnCount값보다 작을 때 아래의 명령어들을 호출하고 i++
            {
                int randomPositionIndex = Random.Range(0, availablePositions.Count - 1);//0에서 availablePositions의 Count-1한 숫자 사이에서 랜덤한 값을 randomPositionIndex에 저장한다.
                int randomPosition = availablePositions[randomPositionIndex];//List availablePositions에 randomPositionIndex에 있는 값을 randomPosition에 저장한다.

                availablePositions.RemoveAt(randomPositionIndex);//availablePositions에 RemoveAt에 randomPositionIndex을 넣어 호출

                Instantiate(meteor, EnemySpawnTransform[randomPosition].position, Quaternion.identity);// 무작위로 선정된 Pos에 무작위로 선정된 Enemy을 생성한다.
            }
        }
    }

}