using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPattern3 : MonoBehaviour

{
    public GameObject Projectile;
    public GameObject vim;
    public float ProjectileMoveSpeed = 5.0f;
    public float FireRate = 2.0f;
   
    public float MoveDistance = 5.0f;
    private float MoveSpeed= 2.0f;
    private int _currentPatternIndex = 0;
    private bool _movingRight = true;
    private bool _bCanMove = false;
    private Vector3 _originPosition;
    private float TemSpeed;
    private void Start()
    {
        TemSpeed = MoveSpeed;
        _originPosition = transform.position; // Boss ���� �� Vector3 ���� _originPosition�� transform.position�� ����
        StartCoroutine(MoveDownAndStartPattern()); //
    }


    //transform.position.y�� _originPosition.y -3f ���� ũ�ٸ� Vector3.down * MoveSpeed * Time.deltaTime ��ŭ �����̰� null ����
    //�ش� �Լ��� ���� �Ǿ��� ��� _bCanMove�� true�� �ٲٰ� InvokeRepeating
    private IEnumerator MoveDownAndStartPattern()
    {
        while (transform.position.y > _originPosition.y - 3f)
        {
            transform.Translate(Vector3.down * MoveSpeed * Time.deltaTime);
            yield return null;
        }

        _bCanMove = true;
        InvokeRepeating("NextPattern", 2.5f, FireRate);
    }

    private void Update()
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

        //_bCanMove�� true��� MoveSideways�� �����Ѵ�.(MoveDownAndStarPattern�� ���� �Ǿ��� ���(���� �Ǿ��� �� ���� ����))
        if (_bCanMove)
            MoveSideways();
    }

    private void NextPattern()
    {
        // ���� �ε����� ������Ű��, ������ ������ ��� �ٽ� ó�� �������� ���ư�
        _currentPatternIndex = (_currentPatternIndex + 1) % 6;

        // ���� ���� ����
        if(_currentPatternIndex == 2)
        {
            StartCoroutine(Pattern3());
        }
    }


    private void MoveSideways()
    {
        if (_movingRight)
        {
            transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
            if (transform.position.x > MoveDistance)
            {
                _movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime);
            if (transform.position.x < -MoveDistance)
            {
                _movingRight = true;
            }
        }
    }

    private void StartMovingSideways()
    {
        StartCoroutine(MovingSidewaysRoutine());
    }

    //�ش� �Լ��� ȣ�� �Ǿ��� �� ������ ����ؼ� �ݺ� ����
    //MoveSideways �Լ� ȣ��
    private IEnumerator MovingSidewaysRoutine()
    {
        while (true)
        {
                MoveSideways();
                yield return null;
        }
    }

    //�ش� �Լ��� ȣ�� �Ǿ��� �� Vector3 ���� position�� direction�� �޾� �����ϰ� position ���� ���� Projectile object�� �����ϰ� Projectile instance�� �����Ͽ� projectile�� !null, ������ �� projectile�� MoveSpeed ������ �ش� Ŭ�������� ����� ProjectilMoveSpeed�� �����Ѵ�.
    //Projectile�� SetDirection �Լ��� direction�� normalized�� ��� ���� ���Կ� ȣ���Ѵ�.
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
    private IEnumerator Pattern3()
    {
        // ���� 3: �� �� �������� �÷��̾�� �ϳ��� �߻�
        int numBullets = 5;
        float interval = 1.0f;

        for (int i = 0; i < numBullets; i++)
        {
            Vector3 playerDirection = (PlayerPosition() - transform.position).normalized;
            ShootProjectile(transform.position, playerDirection);
            yield return new WaitForSeconds(interval);
        }

    }

    private Vector3 PlayerPosition()
    {
        return GameManager.Instance.GetPlayerCharacter().transform.position;
    }

    private void OnDestroy()
    {
        //GameManager.Instance.StageClear();
    }
}