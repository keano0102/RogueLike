using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;

    // Voeg de drie boolean variabelen toe en initialiseer ze op false
    private bool inventoryIsOpen = false;
    private bool droppingItem = false;
    private bool usingItem = false;

    // Voeg een public Inventory toe
    public Inventory inventory;
    public InventoryUI inventoryUI;

    private void Awake()
    {
        controls = new Controls();
        inventory = GetComponent<Inventory>(); // Verondersteld dat het Inventory-component aan hetzelfde GameObject is gekoppeld
    }

    private void Start()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        GameManager.Get.Player = GetComponent<Actor>();
        inventoryUI = UIManager.Instance.InventoryUI; // Verkrijg de InventoryUI van de UIManager
    }

    private void OnEnable()
    {
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SetCallbacks(null);
        controls.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
                if (direction.y > 0)
                {
                    inventoryUI.SelectPreviousItem();
                }
                else if (direction.y < 0)
                {
                    inventoryUI.SelectNextItem();
                }
            }
            else
            {
                Move();
            }
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                inventoryUI.Hide();
                inventoryIsOpen = false;
                droppingItem = false;
                usingItem = false;
            }
        }
    }

    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Action.MoveOrHit(GetComponent<Actor>(), roundedDirection);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 playerPosition = transform.position;
            Consumable item = GameManager.Get.GetItemAtLocation(playerPosition);

            if (item == null)
            {
                Debug.Log("Geen item op deze locatie.");
            }
            else if (!inventory.AddItem(item))
            {
                Debug.Log("Inventory is vol. Kan het item niet oppakken.");
            }
            else
            {
                item.gameObject.SetActive(false);
                GameManager.Get.RemoveItem(item);
                Debug.Log($"{item.name} is toegevoegd aan de inventory.");
            }
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                inventoryUI.Show(GameManager.Get.Player.GetComponent<Inventory>().Items);
                inventoryIsOpen = true;
                droppingItem = true;
            }
        }
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                inventoryUI.Show(GameManager.Get.Player.GetComponent<Inventory>().Items);
                inventoryIsOpen = true;
                usingItem = true;
            }
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                Consumable selectedItem = inventory.Items[inventoryUI.Selected];
                inventory.DropItem(selectedItem);

                if (droppingItem)
                {
                    selectedItem.transform.position = transform.position;
                    GameManager.Get.AddItem(selectedItem);
                    selectedItem.gameObject.SetActive(true);
                }
                else if (usingItem)
                {
                    UseItem(selectedItem);
                    Destroy(selectedItem.gameObject);
                }

                inventoryUI.Hide();
                inventoryIsOpen = false;
                droppingItem = false;
                usingItem = false;
            }
        }
    }

    private void UseItem(Consumable item)
    {
        // Implement the functionality for using an item
        // For now, we will just log the item's name
        Debug.Log($"Using item: {item.name}");
    }
}
