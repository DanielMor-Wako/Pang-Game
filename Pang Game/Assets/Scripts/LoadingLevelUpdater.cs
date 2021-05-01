using UnityEngine;
using TMPro;

public class LoadingLevelUpdater : MonoBehaviour
{
    public int currentLevel;
    public TextMeshProUGUI LevelNumber;

    public float countdown;
    public TextMeshProUGUI LoadingCountdown;

    public void LevelLoaderDisplay(int newLevel, float newCountdown)
    {
        currentLevel = newLevel;
        countdown = newCountdown;
    }

    void FixedUpdate()
    {
        // display level number
        LevelNumber.text = "Level " + currentLevel.ToString();

        //display countdown before level starts
        countdown -= Time.fixedDeltaTime;
        string countdownString = (Mathf.Ceil(countdown * 10) / 10).ToString();
        // stablaize the string to always have min 3 chars.
        // so output is 3.0, and not 3
        if (countdownString.Length == 1)
            countdownString = countdownString+".0";

        LoadingCountdown.text = countdownString;
    }
}
