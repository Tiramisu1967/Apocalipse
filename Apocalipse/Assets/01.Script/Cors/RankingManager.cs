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
        SetRankCanvas.gameObject.SetActive(false);//비가시화
        RankingCanvas.gameObject.SetActive(true);//가시화

        CurrentPlayerInitial = InitialInputFieldText.text;//string 변수 CurrentPlayer Initial에 INitalInputFieldText에 적힌 내용을 저장한다.

        SetCurrentScore();
        SortRanking();
        UpdateRankingUI();
    }
    public void MainMenu()//유니티 에디터 내에서 실행 됨
    {
        GameManager.InitInstance();
        SceneManager.LoadScene("MainMenu");//MainMenu 씬 실행하기
    }

    public void MainMenuRanking()//유니티 에디터 내에서 실행됨
    {
        RankingCanvas.gameObject.SetActive(true);//RankingCanvas 가시화

        for (int i = 0; i < 5; i++)//랭킹 1~5등 까지 순서대로 정렬
        {
            int currentScore = PlayerPrefs.GetInt(i + "BestScore");//currentScore에 현재 PlayerPrefs에 저장되어있는 i + BestScore을 호출한다.// 레지스텅 숫자+BestName 또는 숫자+BestScore라고 저장되어있음//[레지스트리 편집기] -> [HKEY_CURRENT_USER] -> [SOFTWARE] -> [Unity] -> [UnityEditor] -> [DefaultCompany] ->["ProductName"] 

            string currentName = PlayerPrefs.GetString(i + "BestName");//와 같이 호출하지만 int 값이 아닌 string 값을 호출한다.
            //위 두 구문으로 현재 저장되어 있는 PlayerPrefs을 가지고 온다.
            if (currentName == "")//만약 Name이 비어잇으면
                currentName = "None";//None으로 저장하기

            rankingEntries.Add(new RankingEntry(currentScore, currentName));//List에 추가한다. 이름과 스코어를//RankingEntry는 따로 선언된 클래스
        }

        SortRanking();

        for (int i = 0; i < Rankings.Length; i++)//i 값이 Rankings의 길이 보다 커질 때 까지 반복한다.
        {
            if (i < rankingEntries.Count)// 만약 i 가 rankingEntries의 갯수보다 작다면 
            {
                Rankings[i].text = $"{i + 1} {rankingEntries[i].Name} : {rankingEntries[i].Score}";//Rankings의 i번째에 있는 Text에  i+1, rankinEntries의 i번쨰 name, :, rankinEntries의 i번쨰 스토어를 순서대로 출력(대입)한다.
            }
            else //위 조건에 해당하지 않을 경우
            {
                Rankings[i].text = $"{i + 1} -";//i + 1 - 출력(대입)
            }
        }
    }

    void SetCurrentScore()
    {
        rankingEntries.Clear();//리스트의 모든 요소를 제거 ? 물어보기
        // MainMenuRanking와 같음
        for (int i = 0; i < 5; i++)
        {
            int currentScore = PlayerPrefs.GetInt(i + "BestScore");
            string currentName = PlayerPrefs.GetString(i + "BestName");
            if (currentName == "")
                currentName = "None";

            rankingEntries.Add(new RankingEntry(currentScore, currentName));
        }

        // 현재 플레이어의 점수와 이름을 가져와 랭킹에 등록
        int currentPlayerScore = GameInstance.instance.Score;
        string currentPlayerName = CurrentPlayerInitial;

        // 현재 플레이어의 점수가 랭킹에 등록 가능한지 확인
        if (IsScoreEligibleForRanking(currentPlayerScore))
        {
            rankingEntries.Add(new RankingEntry(currentPlayerScore, currentPlayerName));
        }
    }

    bool IsScoreEligibleForRanking(int currentPlayerScore)
    {
        // 랭킹에 등록 가능한지 확인 (예: 상위 5위까지만 등록 가능하도록 설정)
        return rankingEntries.Count < 5 || currentPlayerScore > rankingEntries.Min(entry => entry.Score);//랭킹에 등록된 갯수가 5 이하거나 플레이어의 스코어가 현재 랭킹에 제일 작은 스코어보다 클경우
    }

    void SortRanking()
    {
        // 내림차순으로 정렬
        rankingEntries = rankingEntries.OrderByDescending(entry => entry.Score).ToList();//OrderByDescending().ToList() :내림차순으로 정렬하기//toList == 리스트로 만들기// 즉 OrderByDescending으로 내림차순 정렬된 값들을 ToList로 리스트화 한다.

        // 랭킹이 5개를 초과하면 가장 낮은 점수를 가진 항목을 제거
        if (rankingEntries.Count > 5)
        {
            rankingEntries.RemoveAt(rankingEntries.Count - 1);//RemoveAt 를 하는 이유는 현재 랭킹은 5개까지 출력하고 있으며 상위 5개 아래에 랭킹은 불필요한 값이며, 추후 랭킹을 다시 정렬할 때 불필요한 공간을 차지한다.
        }
    }

    void UpdateRankingUI()// CurrentPlayerScore text에 CurrentPlayerInitial, GameInstance.instance.Score의 값을 순서대로 입력
    {
        CurrentPlayerScore.text = $"{CurrentPlayerInitial} {GameInstance.instance.Score}";
        
        for (int i = 0; i < Rankings.Length; i++)//MainMenuRanking에 SortRanking(); 호출 다음 구문과 같음
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

        // PlayerPrefs 업데이트
        for (int i = 0; i < rankingEntries.Count; i++)
        {
            PlayerPrefs.SetInt(i + "BestScore", rankingEntries[i].Score);
            PlayerPrefs.SetString(i + "BestName", rankingEntries[i].Name);
        }
    }
}

public class RankingEntry//int값과 string 값을 같이 사용하기 위해 선언한 클래스
{
    public int Score { get; set; }
    public string Name { get; set; }

    public RankingEntry(int score, string name)//자신을 재참조, List에 값 넣을 때 사용, 동시에 사용하기 위해
    {
        Score = score;
        Name = name;
    }
}