using System.Collections;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public static AudioScript instance;

    public bool isPlaying = false;
    public bool alarmActive = false;

    public AudioSource audioSource;
    public AudioClip[] soundClips;

    public AudioClip wakeUp;
    public AudioClip itIs;

    private Coroutine alarmCoroutine;

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

        soundClips = new AudioClip[61]; // 0 = AM, 1-59 = numbers, 60 = PM

        soundClips[0] = Resources.Load<AudioClip>("AlarmSounds/AMPM/AM");
        for (int i = 1; i <= 59; i++)
        {
            soundClips[i] = Resources.Load<AudioClip>("AlarmSounds/Numbers/" + i);
            if (soundClips[i] == null)
                Debug.LogWarning("Missing sound: AlarmSounds/Numbers/" + i);
        }
        soundClips[60] = Resources.Load<AudioClip>("AlarmSounds/AMPM/PM");

        wakeUp = Resources.Load<AudioClip>("AlarmSounds/Sentence/WakeUp");
        itIs = Resources.Load<AudioClip>("AlarmSounds/Sentence/ItIs");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAlarmAudio()
    {
        isPlaying = true;
        if (alarmCoroutine != null) StopCoroutine(alarmCoroutine);
        alarmCoroutine = StartCoroutine(PlayAlarmAudio());
    }

    public void StopAlarmAudio()
    {
        if (alarmCoroutine != null)
        {
            StopCoroutine(alarmCoroutine);
            alarmCoroutine = null;
        }
        isPlaying = false;
    }

    public IEnumerator PlayAlarmAudio()
    {
        while (true)
        {
            audioSource.clip = wakeUp;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);

            audioSource.clip = itIs;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);

            audioSource.clip = soundClips[TimeScript.instance.nowHour];
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);

            // Play minute
            if (TimeScript.instance.minutes != 0)
            {
                audioSource.clip = soundClips[TimeScript.instance.nowMinute];
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);
            }

            // Play AM/PM
            int ampmIndex = TimeScript.instance.PM ? 60 : 0;
            audioSource.clip = soundClips[ampmIndex];
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);

            yield return new WaitForSeconds(3);
        }
    }
}
