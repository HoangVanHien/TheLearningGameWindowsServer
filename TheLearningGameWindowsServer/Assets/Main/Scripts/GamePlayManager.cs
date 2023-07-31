using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public GameManager gameManager;

    public ServerGamePlay gamePlayPrefab;
    public Transform gamePlayParent;
    public List<ServerGamePlay> gamePlays = new List<ServerGamePlay>();

    public void AddNewGamePlay(int matchID, string[] team1ID, CharacterDataStruct[] team1Data, string[] team2ID, CharacterDataStruct[] team2Data)
    {
        ServerGamePlay gamePlay = Instantiate<ServerGamePlay>(gamePlayPrefab, gamePlayParent);
        List<Character> team1 = new List<Character>();
        List<Character> team2 = new List<Character>();
        List<int> ownedCards = new List<int>();
        for (int i = 0; i < 200; i++)
        {
            ownedCards.Add(i);
        }
        for (int i = 0; i < team1ID.Length; i++)
        {
            team1.Add(new Character(team1Data[i], team1ID[i], ownedCards));
            team2.Add(new Character(team2Data[i], team2ID[i], ownedCards));
        }
        gamePlay.SetUp(team1.ToArray(), team2.ToArray(), matchID, 20);
        gamePlays.Add(gamePlay);
    }

    public void OnPlayCard(PlayCardStruct playCard)
    {
        //gameManager.firebaseDatabase.DeletePlayCardRequest(playCard.owner);
        foreach (ServerGamePlay gamePlay in gamePlays)
        {
            if(gamePlay.matchID == playCard.matchID)
            {
                //gameManager.firebaseDatabase.GetCardStruct(playCard.cardID, (GameCardStruct gameCard) =>
                //{
                //    gamePlay.PlayCard(gameCard, playCard.owner, new List<string>(playCard.targets));
                //});
                return;
            }
        }
    }
}
