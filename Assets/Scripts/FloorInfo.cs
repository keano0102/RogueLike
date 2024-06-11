using UnityEngine;
using UnityEngine.UI;

public class FloorInfo : MonoBehaviour
{
    public Text enemiesLeftText;
    public Text floorText;

    // Update the enemies left text
    public void UpdateEnemiesLeft(int enemiesLeft)
    {
        enemiesLeftText.text = $"{enemiesLeft} enemies left";
    }

    // Update the floor text
    public void UpdateFloor(int floor)
    {
        floorText.text = $"Floor {floor}";
    }
}
