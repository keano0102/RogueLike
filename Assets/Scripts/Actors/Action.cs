using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    static public void Move(Actor actor, Vector2 direction)
    {
        // See if someone is at the target position
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);

        // If not, we can move
        if (target == null)
        {
            actor.Move(direction);
            actor.UpdateFieldOfView();
        }

        // End turn in case this is the player
        EndTurn(actor);
    }

    static private void EndTurn(Actor actor)
    {
        // Check if the actor is the player
        if (actor.GetComponent<Player>() != null)
        {
            // Execute StartEnemyTurn of the GameManager
            GameManager.Get.StartEnemyTurn();
        }
    }
}
