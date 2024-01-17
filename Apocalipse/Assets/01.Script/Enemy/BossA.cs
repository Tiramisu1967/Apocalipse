using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BossA : MonoBehaviour
{
    public GameObject Projectile;
    public GameObject vim;
    public float ProjectileMoveSpeed = 5.0f;
    public float FireRate = 2.0f;
    public float MoveSpeed = 2.0f;
    public float MoveDistance = 5.0f;

    private int _currentPatternIndex = 0;
    private bool _movingRight = true;
    private bool _bCanMove = false;
    private Vector3 _originPosition;

    private void Start()
    {
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
        _currentPatternIndex = (_currentPatternIndex + 1) % 6;

        // ���� ���� ����
        switch (_currentPatternIndex)
        {
            case 0:
                Pattern1();
                break;
            case 1:
                Pattern2();
                break;
            case 2:
                StartCoroutine(Pattern3());
                break;
            case 3:
                Pattern4();
                break;
            case 4:
                StartCoroutine(Pattern5());
                break;
            case 5:
                StartCoroutine(Pattern6());
                break;
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

    private void Pattern1()
    {
        // ���� 1: �������� �Ѿ� �߻�
        int numBullets1 = 30;
        float angleStep1 = 360.0f / numBullets1;//12

        /*1. i�� 0���� 1�� ���� �Ҷ� numBullets1���� �۴ٸ� Ŀ�� ������ ������ �����Ѵ�.
          2. ���� i�� 12�� ���� ���� angle1�� ���� �� �����Ͽ� �����Ѵ�.
          3. 2���� ���ǵ� angle1�� Mathf.Def2Rad�� ���� ���� radian1�� ������ �����Ѵ�.
          4. direction1�� x = Mathf.Cos(radian1), y = Mathf.Sin(radian1) z = 0�� Vector ���� �����Ѵ�.
          5. ShootProjectile �Լ��� ����
        */
        for (int i = 0; i < numBullets1; i++)
        {
            float angle1 = i * angleStep1;
            float radian1 = angle1 * Mathf.Deg2Rad;
            Vector3 direction1 = new Vector3(Mathf.Cos(radian1), Mathf.Sin(radian1), 0);

            ShootProjectile(transform.position, direction1);
        }
    }

    private void Pattern2()
    {
        // ���� 2: ��������� �Ѿ� �߻�
        int numBullets2 = 12;
        float angleStep2 = 360.0f / numBullets2;

        for (int i = 0; i < numBullets2; i++)
        {
            float angle2 = i * angleStep2;
            float radian2 = angle2 * Mathf.Deg2Rad;
            Vector3 direction2 = new Vector3(Mathf.Cos(radian2), Mathf.Sin(radian2), 0);

            ShootProjectile(transform.position, direction2);
        }

        Debug.Log("�����");
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

    private void Pattern4()
    {
        // ���� 4: ���������� �Ѿ� �߻�
        int numBullets3 = 10;
        float angleStep3 = 360.0f / numBullets3;
        float radius = 2.0f;

        for (int i = 0; i < numBullets3; i++)
        {
            float angle3 = i * angleStep3;
            float radian3 = angle3 * Mathf.Deg2Rad;
            float x = radius * Mathf.Cos(radian3);
            float y = radius * Mathf.Sin(radian3);

            Vector3 direction3 = new Vector3(x, y, 0).normalized;

            ShootProjectile(transform.position, direction3);
        }

        Debug.Log("������");
    }

    private IEnumerator Pattern5()
    {
        // ���� 4: ���������� �Ѿ� �߻�
        int numBullets3 = 10;
        float angleStep3 = 360.0f / numBullets3;

        for (int n = 0; n < 10; n++)
        {
            for (int i = 0; i < numBullets3; i++)
            {
                float angle3 = i * angleStep3;
                float radian3 = angle3 * Mathf.Deg2Rad;
                float x = Mathf.Cos(radian3);
                float y = Mathf.Sin(radian3);

                Vector3 direction3 = new Vector3(x, y, 0).normalized;

                ShootProjectile(transform.position, direction3);
            }
            yield return new WaitForSeconds(0.2f);
            Debug.Log(n);
        }
    }
    private IEnumerator Pattern6()
    {
        // ���� 4: ���������� �Ѿ� �߻�
        Vector3 playerDirection2 = (PlayerPosition() - transform.position).normalized;
        for (int n = 0; n < 19; n++)
        {
           GameObject instance = Instantiate(vim, transform.position, Quaternion.identity);
           Projectile projectile = instance.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.MoveSpeed = ProjectileMoveSpeed*10;
                projectile.SetDirection(playerDirection2.normalized);
            }
            yield return new WaitForSeconds(0.2f);
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