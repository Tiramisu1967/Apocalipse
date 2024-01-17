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
        _originPosition = transform.position; // Boss 생성 시 Vector3 변수 _originPosition에 transform.position를 저장
        StartCoroutine(MoveDownAndStartPattern()); //
    }


    //transform.position.y가 _originPosition.y -3f 보다 크다면 Vector3.down * MoveSpeed * Time.deltaTime 만큼 움직이고 null 리턴
    //해당 함수가 실행 되었을 경우 _bCanMove를 true로 바꾸고 InvokeRepeating
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
        //_bCanMove가 true라면 MoveSideways을 실행한다.(MoveDownAndStarPattern이 실행 되었을 경우(생성 되었을 때 최초 실행))
        if (_bCanMove)
            MoveSideways();
    }

    private void NextPattern()
    {
        // 패턴 인덱스를 증가시키고, 마지막 패턴일 경우 다시 처음 패턴으로 돌아감
        _currentPatternIndex = (_currentPatternIndex + 1) % 6;

        // 현재 패턴 실행
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

    //해당 함수가 호출 되었을 시 다음을 계속해서 반복 실행
    //MoveSideways 함수 호출
    private IEnumerator MovingSidewaysRoutine()
    {
        while (true)
        {
            MoveSideways();
            yield return null;
        }
    }

    //해당 함수가 호출 되었을 때 Vector3 변수 position과 direction를 받아 정의하고 position 백터 값에 Projectile object를 생성하고 Projectile instance에 접근하여 projectile이 !null, 존재할 때 projectile의 MoveSpeed 변수에 해당 클레스에서 선언된 ProjectilMoveSpeed를 저장한다.
    //Projectile에 SetDirection 함수에 direction을 normalized한 결과 값을 대입에 호출한다.
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
        // 패턴 1: 원형으로 총알 발사
        int numBullets1 = 30;
        float angleStep1 = 360.0f / numBullets1;//12

        /*1. i가 0부터 1씩 증가 할때 numBullets1보다 작다면 커질 때까지 다음을 실행한다.
          2. 현재 i의 12를 곱한 수를 angle1을 선언 후 대입하여 정의한다.
          3. 2에서 정의된 angle1과 Mathf.Def2Rad를 곱한 값을 radian1의 대입해 정의한다.
          4. direction1에 x = Mathf.Cos(radian1), y = Mathf.Sin(radian1) z = 0인 Vector 값을 대입한다.
          5. ShootProjectile 함수를 실행
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
        // 패턴 2: 방사형으로 총알 발사
        int numBullets2 = 12;
        float angleStep2 = 360.0f / numBullets2;

        for (int i = 0; i < numBullets2; i++)
        {
            float angle2 = i * angleStep2;
            float radian2 = angle2 * Mathf.Deg2Rad;
            Vector3 direction2 = new Vector3(Mathf.Cos(radian2), Mathf.Sin(radian2), 0);

            ShootProjectile(transform.position, direction2);
        }

        Debug.Log("방사형");
    }

    private IEnumerator Pattern3()
    {
        // 패턴 3: 몇 초 간격으로 플레이어에게 하나씩 발사
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
        // 패턴 4: 나선형으로 총알 발사
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

        Debug.Log("나선형");
    }

    private IEnumerator Pattern5()
    {
        // 패턴 4: 나선형으로 총알 발사
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
        // 패턴 4: 나선형으로 총알 발사
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