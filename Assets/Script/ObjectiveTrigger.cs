using UnityEngine;
using System.Collections;

public class ObjectiveTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        Managers.Mission.ReachObjective(); // Вызываем новый целевой метод в сценарии MissionManager. 
    }
}