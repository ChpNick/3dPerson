using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    [SerializeField] private AudioSource soundSource;

    [SerializeField] private AudioSource music1Source;
    [SerializeField] private AudioSource music2Source;

    [SerializeField] private string introBGMusic; // имена музыкальных клипов
    [SerializeField] private string levelBGMusic;

//    Следим за тем, какой из источников активен, а какой нет.
    private AudioSource _activeMusic;
    private AudioSource _inactiveMusic;
    private float _musicVolume;

    public float crossFadeRate = 1.5f;
    private bool _crossFading; // Переключатель, позволяющий избежать ошибок в процессе перехода.


    private NetworkService _network;

    public float musicVolume {
        get { return _musicVolume; }
        set {
            _musicVolume = value;
            if (music1Source != null && !_crossFading) {
                music1Source.volume = _musicVolume;
                music2Source.volume = _musicVolume;
            }
        }
    }

    public bool musicMute {
        get { return music1Source.mute; }
        set {
            if (music1Source != null) {
                music1Source.mute = value;
                music2Source.mute = value;
            }
        }
    }

//    Свойство с функцией чтения и функцией доступа для громкости.
//    Реализуем функцию чтения/функцию доступа с помощью AudioListener.
    public float soundVolume {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }

//    Добавляем аналогичное свойство для выключения. 
    public bool soundMute {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

    public void Startup(NetworkService service) {
        Debug.Log("Audio manager starting...");
//        _network = service;


//        Эти свойства заставляют компонент AudioSource игнорировать громкость компонента AudioListener.
        music1Source.ignoreListenerVolume = true;
        music2Source.ignoreListenerVolume = true;

        music1Source.ignoreListenerPause = true;
        music2Source.ignoreListenerPause = true;

        soundVolume = 1f;
        musicVolume = 1f;

        _activeMusic = music1Source; // Инициализируем один из источников как активный. 
        _inactiveMusic = music2Source;

        status = ManagerStatus.Started;
    }

    //    Загрузка музыки intro из папки Resources.
    public void PlayIntroMusic() {
        PlayMusic(Resources.Load("Music/" + introBGMusic) as AudioClip);
    }

//    Загрузка основной музыки из папки Resources.
    public void PlayLevelMusic() {
        PlayMusic(Resources.Load("Music/" + levelBGMusic) as AudioClip);
    }

//    Воспроизведение музыки при помощи параметра AudioSource.clip.
    private void PlayMusic(AudioClip clip) {
//        music1Source.clip = clip;
//        music1Source.Play();
        if (_crossFading) {
            return;
        }

        StartCoroutine(CrossFadeMusic(clip)); //При изменении музыкальной композиции вызываем сопрограмму.
    }

    private IEnumerator CrossFadeMusic(AudioClip clip) {
        _crossFading = true;
        _inactiveMusic.clip = clip;
        _inactiveMusic.volume = 0;
        _inactiveMusic.Play();

        float scaledRate = crossFadeRate * _musicVolume;

        while (_activeMusic.volume > 0) {
            _activeMusic.volume -= scaledRate * Time.deltaTime;
            _inactiveMusic.volume += scaledRate * Time.deltaTime;
            yield return null; // Эта инструкция yield останавливает операции на один кадр. 
        }
        
        AudioSource temp = _activeMusic; 
        _activeMusic = _inactiveMusic;
        _activeMusic.volume = _musicVolume;
        _inactiveMusic = temp;
        _inactiveMusic.Stop();
        _crossFading = false;
    }

    public void StopMusic() {
        _activeMusic.Stop();
        _inactiveMusic.Stop();
    }

//    Воспроизводим звуки, не имеющие другого источника.
    public void PlaySound(AudioClip clip) {
        soundSource.PlayOneShot(clip);
    }
}