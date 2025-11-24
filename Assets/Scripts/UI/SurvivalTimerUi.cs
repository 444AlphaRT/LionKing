using UnityEngine;
using TMPro;

public class SurvivalTimerUI : MonoBehaviour
{
    // Reference to the GameManager to read the survival time from
    [SerializeField] private GameManager gameManager;

    // Text component used to display the timer on screen
    [SerializeField] private TextMeshProUGUI timerText;

    // Text prefix shown before the time value
    [SerializeField] private string prefixText = "Time: ";

    // Number of seconds in one minute (kept as a field to avoid magic numbers)
    [SerializeField] private int secondsPerMinute = 60;

    private void Reset()
    {
        // Try to auto-assign the TextMeshPro component on the same object
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (gameManager == null || timerText == null)
        {
            return;
        }

        float totalSeconds = gameManager.GetSurvivalTimeSeconds();
        int wholeSeconds = Mathf.FloorToInt(totalSeconds);

        int minutes = wholeSeconds / secondsPerMinute;
        int seconds = wholeSeconds % secondsPerMinute;

        // Format as MM:SS
        string timeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.text = prefixText + timeFormatted;
    }
}