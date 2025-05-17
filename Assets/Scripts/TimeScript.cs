using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TMP_InputField timeInput;
    public TextMeshProUGUI[] buttonTexts;

    private int editingIndex = -1;

    private void Start()
    {
        timeInput.gameObject.SetActive(false);
    }

    private void Update()
    {
        System.DateTime now = System.DateTime.Now;
        timeText.text = now.ToString("hh:mm:ss");
    }

    public void OnSetTimeButtonClicked(int timeUnit)
    {
        editingIndex = timeUnit;
        timeInput.text = buttonTexts[timeUnit].text;
        timeInput.gameObject.SetActive(true);
        buttonTexts[timeUnit].gameObject.SetActive(false);
        timeInput.ActivateInputField();
    }

    public void OnInputEndEdit(string time)
    {
        if(editingIndex >= 0 && editingIndex < buttonTexts.Length)
        {
            buttonTexts[editingIndex].text = time;
        }

        buttonTexts[editingIndex].gameObject.SetActive(true);
        timeInput.gameObject.SetActive(false);
        editingIndex = -1;
    }
    

}
