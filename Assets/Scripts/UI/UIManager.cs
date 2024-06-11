using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Documenten")]
    public GameObject HealthBar; // Zorg ervoor dat dit is toegewezen in de inspector
    public GameObject Messages;
    public GameObject Inventory; // GameObject voor de inventory UI

    private HealthBar healthBar;
    private Messages messagesController;
    private InventoryUI inventoryUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (HealthBar != null)
        {
            healthBar = HealthBar.GetComponent<HealthBar>();
            if (healthBar == null)
            {
                Debug.LogError("HealthBar component is niet gevonden op het toegewezen HealthBarObject!");
            }
        }
        else
        {
            Debug.LogError("HealthBar is niet toegewezen in de UIManager!");
        }

        if (Messages != null)
        {
            messagesController = Messages.GetComponent<Messages>();
        }

        if (Inventory != null)
        {
            inventoryUI = Inventory.GetComponent<InventoryUI>();
            if (inventoryUI == null)
            {
                Debug.LogError("InventoryUI component is niet gevonden op het toegewezen Inventory GameObject!");
            }
        }
        else
        {
            Debug.LogError("Inventory is niet toegewezen in de UIManager!");
        }

        if (messagesController != null)
        {
            messagesController.Clear();
            messagesController.AddMessage("Welkom in de kerker, Avonturier!", Color.yellow);
        }
    }

    public void UpdateHealth(int current, int max)
    {
        if (healthBar != null)
        {
            healthBar.SetValues(current, max);
        }
        else
        {
            Debug.LogError("HealthBar component is niet toegewezen!");
        }
    }

    public void ShowInventory()
    {
        if (inventoryUI != null)
        {
            inventoryUI.Show();
        }
        else
        {
            Debug.LogError("InventoryUI component is niet toegewezen!");
        }
    }

    public void HideInventory()
    {
        if (inventoryUI != null)
        {
            inventoryUI.Hide();
        }
        else
        {
            Debug.LogError("InventoryUI component is niet toegewezen!");
        }
    }
}
