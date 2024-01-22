using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPattern1 : MonoBehaviour
{
    
    public float Amplitude; // ������ ����(���Ʒ� �̵� �Ÿ�)

    private bool movingUp = true;
    private Vector3 startPosition;
    public float MoveSpeed;
    private float TemSpeed;
    void Start()
    {
        TemSpeed = MoveSpeed;
        startPosition = transform.position;
    }

    void Update()
    {
        Enemy enemy = GetComponent<Enemy>();
        if (enemy.isfreeze == 1)
        {
            MoveSpeed = 0;
        }
        else
        {
            MoveSpeed = TemSpeed;
        }

        float verticalMovement = MoveSpeed * Time.deltaTime;

            // ���� �̵� ���̸鼭 ���� ��ġ�� ���� ��ġ���� �������� ���� ���
            if (movingUp && transform.position.x < startPosition.x + Amplitude)
            {
                transform.position += new Vector3(verticalMovement, 0f, 0f);
            }
            // �Ʒ��� �̵� ���̸鼭 ���� ��ġ�� ���� ��ġ���� �������� ū ���
            else if (!movingUp && transform.position.x > startPosition.x - Amplitude)
            {
                transform.position -= new Vector3(verticalMovement, 0f, 0f);
            }
            // ���� ������ ��� ��� �̵� ������ �ݴ�� ����
            else
            {
                movingUp = !movingUp;
            }

            transform.position -= new Vector3(0f, MoveSpeed * Time.deltaTime, 0f);
        }
       
    
}