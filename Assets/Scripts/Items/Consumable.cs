using UnityEngine;

public class Consumable : MonoBehaviour
{
    public enum ItemType
    {
        HealthPotion,
        Fireball,
        ScrollOfConfusion
    }

    [SerializeField]
    private ItemType type;

    public ItemType Type
    {
        get { return type; }
    }

    private void Start()
    {
        // Voeg dit consumable item toe aan de lijst met items in de GameManager
        GameManager.Get.AddItem(this);
    }
}
