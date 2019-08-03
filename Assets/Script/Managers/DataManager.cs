using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    private string _filename;

    private NetworkService _network;

    public void Startup(NetworkService service) {
        Debug.Log("Data manager starting...");
        _network = service;
        _filename = Path.Combine(
            Application.persistentDataPath, "game.dat"); // Генерируем полный путь к файлу game.dat. 
        status = ManagerStatus.Started;
    }

    public void SaveGameState() {
        // Словарь, который будет подвергнут сериализации. 
        Dictionary<string, object> gamestate = new Dictionary<string, object>();
        gamestate.Add("inventory", Managers.Inventory.GetData());
        gamestate.Add("health", Managers.Player.Health);
        gamestate.Add("maxHealth", Managers.Player.maxHealth);
        gamestate.Add("curLevel", Managers.Mission.curLevel);
        gamestate.Add("maxLevel", Managers.Mission.maxLevel);

        // Создаем файл по указанному адресу.
        FileStream stream = File.Create(_filename);
        BinaryFormatter formatter = new BinaryFormatter();

        //Сериализуем объект Dictionary как содержимое созданного файла.

        formatter.Serialize(stream, gamestate);
        stream.Close();
    }

    public void LoadGameState() {
        // Переход к загрузке только при наличии файла.
        if (!File.Exists(_filename)) {
            Debug.Log("No saved game");
            return;
        }

        // Словарь для размещения загруженных данных.
        Dictionary<string, object> gamestate;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(_filename, FileMode.Open);
        gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();

        // Обновляем диспетчеры, снабжая их десериализованными данными.
        Managers.Inventory.UpdateData((Dictionary<string, int>) gamestate["inventory"]);
        Managers.Player.UpdateData((int) gamestate["health"], (int) gamestate["maxHealth"]);
        Managers.Mission.UpdateData((int) gamestate["curLevel"], (int) gamestate["maxLevel"]);
        Managers.Mission.RestartCurrent();
    }
}