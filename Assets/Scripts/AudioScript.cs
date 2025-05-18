using System.Collections;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public static AudioScript instance;

    public bool isPlaying = false;
    public bool alarmActive = false;

    public AudioSource voiceSource;
    public AudioClip[] soundClips;
    public AudioSource musicSource;
    public AudioClip musicClip;

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
        musicClip = Resources.Load<AudioClip>("AlarmSounds/Music/Morning Alarm");
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
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
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

        if (musicSource != null)
            musicSource.Stop();
    }

    public IEnumerator PlayAlarmAudio()
    {
        while (true)
        {
            voiceSource.clip = wakeUp;
            voiceSource.Play();
            yield return new WaitForSeconds(voiceSource.clip.length);

            voiceSource.clip = itIs;
            voiceSource.Play();
            yield return new WaitForSeconds(voiceSource.clip.length);

            voiceSource.clip = soundClips[TimeScript.instance.nowHour];
            voiceSource.Play();
            yield return new WaitForSeconds(voiceSource.clip.length);

            // Play minute
            if (TimeScript.instance.nowMinute != 0)
            {
                voiceSource.clip = soundClips[TimeScript.instance.nowMinute];
                voiceSource.Play();
                yield return new WaitForSeconds(voiceSource.clip.length);
            }

            // Play AM/PM
            int ampmIndex = TimeScript.instance.nowPM ? 60 : 0;
            voiceSource.clip = soundClips[ampmIndex];
            voiceSource.Play();
            yield return new WaitForSeconds(voiceSource.clip.length);

            yield return new WaitForSeconds(3);
        }
    }
}
