using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePanel : MonoBehaviour
{

    public Text HoursText;
    public Text MinutesText;

    public Text MonthText;
    public Text DaysText;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private string[] Month = new string[]
    {
        "Jan.",
        "Feb.",
        "Mar.",
        "Apr.",
        "May",
        "June",
        "July",
        "Aug.",
        "Sept.",
        "Oct.",
        "Nov.",
        "Dec.",
    };
    // Update is called once per frame
    private void FixedUpdate()
    {
        HoursText.text = System.DateTime.Now.ToString("HH");
        MinutesText.text = System.DateTime.Now.ToString("mm");
        MonthText.text = Month[int.Parse( System.DateTime.Now.ToString("MM"))-1];
        DaysText.text = System.DateTime.Now.ToString("dd");
    }
}  