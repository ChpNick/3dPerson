using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//    Наследуем класс и реализуем интерфейс
public class PlayerManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    public int Health { get; private set; }
    public int maxHealth { get; private set; }

    public void Startup(NetworkService service) {
        Debug.Log("Player manager starting...");
//        Health = 50; // │ Эти значения могут быть инициализированы 
//        maxHealth = 100; // │ сохраненными данными.
        UpdateData(50, 100); // Вызываем метод обновления вместо того, чтобы задавать переменные напрямую.

        status = ManagerStatus.Started;
    }

    public void UpdateData(int health, int maxHealth) {
        this.Health = health;
        this.maxHealth = maxHealth;
    }

    public void ChangeHealth(int value) {
        // Другие сценарии не могут напрямую задавать переменную health, но могут вызывать эту функцию.
        Health += value;
        if (Health > maxHealth) {
            Health = maxHealth;
        }
        else if (Health < 0) {
            Health = 0;
        }

        Debug.Log("Health: " + Health + "/" + maxHealth);
        
        if (Health == 0) {
            Messenger.Broadcast(GameEvent.LEVEL_FAILED);
        }
        
        Messenger.Broadcast(GameEvent.HEALTH_UPDATED);
    }
    
    // Возвращаем игрока в исходное состояние.
    public void Respawn() {
        UpdateData(50, 100);
    }
}