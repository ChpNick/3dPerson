﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    public int curLevel { get; private set; }
    public int maxLevel { get; private set; }

    private NetworkService _network;

    public void Startup(NetworkService service) {
        Debug.Log("Mission manager starting...");

        _network = service;

        curLevel = 0;
        maxLevel = 1;
        status = ManagerStatus.Started;
    }
    
    public void RestartCurrent() {
        string name = "Level" + curLevel;
        Debug.Log("Loading " + name);
        Application.LoadLevel(name);
    }

    public void GoToNext() {
        if (curLevel < maxLevel) {
            // Рассылаем аргументы вместе с объектом WWW, используя объект WWWForm.
            curLevel++;
            string name = "Level" + curLevel;
            Debug.Log("Loading " + name);
            Application.LoadLevel(name); // Проверяем, достигнут ли последний уровень.
        }
        else {
            Debug.Log("Last level");
        }
    }

    public void ReachObjective() {
        // здесь может быть код обработки нескольких целей
        Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
    }
}