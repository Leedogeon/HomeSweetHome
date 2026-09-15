using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(instance);
        }
    }

    public void SFXPlay(string sfxName,AudioClip clip)
    {
        GameObject sound = new GameObject(sfxName + "Sound");
        AudioSource audioSource = sound.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(sound,clip.length);

    }
}
