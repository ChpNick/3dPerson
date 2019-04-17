using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkService {
//     URL-адрес для отправки запроса.
    private const string xmlApi =
        "http://api.openweathermap.org/data/2.5/weather?q=Ivanovo,ru&mode=xml&APPID=690bec1d62bcd846a34e1e6b3f020282";

    private const string jsonApi =
        "http://api.openweathermap.org/data/2.5/weather?q=Ivanovo,ru&APPID=690bec1d62bcd846a34e1e6b3f020282";
    
    private const string webImage = "http://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";

    private bool IsResponseValid(WWW www) {
//      Проверка ответа на наличие ошибок. 
        if (www.error != null) {
            Debug.Log("bad connection");
            return false;
        }
        else if (string.IsNullOrEmpty(www.text)) {
            Debug.Log("bad data");
            return false;
        }
        else {
            // все хорошо
            return true;
        }
    }

    private IEnumerator CallAPI(string url, Action<string> callback) {
        WWW www = new WWW(url); // HTTP-запрос, отправленный путем создания веб-объекта. 

        yield return www; // Пауза в процессе скачивания.

        if (!IsResponseValid(www))
            yield break; // Прерывание сопрограммы в случае ошибки.

        callback(www.text); // Делегат может быть вызван так же, как и исходная функция.
    }

    public IEnumerator GetWeatherXML(Action<string> callback) {
        return CallAPI(xmlApi, callback); // Каскад ключевых слов yield в вызывающих друг друга методах сопрограммы.
    }

    public IEnumerator GetWeatherJSON(Action<string> callback) {
        return CallAPI(jsonApi, callback);
    }
    
    public IEnumerator DownloadImage(Action<Texture2D> callback) {
        Debug.Log("Download Image!!!");
        
        WWW www = new WWW(webImage);
        yield return www;
        callback(www.texture);
    }
}