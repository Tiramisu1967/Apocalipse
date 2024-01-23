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

    public void GameStart()
    {
        SceneManager.LoadScene("Stage1");
    }

};