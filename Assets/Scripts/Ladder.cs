using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool Up; // Bool om aan te geven of de ladder omhoog gaat

    void Start()
    {
        // Voeg deze ladder toe aan de GameManager
        GameManager.Get.AddLadder(this);
    }
}
