using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using MiniJSON;

public class WeatherManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    // Сюда добавляется значение облачности (см. листинг 9.8)
    private NetworkService _network;

    // Облачность редактируется внутренне, в остальных местах это свойство предназначено только для чтения.
    public float cloudValue { get; private set; }

    public void Startup(NetworkService service) {
        Debug.Log("Weather manager starting...");
        _network = service; // Сохранение вставленного объекта NetworkService.

        StartCoroutine(_network.GetWeatherJSON(OnJsonDataLoaded));
        status = ManagerStatus.Initializing;
    }

    private void OnXMLDataLoaded(string data) {
        Debug.Log(data);

        XmlDocument doc = new XmlDocument();

        doc.LoadXml(data); // Разбиваем XML-код на структуру с возможностью поиска. 

        XmlNode root = doc.DocumentElement;
        XmlNode node = root.SelectSingleNode("clouds"); // Извлекаем из данных один узел.

        string value = node.Attributes["value"].Value;

        cloudValue = Convert.ToInt32(value) / 100f; // Преобразуем значение в число типа float в диапазоне от 0 до 1. 
        Debug.Log("Value: " + cloudValue);

        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);

        status = ManagerStatus.Started;
    }

    private void OnJsonDataLoaded(string data) {
        Debug.Log(data);

        Dictionary<string, object> dict;

        dict = Json.Deserialize(data) as Dictionary<string, object>;
        var clouds = (Dictionary<string, object>) dict["clouds"];

        cloudValue = (long) clouds["all"] / 100f;
        Debug.Log("Value: " + cloudValue);

        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);

        status = ManagerStatus.Started;
    }
}