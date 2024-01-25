using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    public CharacterManager CharacterManager; //CharacterManager 선언
    public MapManager MapManager;
    public EnemySpawnManager EnemySpawnManager;
    public ItemManager ItemManager;
    public Canvas StageResultCanvas;
    public TMP_Text CurrentScoreText;
    public TMP_Text TimeText;
    [HideInInspector] public bool bStageCleared = false;

    private void Awake()  // 객체 생성시 최초 실행 (그래서 싱글톤을 여기서 생성)
    {
        if (Instance == null)  // 단 하나만 존재하게끔
        {
            Instance = this;  // 객체 생성시 instance에 자기 자신을 넣어줌
        }
        else
            Destroy(this.gameObject);
    }

    public PlayerCharacter GetPlayerCharacter() { return CharacterManager.Player.GetComponent<PlayerCharacter>(); }

    void Start()
    {
        if (CharacterManager == null) { return; }
        CharacterManager.Init(this); //초기화/무엇이 먼저 실행 될지 모르기에/ map,sound, Item, Character은 base를 상속 받을 것. 이유는 GameManager를 상속할 것이고 GameManager에 접근하기 위해. 상호 참조/ 
        EnemySpawnManager.Init(this);
        MapManager.Init(this);
    }

    public void GameStart()//button에서 입력받아 실행
    {
        
        SceneManager.LoadScene("Stage1");//Stage1 불러오기
    }

    public void EnemyDies()//Enemy 에서 참조됨, 에너미가 죽을 시 해당 함수가 호출 되어 스코어에 10을 더함
    {
        AddScore(10);
    }

    public void StageClear()//보스 에너미가 파괴 될 때 또는 f6입력이 들어왔을 때 호출된다.
    {
        AddScore(500);//AddScore에 500의 값을 보내 Score에 500을 더한다.

        float gameStartTime = GameInstance.instance.GameStartTime;//GameInstance에서 계산 중이던 GameStrartTime을 받아와 gameStartTime에 대입한다.
        int score = GameInstance.instance.Score;//위 와 똑같이 gameInstance에 Score을 가지고 와 저장한다.

        // 걸린 시간
        int elapsedTime = Mathf.FloorToInt(Time.time - gameStartTime);//시작된 순간부터 진행된 시간에서 GameStartTime을 빼서 소수점 아래는 내리고 elapsedTime에 저장한다.

        // 스테이지 클리어 결과창 : 점수, 시간
        StageResultCanvas.gameObject.SetActive(true);//가시화
        CurrentScoreText.text = "CurrentScore : " + score;//Text에 Score 값 추가하여 보여주기
        TimeText.text = "ElapsedTime : " + elapsedTime;//Text에 elapsedTime 값 추가하여 보여주기

        bStageCleared = true;//함수 호출을 체크

        // 5초 뒤에 다음 스테이지
        StartCoroutine(LoadNextStageAfterDelay(5f));
    }

    IEnumerator LoadNextStageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);//delay 만큼 쉬기

        switch (GameInstance.instance.CurrentStageLevel)//CurrentstageLevel 가지고 오기
        {
            case 1:
                SceneManager.LoadScene("Stage2");// 씬 로드
                GameInstance.instance.CurrentStageLevel = 2;//값 상슴
                break;

            case 2:
                SceneManager.LoadScene("Result");
                break;
        }
    }

    public void AddScore(int score)
    {
        GameInstance.instance.Score += score;// 들어완 값을 instance score에 저장
    }

    public void Update()//치트
    {
        if (Input.GetKeyUp(KeyCode.F1))//F1이 들어올 경우 
        {
            // 모든 Enemy 찾기
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in enemies)//enemise 배열애 있는 모든 obj를 확인한다.
            {
                Enemy enemy = obj?.GetComponent<Enemy>();
                enemy?.Dead();//Dead 함수를 실행
            }
        }

        // 공격 업그레이드를 4 단계로 상승
        if (Input.GetKeyUp(KeyCode.F2))
        {
            GetPlayerCharacter().CurrentWeaponLevel = 3;//WeaponLeval을 3으로 설정하고
            GameInstance.instance.CurrentPlayerWeaponLevel = GetPlayerCharacter().CurrentWeaponLevel;//GameInstance에도 적용시킨다.
        }

        // 스킬의 쿨타임 및 횟수를 초기화 시킨다
        if (Input.GetKeyUp(KeyCode.F3))
        {
            GetPlayerCharacter().InitSkillCoolDown();//해당 함수를 호출하여 초기화
        }

        // 내구도 초기화
        if (Input.GetKeyUp(KeyCode.F4))
        {
            GetPlayerCharacter().GetComponent<PlayerHPSystem>().InitHealth();//해당 함수를 호출하여 초기화
        }

        // 연료 초기화
        if (Input.GetKeyUp(KeyCode.F5))
        {
            GetPlayerCharacter().GetComponent<PlayerFuelSystem>().InitFuel();//해당 함수를 호출하여 초기화
        }

        // 스테이지 클리어
        if (Input.GetKeyUp(KeyCode.F6))
        {
            StageClear();////해당 함수를 호출하여 초기화
        }
    }

    static public  void InitInstance()
    {
        Debug.Log("!");
        GameInstance.instance.CurrentPlayerFuel = 100;
        GameInstance.instance.CurrentAddOnCount = 0;
        GameInstance.instance.CurrentPlayerHP = 3;
        GameInstance.instance.CurrentPlayerWeaponLevel = 0;
        GameInstance.instance.CurrentStageLevel = 1;
        GameInstance.instance.GameStartTime = 0;
        GameInstance.instance.Score = 0;
    }
}