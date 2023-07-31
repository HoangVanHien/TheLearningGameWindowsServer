using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AETakeDamgae : ActionEvent
{

    public AETakeDamgae(AETakeDamgae takeDamgae)
    {
        AcionEventClone(takeDamgae);
    }

    public AETakeDamgae(ActionEventStruct actionEvent)
    {
        AcionEventClone(actionEvent);
    }

    protected override void MainAction(Character owner, Character[] targets)//source is the one who take the damage, targets are those who attack
    {
        if (owner.characterData.shield > 0)
        {
            owner.characterData.shield--;
            return;
        }

        int damage = FinalNumber(owner);
        if (damage < 1) damage = 1;
        owner.characterData.health -= damage;
    }
}
