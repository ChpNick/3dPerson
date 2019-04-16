using UnityEngine;
using System.Collections;

public class WeatherController : MonoBehaviour {
    // Ссылаться можно не только на объекты сцены, но и на материал на вкладке Project.
    [SerializeField] private Material sky;
    [SerializeField] private Light sun;

    private float _fullIntensity;
//    private float _cloudValue = 0f;

    // Добавляем/удаляем подписчиков на событие.
    void Awake() {
        Messenger.AddListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated);
    }

    void OnDestroy() {
        Messenger.RemoveListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated);
    }

    void Start() {
        _fullIntensity = sun.intensity; // Исходная интенсивность света считается «полной». 
    }

//    void Update() {
//        SetOvercast(_cloudValue);
//        _cloudValue += .005f; // Для непрерывности перехода увеличивайте значение в каждом кадре. 
//    }

    private void OnWeatherUpdated() {
        SetOvercast(Managers.Weather.cloudValue); // Используем значение облачности из сценария WeatherManager.
    }

    private void SetOvercast(float value) {
        // Корректируем как значение материала Blend, так и интенсивность света.
        Debug.Log("SET OVER CAST");
        Debug.Log(value);
        sky.SetFloat("_Blend", value);
        sun.intensity = _fullIntensity - (_fullIntensity * value);
    }
}