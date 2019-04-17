using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImagesManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }
    private NetworkService _network;
    private Texture2D _webImage; // Переменная для сохранения скачанного изображения.

    public void Startup(NetworkService service) {
        Debug.Log("Images manager starting...");
        _network = service;
        status = ManagerStatus.Started;
    }

    public void GetWebImage(Action<Texture2D> callback) {
        if (_webImage == null) {
            // Проверяем, нет ли уже сохраненного изображения.
            StartCoroutine(_network.DownloadImage(callback));
        }
        else {
            callback(_webImage);
        }
    }
}