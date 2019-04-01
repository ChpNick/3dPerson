using UnityEngine;
using System.Collections;

public class DeviceTrigger : MonoBehaviour {
    //Список целевых объектов, которые будет активировать данный триггер.
    [SerializeField] private GameObject[] targets;

    // Метод OnTriggerEnter() вызывается при попадании foreach (GameObject target in targets) { объекта в зону триггера...
    void OnTriggerEnter(Collider other) {
        foreach (GameObject target in targets) {
            target.SendMessage("Activate");
        }
    }

//    в то время как метод OnTriggerExit() вызывается при выходе объекта из зоны триггера.
    void OnTriggerExit(Collider other) {
        foreach (GameObject target in targets) {
            target.SendMessage("Deactivate");
        }
    }
}