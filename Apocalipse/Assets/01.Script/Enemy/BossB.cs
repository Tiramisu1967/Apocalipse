using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BossB : MonoBehaviour
{
    public GameObject Projectile;
    public GameObject vim;
    public float ProjectileMoveSpeed = 5.0f;
    public float FireRate = 2.0f;
    public float MoveSpeed = 2.0f;
    public float MoveDistance = 5.0f;

    private float Hp;
    private int _currentPatternIndex = 0;
    private bool _movingRight = true;
    private bool _bCanMove = false;
    private bool isdown = false;
    private bool iscooldown = false;
    private Vector3 _originPosition;

    private void Start()
    {
        Enemy enemy = GetComponent<Enemy>();
        enemy.bMustSpawnItem = true;
        Hp = enemy.Health;
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
        //_bCanMove�� true��� MoveSideways�� �����Ѵ�.(MoveDownAndStarPattern�� ���� �Ǿ��� ���(���� �Ǿ��� �� ���� ����))
        if (_bCanMove)
            MoveSideways();
    }

    private void NextPattern()
    {
        // ���� �ε����� ������Ű��, ������ ������ ��� �ٽ� ó�� �������� ���ư�
        if(Hp <= 7 && iscooldown == false)
        {

        }
        else 
        {
        
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
    private IEnumerator Pattern1()
    {
        // ���� 4: ���������� �Ѿ� �߻�
        int numBullets3 = 10;
        float angleStep3 = 360.0f / numBullets3;

        for (int n = 0; n < 10; n++)
        {
            for (int i = 1; i <= numBullets3; i++)
            {
                float angle3 = (i-1) * angleStep3;
                float radian3 = angle3 * Mathf.Deg2Rad;
                float x = Mathf.Cos(radian3) * i;
                float y = Mathf.Sin(radian3) * i;

                Vector3 direction3 = new Vector3(x, y, 0).normalized;

                ShootProjectile(transform.position, direction3);
            }
            yield return new WaitForSeconds(0.2f);
            Debug.Log(n);
        }
    }

    private IEnumerator Pattern2()
    {
        // ���� 4: ���������� �Ѿ� �߻�
        int numBullets3 = 10;
        float angleStep3 = 360.0f / numBullets3;

        for (int n = 0; n < 10; n++)
        {
            for (int i = 1; i <= numBullets3; i++)
            {
                float angle3 = (i - 1) * angleStep3;
                float radian3 = angle3 * Mathf.Deg2Rad;
                float x = Mathf.Cos(radian3) * i;
                float y = Mathf.Sin(radian3) * i;

                Vector3 direction3 = new Vector3(x, y, 0).normalized;

                ShootProjectile(transform.position, direction3);
            }
            yield return new WaitForSeconds(0.2f);
            Debug.Log(n);
        }
    }

    private IEnumerator Pattern3()
    {
        Hp += 20;
        isdown = true;
        yield return new WaitForSeconds(0.3f);
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