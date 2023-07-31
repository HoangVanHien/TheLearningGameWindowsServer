using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Cloud.Firestore;
using System;

public class Test : MonoBehaviour
{
    public static Test instance;
    //public FirebaseDatabaseManager firebaseDatabase;
    //public MatchMakingManager matchMakingManger;
    //public GamePlayManager gamePlayManger;

    private void Awake()
    {
        if (Test.instance)//if there is already a GameManager
        {
            Destroy(gameObject);//Destroy the new GameManager that being duplicated
            return;
        }
        instance = this;//To make all the instance being call become this
        DontDestroyOnLoad(gameObject);//prevent this gameObject (GameManager) from being deleted when load a new scene
    }

    void Start()
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + @"cloudfire.json";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

        FirestoreDb db = FirestoreDb.Create("the-learning-game-143304");

        //firebaseAuth.InitializeFirebase(this);
        //firebaseDatabase.InitializeFirebase(this, db);
        //firebaseDatabase.OnFindingMatchPlayerChangeAddListener(matchMakingManger.OnFindingMatchPlayerChange);
        //firebaseDatabase.OnPlayCardChangeAddListener(gamePlayManger.OnPlayCard);

        //firebaseDatabase.RandomQuestGen();
        //firebaseDatabase.CardGen();
    }
}
