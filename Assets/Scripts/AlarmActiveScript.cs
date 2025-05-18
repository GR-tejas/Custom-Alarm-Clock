using UnityEngine;

public class AlarmActiveScript : MonoBehaviour
{
    private void OnEnable()
    {
        AudioScript.instance.StartAlarmAudio();
    }

    private void OnDisable()
    {
        AudioScript.instance.StopAlarmAudio();
    }
}