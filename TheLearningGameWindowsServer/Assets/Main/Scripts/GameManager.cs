using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public FirebaseDatabaseManager firebaseDatabase;
    public MatchMakingManager matchMakingManger;
    public GamePlayManager gamePlayManger;


    private void Awake()
    {
        if (GameManager.instance)//if there is already a GameManager
        {
            Destroy(gameObject);//Destroy the new GameManager that being duplicated
            return;
        }
        instance = this;//To make all the instance being call become this
        DontDestroyOnLoad(gameObject);//prevent this gameObject (GameManager) from being deleted when load a new scene


    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(AppDomain.CurrentDomain.BaseDirectory + @"cloudfire.json");
        string path = AppDomain.CurrentDomain.BaseDirectory + @"cloudfire.json";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"E:\C#\Unity\TheLearningGameWindowsServer\TheLearningGameWindowsServer\cloudfire.json");

        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        FirestoreDb db = FirestoreDb.Create("the-learning-game-143304");

        firebaseDatabase.InitializeFirebase(this, db);
        firebaseDatabase.OnFindingMatchPlayerChangeAddListener(matchMakingManger.OnFindingMatchPlayerChange);
        firebaseDatabase.OnPlayCardChangeAddListener(gamePlayManger.OnPlayCard);

        firebaseDatabase.RandomQuestGen();
        firebaseDatabase.CardGen();
    }

    public void SetUpGamePlay(int matchID, string[] team1ID, CharacterDataStruct[] team1Data, string[] team2ID, CharacterDataStruct[] team2Data)
    {
        gamePlayManger.AddNewGamePlay(matchID, team1ID, team1Data, team2ID, team2Data);
    }
}
