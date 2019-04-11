using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Импорт новых структур данных (используемых в листинге 8.14).

public class InventoryManager : MonoBehaviour, IGameManager {
    // Свойство читается откуда угодно, но задается только в этом сценарии.
    public ManagerStatus status { get; private set; }

    private Dictionary<string, int> _items; // При объявлении словаря указывается два типа: тип ключа и тип значения.

    public void Startup() {
        // Сюда идут все задачи запуска с долгим временем выполнения
        Debug.Log("Inventory manager starting...");
        _items = new Dictionary<string, int>(); // Инициализируем словарь элементов.

        status = ManagerStatus.Started; // Для задач с долгим временем выполнения используем состояние 'Initializing’.
    }

    // Вывод на консоль сообщения о текущем инвентаре.
    private void DisplayItems() {
        string itemDisplay = "Items: ";

        foreach (KeyValuePair<string, int> item in _items) {
            itemDisplay += item.Key + "(" + item.Value + ") ";
        }

        Debug.Log(itemDisplay);
    }

    // Другие сценарии не могут напрямую управлять списком элементов, но могут вызывать этот метод.
    public void AddItem(string name) {
        // Проверка существующих записей перед вводом новых данных.
        if (_items.ContainsKey(name)) {
            _items[name] += 1;
        }
        else {
            _items[name] = 1;
        }

        DisplayItems();
    }

    public List<string> GetItemList() {
        List<string> list = new List<string>(_items.Keys); // Возвращаем список всех ключей словаря.
        return list;
    }

    public int GetItemCount(string name) {
        // Возвращаем количество указанных элементов в инвентаре. 
        if (_items.ContainsKey(name)) {
            return _items[name];
        }

        return 0;
    }
}