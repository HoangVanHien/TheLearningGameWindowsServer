using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMakingManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private List<string> oldFindingMatchPlayerList = new List<string>();
    [SerializeField] private List<CharacterDataStruct> oldCharacterDatas = new List<CharacterDataStruct>();
    [SerializeField] private List<string> makingMatchPlayerList = new List<string>();

    List<string> newFindingMatchPlayerList = new List<string>();

    private void FixedUpdate()
    {
        if (newFindingMatchPlayerList == null || newFindingMatchPlayerList.Count <= 0)
        {
            MatchMaking();
        }
    }

    private void MatchMaking()
    {
        for (int i = 0; i < oldCharacterDatas.Count - 1; i++)
        {
            Debug.Log(oldCharacterDatas[i].playerName);
            for (int j = i + 1; j < oldCharacterDatas.Count; j++)
            {
                if (oldFindingMatchPlayerList[i] == oldFindingMatchPlayerList[j])
                {
                    oldCharacterDatas.RemoveAt(j);
                    oldFindingMatchPlayerList.RemoveAt(j);
                }
                if (oldCharacterDatas[i].health == oldCharacterDatas[j].health)//match condition
                {
                    MatchBuild(new string[] { oldFindingMatchPlayerList[i] }, new CharacterDataStruct[] { oldCharacterDatas[i] },
                                new string[] { oldFindingMatchPlayerList[j] }, new CharacterDataStruct[] { oldCharacterDatas[j] });
                    makingMatchPlayerList.Add(oldFindingMatchPlayerList[j]);
                    makingMatchPlayerList.Add(oldFindingMatchPlayerList[i]);
                    oldCharacterDatas.RemoveAt(j);
                    oldCharacterDatas.RemoveAt(i);
                    oldFindingMatchPlayerList.RemoveAt(j);
                    oldFindingMatchPlayerList.RemoveAt(i);
                    return;
                }
            }
        }
    }

    public void MatchBuild(string[] team1, CharacterDataStruct[] teamData1, string[] team2, CharacterDataStruct[] teamData2)
    {
        List<MatchPersonalDataStruct> matchTeam1 = new List<MatchPersonalDataStruct>();
        List<MatchPersonalDataStruct> matchTeam2 = new List<MatchPersonalDataStruct>();
        int matchID = Random.Range(0, 100000);
        PlayerMatchStruct playerMatchStruct = new PlayerMatchStruct(matchID, true);
        for (int i = 0; i < team1.Length; i++)
        {
            gameManager.firebaseDatabase.SetPlayerMatchID(team1[i], playerMatchStruct);
            gameManager.firebaseDatabase.CancelFindingMatch(team1[i]);
            gameManager.firebaseDatabase.SetPlayerStatus(team1[i], PlayerStatus.inMatch);
            List<int> onHandCards = new List<int>();

            for (int j = 0; j < 5; j++)
            {
                int card;
                do card = Random.Range(0, 199);
                while (onHandCards.Contains(card));
                onHandCards.Add(card);
            }
            matchTeam1.Add(new MatchPersonalDataStruct(teamData1[i], team1[i], onHandCards.ToArray(), new string[] { }));

        }

        playerMatchStruct.isTeam1 = false;
        for (int i = 0; i < team2.Length; i++)
        {
            gameManager.firebaseDatabase.SetPlayerMatchID(team2[i], playerMatchStruct);
            gameManager.firebaseDatabase.CancelFindingMatch(team2[i]);
            gameManager.firebaseDatabase.SetPlayerStatus(team2[i], PlayerStatus.inMatch);
            List<int> onHandCards = new List<int>();

            for (int j = 0; j < 5; j++)
            {
                int card;
                do card = Random.Range(0, 199);
                while (onHandCards.Contains(card));
                onHandCards.Add(card);
            }
            matchTeam2.Add(new MatchPersonalDataStruct(teamData2[i], team2[i], onHandCards.ToArray(), new string[] { }));
        }

        gameManager.firebaseDatabase.SetMatch(matchID, new MatchStruct(GamePhase.Round, 0, 0, 0, matchTeam1.ToArray(), matchTeam2.ToArray(), -1));
        gameManager.SetUpGamePlay(matchID, team1, teamData1, team2, teamData2); ;
    }

    public void OnFindingMatchPlayerChange(string[] userIDs)
    {
        Debug.Log("change");
        if (userIDs.Length <= 0) return;
        newFindingMatchPlayerList = new List<string>(userIDs);

        foreach (string id in makingMatchPlayerList)
        {
            if (oldFindingMatchPlayerList.Contains(id))
            {
                int index = oldFindingMatchPlayerList.IndexOf(id);
                oldCharacterDatas.RemoveAt(index);
                oldFindingMatchPlayerList.RemoveAt(index);
            }
            if (newFindingMatchPlayerList.Contains(id))
            {
                newFindingMatchPlayerList.Remove(id);
            }
            else
            {
                makingMatchPlayerList.Remove(id);
            }
        }

        for (int i = 0; i < oldFindingMatchPlayerList.Count; i++)
        {
            Debug.Log(oldCharacterDatas[i].playerName);
            if (!newFindingMatchPlayerList.Contains(oldFindingMatchPlayerList[i]))//remove player not finding game
            {
                oldFindingMatchPlayerList.RemoveAt(i);
                oldCharacterDatas.RemoveAt(i);
                i--;
            }
            else
            {
                newFindingMatchPlayerList.Remove(oldFindingMatchPlayerList[i]);//remove already exist player
            }
        }

        foreach (string id in newFindingMatchPlayerList)
        {
            gameManager.firebaseDatabase.GetPlayerStatus(id, (PlayerStatus playerStatus) =>
            {
                Debug.Log(playerStatus + " " + id);
                if (playerStatus == PlayerStatus.findingMatch)
                {
                    oldFindingMatchPlayerList.Add(id);
                    gameManager.firebaseDatabase.GetPlayerData(id, (CharacterDataStruct characterData) =>
                    {
                        oldCharacterDatas.Add(characterData);
                        newFindingMatchPlayerList.Remove(id);
                    });
                }
                else
                {
                    gameManager.firebaseDatabase.CancelFindingMatch(id);
                    newFindingMatchPlayerList.Remove(id);
                }
            });
        }
        if (userIDs.Length == 1) return;
    }
}
