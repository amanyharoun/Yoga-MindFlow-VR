using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WatchManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI secondsText;
    void Start()
    {
        
    }

    void Update()
    {
        ShowTime();
    }
    public void ShowTime()
    {
        System.DateTime now = System.DateTime.Now;

        // Format hours and minutes
        string hoursAndMinutes = now.ToString("HH:mm");
        timeText.text = hoursAndMinutes;
        // Get the seconds
        string seconds = now.Second.ToString("D2"); // D2 ensures two digits, e.g., 01, 02
        secondsText.text=seconds;
        // Print to the console

    }
}
