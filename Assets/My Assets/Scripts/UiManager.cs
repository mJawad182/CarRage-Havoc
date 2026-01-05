using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    public TextMeshProUGUI pointsText;
    private int totalPoints = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Optionally: DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (pointsText == null) 
        {
            Debug.LogError("pointsText is not assigned in the inspector!");
            return;
        }
        pointsText.text = totalPoints.ToString();
    }

    public void GainPoints(int points)
    {
        totalPoints += points;
        pointsText.text = totalPoints.ToString();
        Debug.Log("Coined");
    }
}
