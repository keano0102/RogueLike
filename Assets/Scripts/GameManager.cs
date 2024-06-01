using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Get { get => instance; }

    public List<Actor> Enemies { get; private set; } = new List<Actor>();
    public Actor Player;
    public List<Consumable> Items { get; private set; } = new List<Consumable>();

    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
    }

    public void SetPlayer(Actor player)
    {
        Player = player;
    }

    public GameObject CreateActor(string name, Vector2 position)
    {
        GameObject actor = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
        actor.name = name;
        return actor;
    }

    public Actor GetActorAtLocation(Vector3 location)
    {
        if (Player != null && Player.transform.position == location)
        {
            return Player;
        }

        foreach (Actor enemy in Enemies)
        {
            if (enemy.transform.position == location)
            {
                return enemy;
            }
        }

        return null;
    }

    public List<Actor> GetNearbyEnemies(Vector3 location)
    {
        List<Actor> nearbyEnemies = new List<Actor>();

        foreach (Actor enemy in Enemies)
        {
            if (Vector3.Distance(enemy.transform.position, location) < 5)
            {
                nearbyEnemies.Add(enemy);
            }
        }

        return nearbyEnemies;
    }

    public void StartEnemyTurn()
    {
        foreach (Actor enemy in Enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.RunAI();
            }
        }
    }

    public void RemoveEnemy(Actor enemy)
    {
        if (Enemies.Contains(enemy))
        {
            Enemies.Remove(enemy);
            Destroy(enemy.gameObject);
            Debug.Log($"{enemy.name} has been removed.");
        }
        else
        {
            Debug.Log("Enemy not found in the list.");
        }
    }

    public void AddItem(Consumable item)
    {
        Items.Add(item);
    }

    public void RemoveItem(Consumable item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            Destroy(item.gameObject);
            Debug.Log($"{item.name} has been removed.");
        }
        else
        {
            Debug.Log("Item not found in the list.");
        }
    }

    public Consumable GetItemAtLocation(Vector3 location)
    {
        foreach (Consumable item in Items)
        {
            if (item.transform.position == location)
            {
                return item;
            }
        }
        return null;
    }
}
