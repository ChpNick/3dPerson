using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
    [SerializeField] private SettingsPopup s_popup; // Ссылки на всплывающее окно в сцене.

    [SerializeField] private Text healthLabel; // Ссылка на UI-объект в сцене. 
    [SerializeField] private InventoryPopup i_popup;

    void Awake() {
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }

    void OnDestroy() {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }

    void Start() {
        s_popup.gameObject.SetActive(false); // Инициализируем всплывающее окно в скрытом состоянии.

        OnHealthUpdated();
        i_popup.gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            bool isShowingInventory = i_popup.gameObject.activeSelf;
            i_popup.gameObject.SetActive(!isShowingInventory);
            i_popup.Refresh();
        }

        // Вызываем и скрываем всплывающее окно при помощи клавиши M.
        if (Input.GetKeyDown(KeyCode.S)) {
            bool isShowing = s_popup.gameObject.activeSelf;
            s_popup.gameObject.SetActive(!isShowing);
        }
    }

    private void OnHealthUpdated() {
        string message = "Health: " + Managers.Player.Health + "/" + Managers.Player.maxHealth;
        healthLabel.text = message;
    }
}