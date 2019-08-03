using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
    [SerializeField] private SettingsPopup s_popup; // Ссылки на всплывающее окно в сцене.

    [SerializeField] private Text healthLabel; // Ссылка на UI-объект в сцене. 
    [SerializeField] private InventoryPopup i_popup;

    [SerializeField] private Text levelEnding;

    void Awake() {
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.AddListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }

    void OnDestroy() {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.RemoveListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.RemoveListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }

    void Start() {
        OnHealthUpdated();

        // Инициализируем всплывающие окна в скрытом состоянии.
        levelEnding.gameObject.SetActive(false);
        s_popup.gameObject.SetActive(false);
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

    private void OnLevelComplete() {
        StartCoroutine(CompleteLevel());
    }

    private IEnumerator CompleteLevel() {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Complete!";

        // Отображаем сообщение в течение двух секунд, а потом переходим на следующий уровень.
        yield return new WaitForSeconds(2);

        levelEnding.gameObject.SetActive(false);
        Managers.Mission.GoToNext();
    }

    private void OnLevelFailed() {
        StartCoroutine(FailLevel());
    }

    private IEnumerator FailLevel() {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Failed!"; // Используем ту же самую текстовую метку, но с другим сообщением.
        
        yield return new WaitForSeconds(2);

        Managers.Player.Respawn();
        Managers.Mission.RestartCurrent(); // После двухсекундной паузы начинаем текущий уровень сначала. }
    }
    
    public void SaveGame() {
        Managers.Data.SaveGameState();
    }
    public void LoadGame() {
        Managers.Data.LoadGameState();
    }
    
    private void OnGameComplete() {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "You Finished the Game!";
    }
}