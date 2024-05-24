using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private VisualElement root;
    private VisualElement healthBar;
    private Label healthLabel;

    // Start is called before the first frame update
    void Start()
    {
        // Fetch the root VisualElement from the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Assuming you have defined the health bar and label in the UXML
        healthBar = root.Q<VisualElement>("HealthBar");
        healthLabel = root.Q<Label>("HealthLabel");

        // Initialize the health bar and label with default values
        healthBar.style.width = new Length(100, LengthUnit.Percent); // Set to 100% width
        healthLabel.text = "100/100 HP";
    }

    // Method to set the health values and update the UI
    public void SetValues(int currentHitPoints, int maxHitPoints)
    {
        // Calculate the percentage of hit points remaining
        float percent = (float)currentHitPoints / maxHitPoints * 100;

        // Update the width of the health bar
        healthBar.style.width = new Length(percent, LengthUnit.Percent);

        // Update the text of the health label
        healthLabel.text = $"{currentHitPoints}/{maxHitPoints} HP";
    }
}
