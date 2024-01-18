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

    public override void Init(GameManager gameManager)//��ȣ ����
    {
        base.Init(gameManager);
        StartCoroutine(Meteor());
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (!_bSpawnBoss)//������ ���� ���� �ʾ��� ��
        {
            yield return new WaitForSeconds(CoolDownTime);//��Ÿ�� ��ٸ���

            int spawnCount = Random.Range(1, EnemySpawnTransform.Length -1);//spawnCount int �Լ��� ���� �Լ��� ����Ͽ� 1~EnemySpawnTransform�� ���� ��ŭ spawnCount�� ����
            List<int> availablePositions = new List<int>(EnemySpawnTransform.Length);//EnemySpawnTransform.Length�� ����Ʈȭ

            for (int i = 0; i < EnemySpawnTransform.Length; i++)//i ���� EnemiySpawnTransform.Length ������ ���� �� availablePositions�� Add�� i���� �ְ� ȣ�� ��i++/LIST�� ���Ѵ� -> ���ʹ��� ����
            {
                availablePositions.Add(i); 
            }

            for (int i = 0; i < spawnCount; i++)//i ���� spawnCount������ ���� �� �Ʒ��� ��ɾ���� ȣ���ϰ� i++
            {
                int randomEnemy = Random.Range(0, Enemys.Length);//0~Enemys�� ���� ���̿� ������ ���� randomEnemy�� �����Ѵ�.
                int randomPositionIndex = Random.Range(0, availablePositions.Count - 1);//0���� availablePositions�� Count-1�� ���� ���̿��� ������ ���� randomPositionIndex�� �����Ѵ�.
                int randomPosition = availablePositions[randomPositionIndex];//List availablePositions�� randomPositionIndex�� �ִ� ���� randomPosition�� �����Ѵ�.

                availablePositions.RemoveAt(randomPositionIndex);//availablePositions�� RemoveAt�� randomPositionIndex�� �־� ȣ��

                Instantiate(Enemys[randomEnemy], EnemySpawnTransform[randomPosition].position, Quaternion.identity);// �������� ������ Pos�� �������� ������ Enemy�� �����Ѵ�.
            }

            

            _spawnCount += spawnCount; //ī��Ʈ ���
            
            
            if (_spawnCount >= BossSpawnCount)//���� ī��Ʈ�� ���� ���� ī��Ʈ���� ũ�ų� ���� stage�� 1�̸� ����A ��ȯ
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
            yield return new WaitForSeconds(2);//��Ÿ�� ��ٸ���

            int spawnCount = Random.Range(1, EnemySpawnTransform.Length - 1);//spawnCount int �Լ��� ���� �Լ��� ����Ͽ� 1~EnemySpawnTransform�� ���� ��ŭ spawnCount�� ����
            List<int> availablePositions = new List<int>(EnemySpawnTransform.Length);//EnemySpawnTransform.Length�� ����Ʈȭ

            for (int i = 0; i < EnemySpawnTransform.Length; i++)//i ���� EnemiySpawnTransform.Length ������ ���� �� availablePositions�� Add�� i���� �ְ� ȣ�� ��i++/LIST�� ���Ѵ� -> ���ʹ��� ����
            {
                availablePositions.Add(i);
            }

            for (int i = 0; i < spawnCount; i++)//i ���� spawnCount������ ���� �� �Ʒ��� ��ɾ���� ȣ���ϰ� i++
            {
                int randomPositionIndex = Random.Range(0, availablePositions.Count - 1);//0���� availablePositions�� Count-1�� ���� ���̿��� ������ ���� randomPositionIndex�� �����Ѵ�.
                int randomPosition = availablePositions[randomPositionIndex];//List availablePositions�� randomPositionIndex�� �ִ� ���� randomPosition�� �����Ѵ�.

                availablePositions.RemoveAt(randomPositionIndex);//availablePositions�� RemoveAt�� randomPositionIndex�� �־� ȣ��

                Instantiate(meteor, EnemySpawnTransform[randomPosition].position, Quaternion.identity);// �������� ������ Pos�� �������� ������ Enemy�� �����Ѵ�.
            }
        }
    }

}