using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager Instance { get; private set; }

    [Header("Documents")]
    public GameObject HealthBar;
    public GameObject Messages;

    private HealthBarController healthBarController;
    private Messages messagesController;

    private void Awake()
    {
        // Implement singleton pattern
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
        // Get the script components from the GameObjects
        if (HealthBar != null)
        {
            healthBarController = HealthBar.GetComponent<HealthBarController>();
        }
        if (Messages != null)
        {
            messagesController = Messages.GetComponent<Messages>();
        }
    }

    public void UpdateHealth(int current, int max)
    {
        if (healthBarController != null)
        {
            healthBarController.SetValues(current, max);
        }
    }

    public void AddMessage(string message, Color color)
    {
        if (messagesController != null)
        {
            messagesController.AddMessage(message, color);
        }
    }
}
