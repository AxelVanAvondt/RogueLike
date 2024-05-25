using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    static public void MoveOrHit(Actor actor, Vector2 direction)
    {
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);

        if (target == null)
        {
            Move(actor, direction);
        }
        else
        {
            Hit(actor, target);
        }

        EndTurn(actor);
    }
    static public void Move(Actor actor, Vector2 direction)
    {
        actor.Move(direction);
        actor.UpdateFieldOfView();
    }
    static public void Hit(Actor actor, Actor target)
    {
        int damage = actor.Power - target.Defense;
        Color c = Color.red;
        string message = $"{actor.name} tried to attack {target.name}, but did no damage";
        if(actor == actor.GetComponent<Player>())
        {
            c = Color.white;
        }
        if(damage > 0)
        {
            target.DoDamage(damage);
            message = $"{actor.name} did {damage} damage to {target.name}";
        }
        UIManager.Get.AddMessage(message, c);
    }
    static private void EndTurn(Actor actor)
    {
        if(actor.GetComponent<Player>() != null)
        {
            GameManager.Get.StartEnemyTurn();
        }
    }
}
