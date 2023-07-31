using Google.Cloud.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataPath
{
    public static string playerCharacterData = "characterData/";
    public static string playerStatus = "playerStatus/";
    public static string gameCard = "gameCard/";
    public static string question = "question/";
    public static string findingMatch = "findingMatch";
    public static string match = "match/";
    public static string playerMatchID = "playerMatchID/";
    public static string playCard = "playCard";
}

public enum PlayerStatus
{
    online = 0,
    offline,
    findingMatch,
    inMatch,
    count,
};

public enum CharacterDataEnum
{
    health = 0,
    ATK,
    DEF,
    shield,
    Count,
}

[System.Serializable]
[FirestoreData]
public struct CharacterDataStruct
{
    [FirestoreProperty]
    public string playerName { get; set; }
    [FirestoreProperty]
    public int health { get; set; }
    [FirestoreProperty]
    public int ATK { get; set; }
    [FirestoreProperty]
    public int DEF { get; set; }
    [FirestoreProperty]
    public int shield { get; set; }

    public CharacterDataStruct(string playerName, int health, int ATK, int shield, int DEF)
    {
        this.playerName = playerName;
        this.health = health;
        this.ATK = ATK;
        this.DEF = DEF;
        this.shield = shield;
    }
}

[System.Serializable]
[FirestoreData]
public struct PlayerStatusStruct
{
    [FirestoreProperty]
    public PlayerStatus playerStatus { get; set; }

    public PlayerStatusStruct(PlayerStatus playerStatus)
    {
        this.playerStatus = playerStatus;
    }
}

[System.Serializable]
[FirestoreData]
public struct UserIDStruct
{
    [FirestoreProperty]
    public string userID { get; set; }

    public UserIDStruct(string userID)
    {
        this.userID = userID;
    }
}

[System.Serializable]
[FirestoreData]
public struct Question
{
    [FirestoreProperty]
    public string question { get; set; }
    [FirestoreProperty]
    public string[] answers { get; set; }

    public Question(string question, string[] answers)
    {
        this.question = question;
        this.answers = answers;
    }

}

[System.Serializable]
[FirestoreData]
public struct GameIndexInServerStruct
{
    public int gameID;
    public int gameIndex;

    public GameIndexInServerStruct(int gameID, int gameIndex)
    {
        this.gameID = gameID;
        this.gameIndex = gameIndex;
    }
}

[System.Serializable]
[FirestoreData]
public struct GameCardStruct
{
    [FirestoreProperty]
    public int cardID { get; set; }
    [FirestoreProperty]
    public CardActionStruct[] cardActions { get; set; }
    public GameCardStruct(int cardID, CardActionStruct[] cardActions)
    {
        this.cardID = cardID;
        this.cardActions = cardActions;
    }
}


public enum ActionType
{
    Action = 0,
    Turn,
    Round,
    Attack,
    Defend,
    HealthChange,
    HealthDecrease,
    HealthIncrease,
    Count,
}

public enum ActionOrderType
{
    Instant = 0,
    PreAction,
    AfterAction,
    Both,
    Count,
}

[System.Serializable]
[FirestoreData]
public struct CardActionType
{
    [FirestoreProperty]
    public ActionType actionType { get; set; }
    [FirestoreProperty]
    public ActionOrderType actionOrder { get; set; }

    public CardActionType(ActionType actionType, ActionOrderType actionOrder)
    {
        this.actionType = actionType;
        this.actionOrder = actionOrder;
    }
}

[System.Serializable]
[FirestoreData]
public struct CardActionStruct
{
    [FirestoreProperty]
    public CardActionType[] cardActionTypes { get; set; }
    [FirestoreProperty]
    public ActionEventType actionEventType { get; set; }
    [FirestoreProperty]
    public ActionEventStruct actionEventData { get; set; }

    public CardActionStruct(CardActionType[] cardActionTypes, ActionEventType actionEventType, ActionEventStruct actionEventData)
    {
        this.cardActionTypes = cardActionTypes;
        this.actionEventType = actionEventType;
        this.actionEventData = actionEventData;
    }
}

public enum ActionEventType
{
    TakeDamage = 0,
    Attack,
    Defend,
    Count,
}

[System.Serializable]
[FirestoreData]
public struct ActionEventCharacterInvoleStruct
{
    [FirestoreProperty]
    public float characterScaleNumber { get; set; }
    [FirestoreProperty]
    public CharacterDataEnum characterStat { get; set; }

    public ActionEventCharacterInvoleStruct(float characterScaleNumber, CharacterDataEnum characterSacle)
    {
        this.characterScaleNumber = characterScaleNumber;
        this.characterStat = characterSacle;
    }
}

[System.Serializable]
[FirestoreData]
public struct ActionEventStruct
{
    [FirestoreProperty]
    public int pureNumber { get; set; }
    [FirestoreProperty]
    public ActionEventCharacterInvoleStruct[] characterScales { get; set; }
    [FirestoreProperty]
    public ActionType[] actionTypes { get; set; }

    public ActionEventStruct(int pureNumber, ActionEventCharacterInvoleStruct[] characterScales, ActionType[] actionTypes)
    {
        this.pureNumber = pureNumber;
        this.characterScales = characterScales;
        this.actionTypes = actionTypes;
    }
}


public enum GamePhase
{
    Round,
    GapBetweenRound,
}

[System.Serializable]
[FirestoreData]
public struct MatchStruct
{
    [FirestoreProperty]
    public GamePhase gamePhase { get; set; }
    [FirestoreProperty]
    public int turn { get; set; }
    [FirestoreProperty]
    public int round { get; set; }
    [FirestoreProperty]
    public float timeRemain { get; set; }
    [FirestoreProperty]
    public MatchPersonalDataStruct[] team1 { get; set; }
    [FirestoreProperty]
    public MatchPersonalDataStruct[] team2 { get; set; }
    [FirestoreProperty]
    public int question { get; set; }

    public MatchStruct(GamePhase gamePhase, int turn, int round, float timeRemain, MatchPersonalDataStruct[] team1, MatchPersonalDataStruct[] team2, int question)
    {
        this.gamePhase = gamePhase;
        this.turn = turn;
        this.round = round;
        this.timeRemain = timeRemain;
        this.team1 = team1;
        this.team2 = team2;
        this.question = question;
    }
}

[System.Serializable]
[FirestoreData]
public struct MatchPersonalDataStruct
{
    [FirestoreProperty]
    public CharacterDataStruct CharacterData { get; set; }
    [FirestoreProperty]
    public string userID { get; set; }
    [FirestoreProperty]
    public int[] onHandCards { get; set; }
    [FirestoreProperty]
    public string[] actions { get; set; }

    public MatchPersonalDataStruct(CharacterDataStruct characterData, string userID, int[] onHandCards, string[] actions)
    {
        this.CharacterData = characterData;
        this.userID = userID;
        this.onHandCards = onHandCards;
        this.actions = actions;
    }
}

[System.Serializable]
[FirestoreData]
public struct PlayerMatchStruct
{
    [FirestoreProperty]
    public int matchID { get; set; }
    [FirestoreProperty]
    public bool isTeam1 { get; set; }

    public PlayerMatchStruct(int matchID, bool isTeam1)
    {
        this.matchID = matchID;
        this.isTeam1 = isTeam1;
    }
}

[System.Serializable]
[FirestoreData]
public struct PlayCardStruct
{
    [FirestoreProperty]
    public int matchID { get; set; }
    [FirestoreProperty]
    public int cardID { get; set; }
    [FirestoreProperty]
    public string owner { get; set; }
    [FirestoreProperty]
    public string[] targets { get; set; }

    public PlayCardStruct(int matchID, int cardID, string owner, string[] targets)
    {
        this.matchID = matchID;
        this.cardID = cardID;
        this.owner = owner;
        this.targets = targets;
    }
}