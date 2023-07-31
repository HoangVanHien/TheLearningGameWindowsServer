using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEAttack : ActionEvent
{
    public AEAttack(ActionEventStruct actionEvent, ServerGamePlay gamePlay)
    {
        AcionEventClone(actionEvent);
        this.gamePlay = gamePlay;
    }

    protected override void MainAction(Character owner, Character[] targets)
    {
        foreach (Character target in targets)
        {
            gamePlay.PlayCard(
                new GameCardStruct(
                    -1,
                    new CardActionStruct[] {
                        new CardActionStruct(
                            new CardActionType[] {
                                new CardActionType(ActionType.HealthChange, ActionOrderType.Instant),
                                new CardActionType(ActionType.HealthDecrease, ActionOrderType.Instant) },
                            ActionEventType.TakeDamage,
                            actionEventData) }),
                target,
                new Character[] { owner });
        }
    }
}
