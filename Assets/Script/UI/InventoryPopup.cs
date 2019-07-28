using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class InventoryPopup : MonoBehaviour {
    [SerializeField] private Image[] itemIcons; // Массивы для ссылки на четыре
    [SerializeField] private Text[] itemLabels; // изображения и текстовые метки.

    [SerializeField] private Text curItemLabel;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button useButton;
    private string _curItem;

    public void Refresh() {
        List<string> itemList = Managers.Inventory.GetItemList();

        int len = itemIcons.Length;

        // Проверка списка инвентаря в процессе циклического 
        //просмотра всех изображений элементов UI.
        for (int i = 0; i < len; i++) {
            if (i < itemList.Count) {
                itemIcons[i].gameObject.SetActive(true);
                itemLabels[i].gameObject.SetActive(true);

                string item = itemList[i];

                // Загрузка спрайта из папки Resources
                // Изменение размеров изображения под исходный размер спрайта.
                Sprite sprite = Resources.Load<Sprite>("Icons/" + item);
                itemIcons[i].sprite = sprite;
                itemIcons[i].SetNativeSize();

                int count = Managers.Inventory.GetItemCount(item);
                string message = "x" + count;

                //На метке может появиться не только количество элементов, но и слово «Equipped».
                if (item == Managers.Inventory.equippedItem) {
                    message = "Equipped\n" + message;
                }

                itemLabels[i].text = message;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                // Превращаем значки в интерактивные объекты.
                entry.eventID = EventTriggerType.PointerClick;

                // Лямбда-функция, позволяющая по-разному активировать каждый элемент.
                entry.callback.AddListener((BaseEventData data) => { OnItem(item); });

                EventTrigger trigger = itemIcons[i].GetComponent<EventTrigger>();
                trigger.delegates.Clear(); // Сброс подписчика, чтобы начать с чистого листа. 
                trigger.delegates.Add(entry); // Добавление функции-подписчика к классу EventTrigger.
            }
            else {
                itemIcons[i].gameObject.SetActive(false); // Скрываем изображение/текст при отсутствии
                itemLabels[i].gameObject.SetActive(false); // элементов для отображения. }
            }
        }

        if (!itemList.Contains(_curItem)) {
            _curItem = null;
        }

        if (_curItem == null) {
            // Скрываем кнопки при отсутствии выделенных элементов. 
            curItemLabel.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        // Отображение выделенного в данный момент элемента.
        else {
            curItemLabel.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);
            if (_curItem == "health") {
                useButton.gameObject.SetActive(true);
            }
            else {
                useButton.gameObject.SetActive(false);
            }

            curItemLabel.text = _curItem + ":";
        }
    }

    // Функция, вызываемая подписчиком события щелчка мыши. 
    public void OnItem(string item) {
        _curItem = item;
        Refresh();
    }

    public void OnEquip() {
        Managers.Inventory.EquipItem(_curItem);
        Refresh();
    }

    public void OnUse() {
        Managers.Inventory.ConsumeItem(_curItem);
        if (_curItem == "health") {
            Managers.Player.ChangeHealth(25);
        }

        Refresh();
    }
}