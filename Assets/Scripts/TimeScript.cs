using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{
    public static TimeScript instance;

    public int hours = 0;
    public int minutes = 0;
    public int seconds = 0;
    public bool PM = false;

    public int nowHour;
    public int nowMinute;
    public int nowSecond;
    public bool nowPM;

    public TextMeshProUGUI timeText;
    public TMP_InputField timeInput;
    public TextMeshProUGUI[] buttonTexts;

    public GameObject alarmActiveWindow;

    private Vector2 inputFieldOriginalPos;
    private int editingIndex = -1;

    public bool AlarmActive = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inputFieldOriginalPos = timeInput.GetComponent<RectTransform>().anchoredPosition;
        timeInput.gameObject.SetActive(false);
    }

    private void Update()
    {
        System.DateTime now = System.DateTime.Now;
        timeText.text = now.ToString("hh:mm:ss tt");

        nowHour = now.Hour % 12;
        if (nowHour == 0) 
            nowHour = 12;
        nowMinute = now.Minute;
        nowSecond = now.Second;
        nowPM = now.Hour >= 12;

        if (AlarmActive)
        {
            if(nowHour == hours && nowMinute == minutes && nowSecond == seconds)
            {
                if (!AudioScript.instance.isPlaying)
                    alarmActiveWindow.SetActive(true);
            } 
        }
    }

    private void UpdateTime(int unitIndex, int timeValue)
    {
        switch(unitIndex)
        {
            case 0:
                seconds = timeValue; 
                break;
            case 1:
                minutes = timeValue;
                break;
            case 2:
                hours = timeValue;
                break;
        }
    }

    public void OnAlarmActiveButtonClicked(Button clickedButton)
    {
        TextMeshProUGUI buttonText = clickedButton.GetComponentInChildren<TextMeshProUGUI>();
        Image buttonImage = clickedButton.GetComponent<Image>();
        if (AlarmActive)
        {
            buttonText.text = "OFF";
            buttonImage.color = Color.gray;
            AlarmActive = false;
        }
        else
        {
            buttonText.text = "ON";
            buttonImage.color = Color.green;
            AlarmActive = true;
        }
    }

    public void OnAmPmSetClicked(Button clickedButton)
    {
        TextMeshProUGUI buttonText = clickedButton.GetComponentInChildren <TextMeshProUGUI>();
        if(!PM)
        {
            PM = true;
            buttonText.text = "PM";
        }
        else
        {
            PM = false;
            buttonText.text = "AM";
        }
    }

    public void OnSetTimeButtonClicked(int timeUnit)
    {
        editingIndex = timeUnit;
        timeInput.text = buttonTexts[timeUnit].text;
        timeInput.gameObject.SetActive(true);
        buttonTexts[timeUnit].gameObject.SetActive(false);
        timeInput.ActivateInputField();

        RectTransform btnRect = buttonTexts[timeUnit].GetComponentInParent<Button>().GetComponent<RectTransform>();
        RectTransform inputRect = timeInput.GetComponent<RectTransform>();
        inputRect.position = btnRect.position;
    }

    public void OnInputEndEdit(string time)
    {
        if (editingIndex >= 0 && editingIndex < buttonTexts.Length)
        {
            buttonTexts[editingIndex].text = CheckAndUpdateTime(editingIndex, timeInput.text);
            buttonTexts[editingIndex].gameObject.SetActive(true);
            UpdateTime(editingIndex, int.Parse(buttonTexts[editingIndex].text));
        }

        RectTransform inputRect = timeInput.GetComponent<RectTransform>();
        inputRect.anchoredPosition = inputFieldOriginalPos;

        timeInput.gameObject.SetActive(false);
        editingIndex = -1;
    }
    
    public void OnStopAlarmClicked()
    {
        AudioScript.instance.alarmActive = false;
        alarmActiveWindow.SetActive(false);
    }

    private string CheckAndUpdateTime(int buttonIndex, string time)
    {
        int inputTime = int.Parse(time);
        switch (buttonIndex)
        {
            case 0: //seconds
            case 1: //minutes
                if (inputTime >= 0 && inputTime <= 59)
                    return inputTime.ToString("D2");
                else return "00";
            case 2: //hours
                if (inputTime >= 1 && inputTime <= 12)
                    return inputTime.ToString("D2");
                else return "01";
            default:
                return "00";
        }
    }
}
