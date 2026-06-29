using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{

    public GlobalDictionarySO  bgmJam;
    [Header("Widening")]
    [SerializeField] AudioClip WidenMusic;
    [SerializeField][Range(0f, 1f)] float WidenMusicVolume = 0.3f;
    static MusicPlayer instance;
    [SerializeField] AudioSource audioPlayer;
    public  AudioClip bgm;
    public float  bgmVolume;

    MusicInit music;
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

     void OnEnable()
     {
          SceneManager.sceneLoaded+= OnSceneLoaded;
     }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioPlayer.Stop();
        bgm = null;
        music = FindAnyObjectByType<MusicInit>();
        if(music != null)
        {
            if(music.clip == null) return;
            bgm = music.clip;
            bgmVolume = music.volume;
            PlayBGM();
        }
        /*(string curry = scene.name;
        if(bgmJam.Runtimezdict.ContainsKey(curry))
        {
            bgm = bgmJam.Runtimezdict[curry];
            bgmVolume = 0.5f;
            PlayBGM();
        }*/
    }
     void Update(){
        if(player != null) 
            gameObject.transform.position = player.transform.position;
    }

     void OnDisable()
     {
           audioPlayer.Stop();
          SceneManager.sceneLoaded -= OnSceneLoaded;
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
