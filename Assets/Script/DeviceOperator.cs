using UnityEngine;
using System.Collections;

public class DeviceOperator : MonoBehaviour {
    public float radius = 1.5f; // Расстояние, с которого персонаж может активировать устройства. 

    void Update() {
        if (Input.GetButtonDown("Fire3")) {
            // Реакция на кнопку ввода, заданную в настройках ввода в Unity. 

            // Метод OverlapSphere() возвращает список ближайших объектов.
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider hitCollider in hitColliders) {
                Vector3 direction = hitCollider.transform.position - transform.position;
                
                // Сообщение отправляется только при корректной ориентации персонажа.
                if (Vector3.Dot(transform.forward, direction) > .5f) {
                    // Метод SendMessage() пытается вызвать именованную функцию независимо от типа целевого объекта.
                    hitCollider.SendMessage("Operate", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}