using UnityEngine;
using System.Collections;

public class SettingsPopup : MonoBehaviour {
    [SerializeField] private AudioClip sound;

//    Кнопка переключает свойство mute диспетчера управления звуком.
    public void OnSoundToggle() {
        Managers.Audio.soundMute = !Managers.Audio.soundMute;
        Managers.Audio.PlaySound(sound);
    }

//    Ползунок регулирует свойство volume диспетчера управления звуком
    public void OnSoundValue(float volume) {
        Managers.Audio.soundVolume = volume;
    }

//    Этот метод получает от кнопки численный параметр
    public void OnPlayMusic(int selector) {
        Managers.Audio.PlaySound(sound);
//      Вызываем для каждой кнопки свою музыкальную функцию в диспетчере AudioManager.
        switch (selector) {
            case 1:
                Managers.Audio.PlayIntroMusic();
                break;
            case 2:
                Managers.Audio.PlayLevelMusic();
                break;
            default:
                Managers.Audio.StopMusic();
                break;
        }
    }

    public void OnMusicToggle() {
        Managers.Audio.musicMute = !Managers.Audio.musicMute;
        Managers.Audio.PlaySound(sound);
    }

    public void OnMusicValue(float volume) {
        Managers.Audio.musicVolume = volume;
    }
}