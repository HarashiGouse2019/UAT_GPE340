using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public string musicToPlay;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(musicToPlay);
    }
}
