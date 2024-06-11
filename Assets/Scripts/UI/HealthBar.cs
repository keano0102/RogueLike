using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private VisualElement root;
    private VisualElement healthBar;
    private Label healthText;
    private Label levelText;
    private Label xpText;

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        healthBar = root.Q<VisualElement>("HealthBar");
        healthText = root.Q<Label>("HealthText");
        levelText = root.Q<Label>("LevelText");
        xpText = root.Q<Label>("XPText");

        if (healthBar == null)
        {
            Debug.LogError("HealthBar element is niet gevonden in de UXML!");
        }
        if (healthText == null)
        {
            Debug.LogError("HealthText element is niet gevonden in de UXML!");
        }
        if (levelText == null)
        {
            Debug.LogError("LevelText element is niet gevonden in de UXML!");
        }
        if (xpText == null)
        {
            Debug.LogError("XPText element is niet gevonden in de UXML!");
        }

        // Initialiseer de health bar en labels met standaardwaarden
        SetValues(30, 30); // Voorbeeld initialisatie
        SetLevel(1); // Voorbeeld initialisatie
        SetXP(0); // Voorbeeld initialisatie
    }

    // Methode om de health waarden in te stellen en de UI bij te werken
    public void SetValues(int huidigeHitPoints, int maxHitPoints)
    {
        if (healthBar == null || healthText == null)
        {
            Debug.LogError("HealthBar of HealthText element is niet geïnitialiseerd!");
            return;
        }

        float percentage = (float)huidigeHitPoints / maxHitPoints * 100;

        healthBar.style.width = new Length(percentage, LengthUnit.Percent);

        healthText.text = $"{huidigeHitPoints}/{maxHitPoints} HP";
    }

    // Methode om het level in te stellen en de UI bij te werken
    public void SetLevel(int level)
    {
        if (levelText == null)
        {
            Debug.LogError("LevelText element is niet geïnitialiseerd!");
            return;
        }

        levelText.text = $"Level: {level}";
    }

    // Methode om de XP in te stellen en de UI bij te werken
    public void SetXP(int xp)
    {
        if (xpText == null)
        {
            Debug.LogError("XPText element is niet geïnitialiseerd!");
            return;
        }

        xpText.text = $"XP: {xp}";
    }
}
