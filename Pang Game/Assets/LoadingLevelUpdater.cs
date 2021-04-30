using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingLevelUpdater : MonoBehaviour
{
    public TextMeshProUGUI LevelNumber;
    public TextMeshProUGUI LoadingCountdown;
    public float countdown = 0;

    void FixedUpdate()
    {
        LevelNumber.text = "Level " + GameManager._instance.currentLevel.ToString();
        countdown -= Time.fixedDeltaTime;
        LoadingCountdown.text = (Mathf.Ceil(countdown*10)/10).ToString();
    }
}
