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

    public void GameStart()
    {
        SceneManager.LoadScene("Stage1");
    }

};