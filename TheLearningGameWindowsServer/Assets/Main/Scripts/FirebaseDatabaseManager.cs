using Google.Cloud.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseDatabaseManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private FirestoreDb database;

    [SerializeField] private FirestoreChangeListener findingMatchPlayerListenerRegistration;
    private UnityEvent<string[]> onFindingMatchPlayerChange = new UnityEvent<string[]>();

    [SerializeField] private FirestoreChangeListener playCardListenerRegistration;
    private UnityEvent<PlayCardStruct> onPlayCardChange = new UnityEvent<PlayCardStruct>();

    public void InitializeFirebase(GameManager gameManager, FirestoreDb db)
    {
        this.gameManager = gameManager;
        database = db;
        FindingMatchPlayerListenerRegistration();
        PlayCardListenerRegistration();

        Debug.Log("Done Database");
    }

    public void FindingMatchPlayerListenerRegistration()
    {
        GetPlayerData("jlDyXcVnEkbTWDJLroTmOpaMUVG3", (data) =>
        {
            Debug.Log(data.playerName);
        });
        findingMatchPlayerListenerRegistration = database.Collection(DataPath.findingMatch).Document("jlDyXcVnEkbTWDJLroTmOpaMUVG3").Listen(snapshot =>
        {
            Debug.Log("Change");
            //List<string> userIDs = new List<string>();
            //foreach (DocumentSnapshot doc in snapshot)
            //{
            //    UserIDStruct userID = doc.ConvertTo<UserIDStruct>();
            //    userIDs.Add(userID.userID);
            //}
            //onFindingMatchPlayerChange.Invoke(userIDs.ToArray());
        });
    }

    public void OnFindingMatchPlayerChangeAddListener(UnityAction<string[]> action)
    {
        onFindingMatchPlayerChange.AddListener(action);
    }

    public void PlayCardListenerRegistration()
    {
        playCardListenerRegistration = database.Collection(DataPath.playCard).Listen(snapshot =>
        {
            PlayCardStruct playCard;
            foreach (DocumentSnapshot doc in snapshot)
            {
                playCard = doc.ConvertTo<PlayCardStruct>();
                onPlayCardChange.Invoke(playCard);
            }
        });
    }

    public void OnPlayCardChangeAddListener(UnityAction<PlayCardStruct> action)
    {
        onPlayCardChange.AddListener(action);
    }

    private async void OnDestroy()
    {
        if (findingMatchPlayerListenerRegistration != null) await findingMatchPlayerListenerRegistration.StopAsync();
        onFindingMatchPlayerChange.RemoveAllListeners();
        if (playCardListenerRegistration != null) await playCardListenerRegistration.StopAsync();
        onPlayCardChange.RemoveAllListeners();
    }

    public void SetPlayerData(string userID, CharacterDataStruct characterData)
    {
        database.Document(DataPath.playerCharacterData + userID).SetAsync(characterData);
    }

    public async void GetPlayerData(string userID, UnityAction<CharacterDataStruct> action)
    {
        DocumentSnapshot doc = await database.Document(DataPath.playerCharacterData + userID).GetSnapshotAsync();
        Debug.Log("Get data " + doc.Exists);
        if (doc.Exists)
        {
            CharacterDataStruct characterData = doc.ConvertTo<CharacterDataStruct>();

            Debug.Log("GD" + characterData.playerName);

            action(characterData);

        }
    }

    //public void GetPlayerData(string userID, UnityAction<CharacterDataStruct> action)
    //{
    //    Debug.Log("Get PD");
    //    database.Document(DataPath.playerCharacterData + userID).GetSnapshotAsync().ContinueWith(task =>
    //    {
    //        if (!task.IsCompleted)
    //        {
    //            Debug.LogWarning("Get PD task not complete");
    //            return;
    //        }
    //        if (!task.Result.Exists)
    //        {
    //            Debug.LogWarning("Get PD task Result not exist");
    //            return;
    //        }
    //        Debug.Log("Get data");

    //        CharacterDataStruct characterData = task.Result.ConvertTo<CharacterDataStruct>();

    //        Debug.Log("GD" + characterData.playerName);

    //        action(characterData);

    //    });
    //}

    public void SetPlayerStatus(string userID, PlayerStatus playerStatus)
    {
        database.Document(DataPath.playerStatus + userID).SetAsync(new PlayerStatusStruct(playerStatus));
    }

    public void GetPlayerStatus(string userID, UnityAction<PlayerStatus> action)
    {
        database.Document(DataPath.playerStatus + userID).GetSnapshotAsync().ContinueWith(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogWarning("Get PS task not complete");
                return;
            }
            if (!task.Result.Exists)
            {
                Debug.LogWarning("Get PS task Result not exist");
                return;
            }

            PlayerStatusStruct playerStatus = task.Result.ConvertTo<PlayerStatusStruct>();

            action(playerStatus.playerStatus);

        });
    }

    async public void CancelFindingMatch(string userID)
    {
        DocumentReference player = database.Collection(DataPath.findingMatch).Document(userID);
        await player.DeleteAsync();
    }

    public void SetMatch(int matchID, MatchStruct match)
    {
        database.Document(DataPath.match + matchID).SetAsync(match);
    }

    public void SetPlayerMatchID(string userID, PlayerMatchStruct playerMatchStruct)
    {
        database.Document(DataPath.playerMatchID + userID).SetAsync(playerMatchStruct);
    }

    async public void DeletePlayCardRequest(string owner)
    {
        DocumentReference playCard = database.Collection(DataPath.playCard).Document(owner);
        await playCard.DeleteAsync();
    }

    public void GetCardStruct(int cardID, UnityAction<GameCardStruct> action)
    {
        database.Document(DataPath.gameCard + cardID).GetSnapshotAsync().ContinueWith(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogWarning("Get CS task not complete");
                return;
            }
            if (!task.Result.Exists)
            {
                Debug.LogWarning("Get CS task Result not exist");
                return;
            }
            GameCardStruct gameCard = task.Result.ConvertTo<GameCardStruct>();

            action(gameCard);

        });
    }

    public void CardGen()
    {
        for (int i = 0; i < 200; i++)
        {
            int rj = Random.Range(1, 3);
            List<CardActionStruct> cardActions = new List<CardActionStruct>();
            for (int j = 0; j < rj; j++)
            {
                int rk = Random.Range(1, 3);
                List<CardActionType> cardActionTypes = new List<CardActionType>();
                for (int k = 0; k < rk; k++)
                {
                    cardActionTypes.Add(new CardActionType(
                        (ActionType)Random.Range(0, (int)ActionType.Count - 1),
                        (ActionOrderType)Random.Range(0,
                        (int)ActionOrderType.Count - 1)));
                }

                rk = Random.Range(1, 3);
                List<ActionEventCharacterInvoleStruct> characterScales = new List<ActionEventCharacterInvoleStruct>();
                for (int k = 0; k < rk; k++)
                {
                    characterScales.Add(new ActionEventCharacterInvoleStruct(Random.Range(0f, 3f), (CharacterDataEnum)Random.Range(0, (int)(CharacterDataEnum.Count - 1))));
                }

                cardActions.Add(new CardActionStruct(
                    cardActionTypes.ToArray(),
                    ActionEventType.Attack,
                    new ActionEventStruct(Random.Range(0, 20), characterScales.ToArray(), new ActionType[] { ActionType.Attack })));
            }
            database.Document(DataPath.gameCard + i).SetAsync(new GameCardStruct(i, cardActions.ToArray()));
        }
    }

    public void RandomQuestGen()
    {
        for (int i = 0; i < 25; i++)
        {
            int a = Random.Range(1, 200);
            int b = Random.Range(1, 200);
            int an = a + b;
            List<string> ans = new List<string>() { an.ToString() };
            for (int j = 1; j < Random.Range(4, 8); j++)
            {
                int plus = Random.Range(-20, 20);
                ans.Add((an + (plus != 0 ? plus : 1)).ToString());
            }
            database.Document(DataPath.question + i).SetAsync(new Question((a + " + " + b).ToString(), ans.ToArray()));
        }
    }
}
