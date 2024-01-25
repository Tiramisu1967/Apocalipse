using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public Canvas RankingCanvas;
    public Canvas SetRankCanvas;

    private List<RankingEntry> rankingEntries = new List<RankingEntry>();
    public TextMeshProUGUI[] Rankings = new TextMeshProUGUI[5];
    public TextMeshProUGUI CurrentPlayerScore;
    public TextMeshProUGUI InitialInputFieldText;

    private string CurrentPlayerInitial;

    public void SetInitial()
    {
        SetRankCanvas.gameObject.SetActive(false);//�񰡽�ȭ
        RankingCanvas.gameObject.SetActive(true);//����ȭ

        CurrentPlayerInitial = InitialInputFieldText.text;//string ���� CurrentPlayer Initial�� INitalInputFieldText�� ���� ������ �����Ѵ�.

        SetCurrentScore();
        SortRanking();
        UpdateRankingUI();
    }
    public void MainMenu()//����Ƽ ������ ������ ���� ��
    {
        GameManager.InitInstance();
        SceneManager.LoadScene("MainMenu");//MainMenu �� �����ϱ�
    }

    public void MainMenuRanking()//����Ƽ ������ ������ �����
    {
        RankingCanvas.gameObject.SetActive(true);//RankingCanvas ����ȭ

        for (int i = 0; i < 5; i++)//��ŷ 1~5�� ���� ������� ����
        {
            int currentScore = PlayerPrefs.GetInt(i + "BestScore");//currentScore�� ���� PlayerPrefs�� ����Ǿ��ִ� i + BestScore�� ȣ���Ѵ�.// �������� ����+BestName �Ǵ� ����+BestScore��� ����Ǿ�����//[������Ʈ�� ������] -> [HKEY_CURRENT_USER] -> [SOFTWARE] -> [Unity] -> [UnityEditor] -> [DefaultCompany] ->["ProductName"] 

            string currentName = PlayerPrefs.GetString(i + "BestName");//�� ���� ȣ�������� int ���� �ƴ� string ���� ȣ���Ѵ�.
            //�� �� �������� ���� ����Ǿ� �ִ� PlayerPrefs�� ������ �´�.
            if (currentName == "")//���� Name�� ���������
                currentName = "None";//None���� �����ϱ�

            rankingEntries.Add(new RankingEntry(currentScore, currentName));//List�� �߰��Ѵ�. �̸��� ���ھ//RankingEntry�� ���� ����� Ŭ����
        }

        SortRanking();

        for (int i = 0; i < Rankings.Length; i++)//i ���� Rankings�� ���� ���� Ŀ�� �� ���� �ݺ��Ѵ�.
        {
            if (i < rankingEntries.Count)// ���� i �� rankingEntries�� �������� �۴ٸ� 
            {
                Rankings[i].text = $"{i + 1} {rankingEntries[i].Name} : {rankingEntries[i].Score}";//Rankings�� i��°�� �ִ� Text��  i+1, rankinEntries�� i���� name, :, rankinEntries�� i���� ���� ������� ���(����)�Ѵ�.
            }
            else //�� ���ǿ� �ش����� ���� ���
            {
                Rankings[i].text = $"{i + 1} -";//i + 1 - ���(����)
            }
        }
    }

    void SetCurrentScore()
    {
        rankingEntries.Clear();//����Ʈ�� ��� ��Ҹ� ���� ? �����
        // MainMenuRanking�� ����
        for (int i = 0; i < 5; i++)
        {
            int currentScore = PlayerPrefs.GetInt(i + "BestScore");
            string currentName = PlayerPrefs.GetString(i + "BestName");
            if (currentName == "")
                currentName = "None";

            rankingEntries.Add(new RankingEntry(currentScore, currentName));
        }

        // ���� �÷��̾��� ������ �̸��� ������ ��ŷ�� ���
        int currentPlayerScore = GameInstance.instance.Score;
        string currentPlayerName = CurrentPlayerInitial;

        // ���� �÷��̾��� ������ ��ŷ�� ��� �������� Ȯ��
        if (IsScoreEligibleForRanking(currentPlayerScore))
        {
            rankingEntries.Add(new RankingEntry(currentPlayerScore, currentPlayerName));
        }
    }

    bool IsScoreEligibleForRanking(int currentPlayerScore)
    {
        // ��ŷ�� ��� �������� Ȯ�� (��: ���� 5�������� ��� �����ϵ��� ����)
        return rankingEntries.Count < 5 || currentPlayerScore > rankingEntries.Min(entry => entry.Score);//��ŷ�� ��ϵ� ������ 5 ���ϰų� �÷��̾��� ���ھ ���� ��ŷ�� ���� ���� ���ھ�� Ŭ���
    }

    void SortRanking()
    {
        // ������������ ����
        rankingEntries = rankingEntries.OrderByDescending(entry => entry.Score).ToList();//OrderByDescending().ToList() :������������ �����ϱ�//toList == ����Ʈ�� �����// �� OrderByDescending���� �������� ���ĵ� ������ ToList�� ����Ʈȭ �Ѵ�.

        // ��ŷ�� 5���� �ʰ��ϸ� ���� ���� ������ ���� �׸��� ����
        if (rankingEntries.Count > 5)
        {
            rankingEntries.RemoveAt(rankingEntries.Count - 1);//RemoveAt �� �ϴ� ������ ���� ��ŷ�� 5������ ����ϰ� ������ ���� 5�� �Ʒ��� ��ŷ�� ���ʿ��� ���̸�, ���� ��ŷ�� �ٽ� ������ �� ���ʿ��� ������ �����Ѵ�.
        }
    }

    void UpdateRankingUI()// CurrentPlayerScore text�� CurrentPlayerInitial, GameInstance.instance.Score�� ���� ������� �Է�
    {
        CurrentPlayerScore.text = $"{CurrentPlayerInitial} {GameInstance.instance.Score}";
        
        for (int i = 0; i < Rankings.Length; i++)//MainMenuRanking�� SortRanking(); ȣ�� ���� ������ ����
        {
            if (i < rankingEntries.Count)
            {
                Rankings[i].text = $"{i + 1} {rankingEntries[i].Name} : {rankingEntries[i].Score}";
            }
            else
            {
                Rankings[i].text = $"{i + 1} -";
            }
        }

        // PlayerPrefs ������Ʈ
        for (int i = 0; i < rankingEntries.Count; i++)
        {
            PlayerPrefs.SetInt(i + "BestScore", rankingEntries[i].Score);
            PlayerPrefs.SetString(i + "BestName", rankingEntries[i].Name);
        }
    }
}

public class RankingEntry//int���� string ���� ���� ����ϱ� ���� ������ Ŭ����
{
    public int Score { get; set; }
    public string Name { get; set; }

    public RankingEntry(int score, string name)//�ڽ��� ������, List�� �� ���� �� ���, ���ÿ� ����ϱ� ����
    {
        Score = score;
        Name = name;
    }
}