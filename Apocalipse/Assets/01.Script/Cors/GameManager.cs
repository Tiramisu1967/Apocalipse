using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    public CharacterManager CharacterManager; //CharacterManager ����
    public MapManager MapManager;
    public EnemySpawnManager EnemySpawnManager;
    public ItemManager ItemManager;
    public Canvas StageResultCanvas;
    public TMP_Text CurrentScoreText;
    public TMP_Text TimeText;
    [HideInInspector] public bool bStageCleared = false;

    private void Awake()  // ��ü ������ ���� ���� (�׷��� �̱����� ���⼭ ����)
    {
        if (Instance == null)  // �� �ϳ��� �����ϰԲ�
        {
            Instance = this;  // ��ü ������ instance�� �ڱ� �ڽ��� �־���
        }
        else
            Destroy(this.gameObject);
    }

    public PlayerCharacter GetPlayerCharacter() { return CharacterManager.Player.GetComponent<PlayerCharacter>(); }

    void Start()
    {
        if (CharacterManager == null) { return; }
        CharacterManager.Init(this); //�ʱ�ȭ/������ ���� ���� ���� �𸣱⿡/ map,sound, Item, Character�� base�� ��� ���� ��. ������ GameManager�� ����� ���̰� GameManager�� �����ϱ� ����. ��ȣ ����/ 
        EnemySpawnManager.Init(this);
        MapManager.Init(this);
    }

    public void GameStart()//button���� �Է¹޾� ����
    {
        
        SceneManager.LoadScene("Stage1");//Stage1 �ҷ�����
    }

    public void EnemyDies()//Enemy ���� ������, ���ʹ̰� ���� �� �ش� �Լ��� ȣ�� �Ǿ� ���ھ 10�� ����
    {
        AddScore(10);
    }

    public void StageClear()//���� ���ʹ̰� �ı� �� �� �Ǵ� f6�Է��� ������ �� ȣ��ȴ�.
    {
        AddScore(500);//AddScore�� 500�� ���� ���� Score�� 500�� ���Ѵ�.

        float gameStartTime = GameInstance.instance.GameStartTime;//GameInstance���� ��� ���̴� GameStrartTime�� �޾ƿ� gameStartTime�� �����Ѵ�.
        int score = GameInstance.instance.Score;//�� �� �Ȱ��� gameInstance�� Score�� ������ �� �����Ѵ�.

        // �ɸ� �ð�
        int elapsedTime = Mathf.FloorToInt(Time.time - gameStartTime);//���۵� �������� ����� �ð����� GameStartTime�� ���� �Ҽ��� �Ʒ��� ������ elapsedTime�� �����Ѵ�.

        // �������� Ŭ���� ���â : ����, �ð�
        StageResultCanvas.gameObject.SetActive(true);//����ȭ
        CurrentScoreText.text = "CurrentScore : " + score;//Text�� Score �� �߰��Ͽ� �����ֱ�
        TimeText.text = "ElapsedTime : " + elapsedTime;//Text�� elapsedTime �� �߰��Ͽ� �����ֱ�

        bStageCleared = true;//�Լ� ȣ���� üũ

        // 5�� �ڿ� ���� ��������
        StartCoroutine(LoadNextStageAfterDelay(5f));
    }

    IEnumerator LoadNextStageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);//delay ��ŭ ����

        switch (GameInstance.instance.CurrentStageLevel)//CurrentstageLevel ������ ����
        {
            case 1:
                SceneManager.LoadScene("Stage2");// �� �ε�
                GameInstance.instance.CurrentStageLevel = 2;//�� ��
                break;

            case 2:
                SceneManager.LoadScene("Result");
                break;
        }
    }

    public void AddScore(int score)
    {
        GameInstance.instance.Score += score;// ���� ���� instance score�� ����
    }

    public void Update()//ġƮ
    {
        if (Input.GetKeyUp(KeyCode.F1))//F1�� ���� ��� 
        {
            // ��� Enemy ã��
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in enemies)//enemise �迭�� �ִ� ��� obj�� Ȯ���Ѵ�.
            {
                Enemy enemy = obj?.GetComponent<Enemy>();
                enemy?.Dead();//Dead �Լ��� ����
            }
        }

        // ���� ���׷��̵带 4 �ܰ�� ���
        if (Input.GetKeyUp(KeyCode.F2))
        {
            GetPlayerCharacter().CurrentWeaponLevel = 3;//WeaponLeval�� 3���� �����ϰ�
            GameInstance.instance.CurrentPlayerWeaponLevel = GetPlayerCharacter().CurrentWeaponLevel;//GameInstance���� �����Ų��.
        }

        // ��ų�� ��Ÿ�� �� Ƚ���� �ʱ�ȭ ��Ų��
        if (Input.GetKeyUp(KeyCode.F3))
        {
            GetPlayerCharacter().InitSkillCoolDown();//�ش� �Լ��� ȣ���Ͽ� �ʱ�ȭ
        }

        // ������ �ʱ�ȭ
        if (Input.GetKeyUp(KeyCode.F4))
        {
            GetPlayerCharacter().GetComponent<PlayerHPSystem>().InitHealth();//�ش� �Լ��� ȣ���Ͽ� �ʱ�ȭ
        }

        // ���� �ʱ�ȭ
        if (Input.GetKeyUp(KeyCode.F5))
        {
            GetPlayerCharacter().GetComponent<PlayerFuelSystem>().InitFuel();//�ش� �Լ��� ȣ���Ͽ� �ʱ�ȭ
        }

        // �������� Ŭ����
        if (Input.GetKeyUp(KeyCode.F6))
        {
            StageClear();////�ش� �Լ��� ȣ���Ͽ� �ʱ�ȭ
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