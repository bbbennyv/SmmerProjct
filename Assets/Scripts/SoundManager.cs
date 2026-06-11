using System;
using UnityEngine;


public enum SoundType
{
    PlayerJump,
}

[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    
    public static SoundManager instance;
    private AudioSource audioSource;
    [SerializeField] SoundList[] soundList; 
    

    #if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);

        for(int i = 0; i < soundList.Length; i++) soundList[i].name = names[i];
    }

    #endif


    [Serializable] public struct SoundList
    {
        [HideInInspector] public string name;
        [SerializeField] public AudioClip[] sounds;
    }
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume)
    {
        AudioClip[] clips = instance.soundList[(int)sound].sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        instance.audioSource.PlayOneShot(randomClip, volume);
    }
}

