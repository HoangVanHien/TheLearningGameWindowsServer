using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[SerializeField]
public class Character
{
    public ServerGamePlay inGame;
    public bool isAnswerRight;
    public CharacterDataStruct characterData;

    public string userID;

    public List<int> ownedCards = new List<int>();
    public List<int> inHandCards = new List<int>();
    public List<int> haventPlayedCards = new List<int>();

    private ActionEvent[] preActions = new ActionEvent[(int)ActionType.Count];
    private ActionEvent[] afterActions = new ActionEvent[(int)ActionType.Count];

    public Character(CharacterDataStruct data, string userID, List<int> ownedCards)
    {
        inGame = null;
        isAnswerRight = false;
        characterData = data;
        this.userID = userID;
        this.ownedCards = ownedCards;
        this.haventPlayedCards = ownedCards;
    }

    public void TakePreAction(ActionType actionType, Character[] targets = null)
    {
        if (preActions[(int)actionType] != null) preActions[(int)actionType].Invoke(this, targets);
    }

    public void TakeAfterAction(ActionType actionType, Character[] targets = null)
    {
        if (afterActions[(int)actionType] != null) afterActions[(int)actionType].Invoke(this, targets);
    }

    public void AddAction(ActionEvent preAction, ActionEvent afterAction, ActionType actionType)
    {
        int type = (int)actionType;
        preActions[type].AddListener(preAction);
        afterActions[type].AddListener(afterAction);
    }

    public void RemoveAction(ActionEvent preAction, ActionEvent afterAction, ActionType actionType)
    {
        int type = (int)actionType;
        preActions[type].RemoveListener(preAction);
        afterActions[type].RemoveListener(afterAction);
    }

    public int GetStatData(CharacterDataEnum stat)
    {
        switch (stat)
        {
            case CharacterDataEnum.health:
                return characterData.health;
            case CharacterDataEnum.ATK:
                return characterData.ATK;
            case CharacterDataEnum.DEF:
                return characterData.DEF;
            case CharacterDataEnum.shield:
                return characterData.shield;
            default:
                return -1;
        }
    }

    public void UseCard(int card)
    {
        if (!inHandCards.Contains(card)) return;
        inHandCards.Remove(card);
    }

    public void DrawCard(int drawCount)
    {
        if (drawCount > haventPlayedCards.Count)
        {
            haventPlayedCards = new List<int>(haventPlayedCards);
        }
        for (int i = 0; i < drawCount; i++)
        {
            int index = Random.Range(0, haventPlayedCards.Count - 1);
            int card = haventPlayedCards[index];
            haventPlayedCards.RemoveAt(index);
            inHandCards.Add(card);
        }
    }

    public string[] ActionsInfo()
    {
        return new string[]{ };
    }
}
