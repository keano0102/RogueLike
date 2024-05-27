using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Consumable> Items { get; private set; } = new List<Consumable>();
    public int MaxItems;

    // Functie om een item toe te voegen aan de lijst
    public bool AddItem(Consumable item)
    {
        // Controleer of de lijst minder items bevat dan het maximum aantal items
        if (Items.Count < MaxItems)
        {
            Items.Add(item);
            return true;
        }
        // Als de lijst vol is, geef false terug
        return false;
    }

    // Functie om een item uit de lijst te verwijderen
    public void DropItem(Consumable item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
        }
    }
}
