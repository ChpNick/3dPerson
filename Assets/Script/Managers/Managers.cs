﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))] // Гарантируем существование различных диспетчеров. 
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(WeatherManager))]
[RequireComponent(typeof(ImagesManager))]
[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(MissionManager))]
[RequireComponent(typeof(DataManager))]
public class Managers : MonoBehaviour {
    // Статические свойства, которыми остальной код пользуется для доступа к диспетчерам.
    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static WeatherManager Weather { get; private set; }
    public static ImagesManager Images { get; private set; }
    public static AudioManager Audio { get; private set; }
    public static MissionManager Mission {get; private set;}
    public static DataManager Data {get; private set;}

//    Список диспетчеров, который просматривается в цикле во время стартовой последовательности.
    private List<IGameManager> _startSequence;

    void Awake() {
        DontDestroyOnLoad(gameObject); // Команда Unity для сохранения объекта между сценами.
        
        Data = GetComponent<DataManager>();
        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();
        Weather = GetComponent<WeatherManager>();
        Images = GetComponent<ImagesManager>();
        Audio = GetComponent<AudioManager>();
        Mission = GetComponent<MissionManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Inventory);
        _startSequence.Add(Player);
        _startSequence.Add(Weather);
        _startSequence.Add(Images);
        _startSequence.Add(Audio);
        _startSequence.Add(Mission);
        _startSequence.Add(Data);
        

        StartCoroutine(StartupManagers()); // Асинхронно загружаем стартовую последовательность. 
    }

    private IEnumerator StartupManagers() {
//        Создаются экземпляры объекта NetworkService для вставки во все диспетчеры.
        NetworkService network = new NetworkService();

        foreach (IGameManager manager in _startSequence) {
            manager.Startup(network);
            Debug.Log("---------");
        }

        yield return null;
        int numModules = _startSequence.Count;
        int numReady = 0;

        while (numReady < numModules) {
            // Продолжаем цикл, пока не начнут работать все диспетчеры. 
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager manager in _startSequence) {
                if (manager.status == ManagerStatus.Started) {
                    numReady++;
                }
            }

            if (numReady > lastReady) {
                Debug.Log("Progress: " + numReady + "/" + numModules);
                
                // Событие загрузки рассылается без параметров.
                Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS, numReady, numModules);
            }

            yield return null; // Остановка на один кадр перед следующей проверкой.
        }

        Debug.Log("All managers started up");
        Messenger.Broadcast(StartupEvent.MANAGERS_STARTED);
    }
}