using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private AdamMilVisibility algorithm;
    public List<Vector3Int> FieldOfView = new List<Vector3Int>();
    public int FieldOfViewRange = 8;

    [Header("Powers")]
    [SerializeField] private int maxHitPoints = 30;  // Max HP is set to 30
    [SerializeField] private int hitPoints = 30;     // Initial HP is set to 30
    [SerializeField] private int defense;
    [SerializeField] private int power;
    [SerializeField] private int level = 1;
    [SerializeField] private int xp = 0;
    [SerializeField] private int xpToNextLevel = 100;

    // Public getters for the private variables
    public int MaxHitPoints => maxHitPoints;
    public int HitPoints => hitPoints;
    public int Defense => defense;
    public int Power => power;
    public int Level => level;
    public int XP => xp;
    public int XPToNextLevel => xpToNextLevel;

    private void Start()
    {
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();

        // If this actor is the player, update the health and level in the UIManager
        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
            UIManager.Instance.UpdateLevel(level);
            UIManager.Instance.UpdateXP(xp);
        }
    }

    private void Die()
    {
        // Display the death message through the UIManager
        if (GetComponent<Player>())
        {
            UIManager.Instance.AddMessage("You died!", Color.red);
        }
        else
        {
            UIManager.Instance.AddMessage($"{name} is dead!", Color.green);
            GameManager.Get.RemoveEnemy(this);
        }

        // Create a gravestone at the actor's position
        GameObject gravestone = GameManager.Get.CreateActor("Dead", transform.position);
        gravestone.name = $"Remains of {name}";

        // Destroy this actor's game object
        Destroy(gameObject);
    }

    public void DoDamage(int hp, Actor attacker)
    {
        hitPoints -= hp;
        if (hitPoints < 0)
        {
            hitPoints = 0;
        }

        // If this actor is the player, update the health bar
        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        }

        // If hitPoints are 0, call Die()
        if (hitPoints == 0)
        {
            Die();
            // Check if the attacker is the player
            if (attacker != null && attacker.GetComponent<Player>())
            {
                attacker.AddXP(xp);
            }
        }
    }

    public void Heal(int hp)
    {
        int previousHitPoints = hitPoints;
        hitPoints += hp;
        if (hitPoints > maxHitPoints)
        {
            hitPoints = maxHitPoints;
        }

        int healedAmount = hitPoints - previousHitPoints;

        // If this actor is the player, update the health bar and show a message
        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
            UIManager.Instance.AddMessage($"You have been healed by {healedAmount} hit points.", Color.green);
        }
    }

    public void AddXP(int xp)
    {
        this.xp += xp;
        while (this.xp >= xpToNextLevel)
        {
            this.xp -= xpToNextLevel;
            LevelUp();
        }

        // If this actor is the player, update the XP and level in the UIManager
        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateXP(this.xp);
            UIManager.Instance.UpdateLevel(level);
        }
    }

    private void LevelUp()
    {
        level++;
        xpToNextLevel = Mathf.CeilToInt(xpToNextLevel * 1.5f); // Increase the XP needed for next level exponentially
        maxHitPoints += 10; // Increase max HP on level up
        defense += 2; // Increase defense on level up
        power += 2; // Increase power on level up
        hitPoints = maxHitPoints; // Heal to max HP on level up

        // If this actor is the player, show a level up message and update the health bar
        if (GetComponent<Player>())
        {
            UIManager.Instance.AddMessage("Congratulations, you leveled up!", Color.yellow);
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        }
    }

    public void Move(Vector3 direction)
    {
        if (MapManager.Get.IsWalkable(transform.position + direction))
        {
            transform.position += direction;
        }
    }

    public void UpdateFieldOfView()
    {
        var pos = MapManager.Get.FloorMap.WorldToCell(transform.position);

        FieldOfView.Clear();
        algorithm.Compute(pos, FieldOfViewRange, FieldOfView);

        if (GetComponent<Player>())
        {
            MapManager.Get.UpdateFogMap(FieldOfView);
        }
    }
}
