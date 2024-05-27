using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]
public class Enemy : MonoBehaviour
{
    public Actor Target { get; set; }
    public bool IsFighting { get; private set; } = false;
    public AStar Algorithm { get; private set; }

    void Start()
    {
        GameManager.Get.AddEnemy(GetComponent<Actor>());
        Algorithm = GetComponent<AStar>();
    }

    void Update()
    {
        RunAI();
    }

    public void MoveAlongPath(Vector2Int targetPosition)
    {
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector2 direction = Algorithm.Compute((Vector2Int)gridPosition, targetPosition);
        Action.Move(GetComponent<Actor>(), direction);
    }

    public void RunAI()
    {
        // Als target null is of vernietigd is, stel target in op de speler (vanuit GameManager)
        if (Target == null || Target.Equals(null))
        {
            Target = GameManager.Get.Player;
        }

        // Als de target nog steeds null is na de poging om deze toe te wijzen, keer dan terug
        if (Target == null)
        {
            return;
        }

        // Converteer de positie van de target naar een gridpositie
        Vector3Int targetGridPosition = MapManager.Get.FloorMap.WorldToCell(Target.transform.position);

        // Controleer eerst of er al gevochten wordt, omdat het controleren van het gezichtsveld meer CPU kost
        Vector3Int currentGridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        if (IsFighting || GetComponent<Actor>().FieldOfView.Contains(targetGridPosition))
        {
            // Als de vijand niet aan het vechten was, zou hij nu moeten vechten
            if (!IsFighting)
            {
                IsFighting = true;
            }

            // Bereken de afstand tot de target
            float distanceToTarget = Vector3.Distance(transform.position, Target.transform.position);

            // Als de afstand minder is dan 1.5, val dan de target aan
            if (distanceToTarget < 1.5f)
            {
                Action.Hit(GetComponent<Actor>(), Target);
            }
            else
            {
                // Anders, beweeg langs het pad naar de target
                MoveAlongPath((Vector2Int)targetGridPosition);
            }
        }
    }
}   