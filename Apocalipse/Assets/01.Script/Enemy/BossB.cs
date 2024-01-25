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
    private IEnumerator Pattern1()
    {
        // 패턴 4: 나선형으로 총알 발사
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
        // 패턴 4: 나선형으로 총알 발사
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