using System.Collections;
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

    // Publieke lijst van het type Actor
    public List<Actor> Enemies { get; private set; } = new List<Actor>();

    // Player variabele
    public Actor Player;

    // Publieke lijst van het type Consumable
    public List<Consumable> Items { get; private set; } = new List<Consumable>();

    // Voeg de functie toe met de gevraagde declaratie
    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
    }

    // Methode om de Player in te stellen
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

    // Werk de functie GetActorAtLocation uit
    public Actor GetActorAtLocation(Vector3 location)
    {
        // Controleer of de locatie gelijk is aan de positie van de Player
        if (Player != null && Player.transform.position == location)
        {
            return Player;
        }

        // Controleer of de locatie gelijk is aan de positie van een Enemy
        foreach (Actor enemy in Enemies)
        {
            if (enemy.transform.position == location)
            {
                return enemy;
            }
        }

        // Als er geen Actor gevonden is, geef null terug
        return null;
    }

    // Voeg de functie StartEnemyTurn toe
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

    // Functie om een vijand uit de lijst te verwijderen
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

    // Functie om een item toe te voegen aan de lijst van items
    public void AddItem(Consumable item)
    {
        Items.Add(item);
    }

    // Functie om een item uit de lijst van items te verwijderen
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

    // Functie om een item te krijgen op een bepaalde locatie
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
