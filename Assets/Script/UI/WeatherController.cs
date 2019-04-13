using UnityEngine;
using System.Collections;
public class WeatherController : MonoBehaviour {
    [SerializeField] private Material sky; // Ссылаться можно не только на объекты сцены, но и на материал на вкладке Project. 
    [SerializeField] private Light sun; 
    
    private float _fullIntensity;
    private float _cloudValue = 0f;
    
    void Start() {
        _fullIntensity = sun.intensity; // Исходная интенсивность света считается «полной». 
    }
    void Update() {
        SetOvercast(_cloudValue);
        _cloudValue += .005f; // Для непрерывности перехода увеличивайте значение в каждом кадре. 
    }
    private void SetOvercast(float value) { // Корректируем как значение материала Blend, так и интенсивность света.
            sky.SetFloat("_Blend", value); 
            sun.intensity = _fullIntensity - (_fullIntensity * value);
    }
}