using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
    [SerializeField] private SettingsPopup popup; // Ссылки на всплывающее окно в сцене.

    void Start() {
        popup.gameObject.SetActive(false); // Инициализируем всплывающее окно в скрытом состоянии.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
       
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        // Вызываем и скрываем всплывающее окно при помощи клавиши M.
        if (!Input.GetKeyDown(KeyCode.M)) return;
        
        bool isShowing = popup.gameObject.activeSelf;
        popup.gameObject.SetActive(!isShowing);

        if (isShowing) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}