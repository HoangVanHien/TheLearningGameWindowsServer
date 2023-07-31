using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameCard
{
    public CardAction[] cardActions;
    public ServerGamePlay gamePlay;

    public GameCard(CardAction[] cardActions, ServerGamePlay gamePlay)
    {
        this.cardActions = cardActions;
        this.gamePlay = gamePlay;
    }

    public GameCard(GameCardStruct gameCard, ServerGamePlay gamePlay)
    {
        List<CardAction> cardActions = new List<CardAction>();
        ActionEvent actionEvent = null;
        foreach (CardActionStruct cardActionStruct in gameCard.cardActions)
        {
            switch (cardActionStruct.actionEventType)
            {
                case ActionEventType.TakeDamage:
                    actionEvent = new AETakeDamgae(cardActionStruct.actionEventData);
                    break;
                case ActionEventType.Attack:
                    actionEvent = new AEAttack(cardActionStruct.actionEventData, gamePlay);
                    break;
                case ActionEventType.Defend:
                    break;
                default:
                    break;
            }
            cardActions.Add(new CardAction(cardActionStruct, actionEvent, gamePlay));
        }
        this.cardActions = cardActions.ToArray();
        this.gamePlay = gamePlay;
    }

    public void PlayCard(Character owner, Character[] targets)
    {
        foreach (CardAction card in cardActions)
        {
            card.TakeAction(owner, targets);
        }
    }

    private void OnDestroy()
    {
        foreach (CardAction card in cardActions)
        {
            card.RemoveAction();
        }
    }
}

public class CardAction
{
    private CardActionStruct cardActionStruct;
    private ActionEvent actionEvent;
    private Character actionOwner;
    private ServerGamePlay gamePlay;

    public CardAction(CardActionStruct cardActionStruct, ActionEvent actionEvent, ServerGamePlay gamePlay)
    {
        this.cardActionStruct = cardActionStruct;
        this.actionEvent = actionEvent;
        actionOwner = null;
        this.gamePlay = gamePlay;
    }

    public void TakeAction(Character actionOwner, Character[] targets)
    {
        this.actionOwner = actionOwner;
        foreach (CardActionType cardActionType in cardActionStruct.cardActionTypes)
        {
            switch (cardActionType.actionOrder)
            {
                case ActionOrderType.Instant:
                    actionEvent.Invoke(actionOwner, targets);
                    break;
                case ActionOrderType.PreAction:
                    actionOwner.AddAction(NewActionEvent(cardActionStruct.actionEventType, actionEvent), null, cardActionType.actionType);
                    break;
                case ActionOrderType.AfterAction:
                    actionOwner.AddAction(null, NewActionEvent(cardActionStruct.actionEventType, actionEvent), cardActionType.actionType);
                    break;
                case ActionOrderType.Both:
                    actionOwner.AddAction(NewActionEvent(cardActionStruct.actionEventType, actionEvent), NewActionEvent(cardActionStruct.actionEventType, actionEvent), cardActionType.actionType);
                    actionOwner.AddAction(NewActionEvent(cardActionStruct.actionEventType, actionEvent), NewActionEvent(cardActionStruct.actionEventType, actionEvent), cardActionType.actionType);
                    break;
                default:
                    break;
            }
        }
    }

    private ActionEvent NewActionEvent(ActionEventType actionEventType, ActionEvent actionEvent)
    {
        switch (actionEventType)
        {
            case ActionEventType.TakeDamage:
                if (actionEvent is not AETakeDamgae) return null;
                return new AETakeDamgae((AETakeDamgae)actionEvent);
            case ActionEventType.Attack:
                if (actionEvent is not AETakeDamgae) return null;
                return new AETakeDamgae((AETakeDamgae)actionEvent);
            default:
                return null;
        }
    }

    public void RemoveAction()
    {
        foreach (CardActionType cardActionType in cardActionStruct.cardActionTypes)
        {
            switch (cardActionType.actionOrder)
            {
                case ActionOrderType.PreAction:
                    actionOwner.RemoveAction(actionEvent, null, cardActionType.actionType);
                    break;
                case ActionOrderType.AfterAction:
                    actionOwner.RemoveAction(null, actionEvent, cardActionType.actionType);
                    break;
                case ActionOrderType.Both:
                    actionOwner.RemoveAction(actionEvent, actionEvent, cardActionType.actionType);
                    break;
                default:
                    break;
            }
        }
    }
}

public class ActionEvent
{
    protected UnityAction<Character, Character[]> action;
    protected ActionEventStruct actionEventData;
    protected ServerGamePlay gamePlay;

    public ActionEvent()
    {
        action = null;
    }

    public ActionEventStruct ActionEventData()
    {
        return actionEventData;
    }

    public virtual void AcionEventClone(ActionEvent toCloneActionEvent)
    {
        this.actionEventData = toCloneActionEvent.ActionEventData();
        action = MainAction;
        this.gamePlay = null;
    }

    public virtual void AcionEventClone(ActionEventStruct toCloneActionEvent)
    {
        this.actionEventData = toCloneActionEvent;
        action = MainAction;
    }

    protected int FinalNumber(Character owner)
    {
        int fn = actionEventData.pureNumber;
        foreach (ActionEventCharacterInvoleStruct cs in actionEventData.characterScales)
        {
            fn += (int)(owner.GetStatData(cs.characterStat) * cs.characterScaleNumber);
        }
        return fn;
    }

    protected virtual void MainAction(Character owner, Character[] targets)
    {

    }

    public void AddListener(ActionEvent action)
    {
        AddListener(action.action);
    }
    public void AddListener(UnityAction<Character, Character[]> action)
    {
        this.action += action;
    }

    public void RemoveListener(ActionEvent action)
    {
        RemoveListener(action.action);
    }
    public void RemoveListener(UnityAction<Character, Character[]> action)
    {
        this.action -= action;
    }

    public void RemoveAllListener()
    {
        this.action = null;
    }

    public void Invoke(Character owner, Character[] targets)
    {
        foreach (ActionType type in actionEventData.actionTypes)
        {
            owner.TakePreAction(type, targets);
        }
        action(owner, targets);
        foreach (ActionType type in actionEventData.actionTypes)
        {
            owner.TakeAfterAction(type, targets);
        }
    }
}

