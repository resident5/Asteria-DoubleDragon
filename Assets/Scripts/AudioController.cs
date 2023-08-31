using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixer audioMixer;

    public AudioSource masterAudioSource;
    public AudioSource sfxAudioSource;

    public AudioClip menuButtonSound;

    public static AudioController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSound()
    {
        Debug.Log("TESTING BUTTON SOUND");
        sfxAudioSource.clip = menuButtonSound;
        sfxAudioSource.Play();
    }
}
