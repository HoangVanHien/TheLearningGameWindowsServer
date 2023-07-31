using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEnd
{
    None,
    Win,
    Lose,
    Draw,
}

public class ServerGamePlay : MonoBehaviour
{
    public int matchID;

    [SerializeField] private Character[] team1;
    [SerializeField] private Character[] team2;

    [SerializeField] private int questionCount;
    [SerializeField] private int question;

    public GamePhase gamePhase = GamePhase.Round;
    public int turn = 0;
    private int roundPerTurn = 2;
    public int round = 0;
    private float roundTime = 600;
    private float gapTime = 60;
    public float timeRemain = 0;

    public void SetUp(Character[] team1, Character[] team2, int matchID, int questionCount)
    {
        this.team1 = team1;
        foreach (Character character in team1)
        {
            character.inGame = this;
        }
        this.team2 = team2;
        foreach (Character character in team2)
        {
            character.inGame = this;
        }

        this.matchID = matchID;
        this.questionCount = questionCount;
    }

    public Character[] GetTeam(bool isTeam1)
    {
        return isTeam1 ? team1 : team2;
    }

    // Update is called once per frame
    void Update()
    {
        timeRemain -= Time.deltaTime;
        UpdateMatchStruct();
        switch (gamePhase)
        {
            case GamePhase.Round:
                RoundPhase();
                break;
            case GamePhase.GapBetweenRound:
                GapPhase();
                break;
            default:
                break;
        }
    }

    private void RoundPhase()
    {
        if (timeRemain > 0) return;
        EndRound();
    }

    private void GapPhase()
    {
        CheckWin();
        if (timeRemain > 0) return;
        StartRound();
    }

    private void CheckWin()
    {
        GameEnd team1End = GameEnd.None;
        foreach (Character character in team1)
        {
            if(character.characterData.health > 0)//has 1 member alive
            {
                team1End = GameEnd.Win;
                break;
            }
            else if(character.characterData.health <= 0)//has all member die
            {
                team1End = GameEnd.Draw;
            }
        }
        foreach (Character character in team2)
        {
            if (character.characterData.health > 0)//has 1 member alive
            {
                if (team1End == GameEnd.Win)
                    team1End = GameEnd.None;
                else if (team1End == GameEnd.Draw)
                    team1End = GameEnd.Lose;
                break;
            }
        }
        switch (team1End)
        {
            case GameEnd.Win:
                Debug.Log("Team 1 win");
                break;
            case GameEnd.Lose:
                Debug.Log("Team 2 win");
                break;
            case GameEnd.Draw:
                Debug.Log("Draw game");
                break;
            default:
                break;
        }
    }

    private void StartRound()
    {
        GetNewQuestion();
        if (round == roundPerTurn)//start turn
        {
            round = 1;
            turn++;
            foreach (Character character in team1)
            {
                character.TakePreAction(ActionType.Turn, team2);
            }
            foreach (Character character in team2)
            {
                character.TakePreAction(ActionType.Turn, team1);
            }
        }
        else
        {
            round++;
        }
        foreach (Character character in team1)
        {
            character.TakePreAction(ActionType.Round, team2);
        }
        foreach (Character character in team2)
        {
            character.TakePreAction(ActionType.Round, team1);
        }
        timeRemain = roundTime;
    }

    private void EndRound()
    {

        foreach (Character character in team1)
        {
            character.TakeAfterAction(ActionType.Round, team2);
        }
        foreach (Character character in team2)
        {
            character.TakeAfterAction(ActionType.Round, team1);
        }
        if (round == roundPerTurn)//end turn
        {
            foreach (Character character in team1)
            {
                character.TakeAfterAction(ActionType.Turn, team2);
                character.DrawCard((5 - character.inHandCards.Count) > 0 ? (5 - character.inHandCards.Count) : 0);
            }
            foreach (Character character in team2)
            {
                character.TakeAfterAction(ActionType.Turn, team1);
                character.DrawCard((5 - character.inHandCards.Count) > 0 ? (5 - character.inHandCards.Count) : 0);
            }
        }
        timeRemain = gapTime;
        gamePhase = GamePhase.GapBetweenRound;
    }

    private void GetNewQuestion()
    {
        question = Random.Range(0, questionCount - 1);
    }

    public void PlayCard(GameCardStruct card, string owner, List<string> targets)
    {
        Character ownerChar = null;
        List<Character> targetsChar = new List<Character>();
        foreach (Character character in team1)
        {
            if (character.userID == owner) ownerChar = character;
            if (targets.Contains(character.userID)) targetsChar.Add(character);
        }
        foreach (Character character in team2)
        {
            if (character.userID == owner) ownerChar = character;
            if (targets.Contains(character.userID)) targetsChar.Add(character);
        }
        PlayCard(card, ownerChar, targetsChar.ToArray());
    }

    public void PlayCard(GameCardStruct card, Character owner, Character[] targets)
    {
        GameCard gameCard = new GameCard(card, this);
        gameCard.PlayCard(owner, targets);
    }

    public void UpdateMatchStruct()
    {
        List<MatchPersonalDataStruct> personalDataStructs1 = new List<MatchPersonalDataStruct>();
        foreach (Character character in team1)
        {
            personalDataStructs1.Add(new MatchPersonalDataStruct(character.characterData, character.userID, character.inHandCards.ToArray(), character.ActionsInfo()));
        }
        List<MatchPersonalDataStruct> personalDataStructs2 = new List<MatchPersonalDataStruct>();
        foreach (Character character in team1)
        {
            personalDataStructs2.Add(new MatchPersonalDataStruct(character.characterData, character.userID, character.inHandCards.ToArray(), character.ActionsInfo()));
        }
        //GameManager.instance.firebaseDatabase.SetMatch(matchID, new MatchStruct(gamePhase, turn, round, timeRemain, personalDataStructs1.ToArray(), personalDataStructs2.ToArray(), question));
    }
}
