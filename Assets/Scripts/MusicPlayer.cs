using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Widening")]
    [SerializeField] AudioClip WidenMusic;
    [SerializeField][Range(0f, 1f)] float WidenMusicVolume = 0.3f;
    static MusicPlayer instance;
    [SerializeField] AudioSource audioPlayer;
    public  AudioClip bgm;
    public float  bgmVolume;

    GameObject player;
    void Awake()
    {
        if(instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            player = GameObject.FindWithTag("Player");
            PlayBGM();
        }
    }

    void Update(){
            gameObject.transform.position = player.transform.position;
    }

    void PlayClip(AudioClip Clip, float volume)
    {

        audioPlayer.Stop();
        audioPlayer.clip = Clip;
        audioPlayer.volume = volume;

        audioPlayer.Play();
    }
    public void StopAudio()
    {
        audioPlayer.Stop();
    }

    public void PlayWidenMusic()
    {
        PlayClip(WidenMusic, WidenMusicVolume);
    }

    public void PlayBGM()
    {
        PlayClip(bgm, bgmVolume);
    }
}
