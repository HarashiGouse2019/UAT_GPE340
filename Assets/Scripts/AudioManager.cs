using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    //Get Sliders that are capable of changing music, audio, and master volume
    [SerializeField]
    private List<Slider> S_SFXVOLUME, S_BGMVOLUME, S_MASTERVOLUME;

    //And the mixers that we attach each source
    [SerializeField]
    public AudioMixerGroup SFXMixerGroup, BGMMixerGroup, MasterMixerGroup;

    [System.Serializable]
    public class Audio
    {
        public string name; // Name of the audio

        public AudioClip clip; //The Audio Clip Reference

        [Range(0f, 1f)]
        public float volume; //Adjust Volume

        [Range(.1f, 3f)]
        public float pitch; //Adject pitch

        public bool enableLoop; //If the audio can repeat

        [HideInInspector] public AudioSource source;
    }

    [System.Serializable]
    public class Music
    {
        public string name; // Name of the audio

        public AudioClip clip; //The Audio Clip Reference

        [Range(0f, 1f)]
        public float volume; //Adjust Volume

        [Range(.1f, 3f)]
        public float pitch; //Adject pitch

        public bool enableLoop; //If the audio can repeat

        [HideInInspector] public AudioSource source;
    }

    [SerializeField] private Audio[] getAudio;
    [SerializeField] private Music[] getMusic;

    private void Awake()
    {
        #region SINGLETON
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        #endregion

        //Loop through the audio that is defined in the inspector
        foreach (Audio a in getAudio)
        {
            //We're going to create a AudioSource, and assign some values for sound
            a.source = gameObject.AddComponent<AudioSource>();

            a.source.clip = a.clip;

            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.enableLoop;
            a.source.outputAudioMixerGroup = SFXMixerGroup;
        }

        //We're going to create a AudioSource, and assign some values for music
        foreach (Music m in getMusic)
        {
            m.source = gameObject.AddComponent<AudioSource>();

            m.source.clip = m.clip;

            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.enableLoop;
            m.source.outputAudioMixerGroup = BGMMixerGroup;
        }
    }
    /// <summary>
    /// Play audio and adjust its volume.
    /// </summary>
    /// 
    /// <param name="_name"></param>
    /// The audio clip by name.
    /// 
    /// <param name="_volume"></param>
    /// Support values between 0 and 100.
    ///
    public void PlayAudio(string _name, float _volume = 100, bool _oneShot = false)
    {
        Audio a = Array.Find(getAudio, sound => sound.name == _name);
        if (a == null)
        {
            Debug.LogWarning("Sound name " + _name + " was not found.");
            return;
        }
        else
        {
            switch (_oneShot)
            {
                case true:
                    a.source.PlayOneShot(a.clip, _volume / 100);
                    break;
                default:
                    a.source.Play();
                    a.source.volume = _volume / 100;
                    break;
            }

        }
    }

    public void Select()
    {
        string name = "Select";
        Audio a = Array.Find(getAudio, audio => audio.name == name);
        a.source.PlayOneShot(a.clip);
    }

    public void PlayMusic(string _name, float _volume = 100, bool _oneShot = false)
    {
        Music m = Array.Find(getMusic, music => music.name == _name);
        if (m == null)
        {
            Debug.LogWarning("Music name " + _name + " was not found.");
            return;
        }
        else
        {
            switch (_oneShot)
            {
                case true:
                    m.source.PlayOneShot(m.clip, _volume / 100);
                    break;
                default:
                    m.source.Play();
                    m.source.volume = _volume / 100;
                    break;
            }

        }
    }

    public void StopAudio(string _name)
    {
        Audio a = Array.Find(getAudio, sound => sound.name == _name);
        if (a == null)
        {
            Debug.LogWarning("Sound name " + _name + " was not found.");
            return;
        }
        else
        {
            a.source.Stop();
        }
    }

    public void StopMusic(string _name)
    {
        Music m = Array.Find(getMusic, music => music.name == _name);
        if (m == null)
        {
            Debug.LogWarning("Music name " + _name + " was not found.");
            return;
        }
        else
        {
            m.source.Stop();
        }
    }

    public AudioClip GetAudio(string _name, float _volume = 100)
    {
        Audio a = Array.Find(getAudio, sound => sound.name == _name);
        if (a == null)
        {
            Debug.LogWarning("Sound name " + _name + " was not found.");
            return null;
        }
        else
        {
            a.source.Play();
            a.source.volume = _volume / 100;
            return a.source.clip;
        }
    }

    public AudioMixerGroup GetMasterAudioMixer() => MasterMixerGroup;
    public AudioMixerGroup GetBGMAudioMixer() => BGMMixerGroup;
    public AudioMixerGroup GetSFXAudioMixer() => SFXMixerGroup;
    public List<Slider> GetMasterSlider() => S_MASTERVOLUME;
    public List<Slider> GetBGMSlider() => S_BGMVOLUME;
    public List<Slider> GetSFXSliders() => S_SFXVOLUME;
}
