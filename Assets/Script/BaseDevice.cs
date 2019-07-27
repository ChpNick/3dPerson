using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDevice : MonoBehaviour {
    public float radius = 3.5f;


    // Функция, запускаемая щелчком.
    void OnMouseDown() {
        Transform player = GameObject.FindWithTag("Player").transform;

        if (Vector3.Distance(player.position, transform.position) < radius) {
            Vector3 direction = transform.position - player.position;
            if (Vector3.Dot(player.forward, direction) > .5f) {
                Operate(); // Вызов метода Operate(), если персонаж находится рядом и повернут лицом к устройству. 
            }
        }
    }

    // Ключевое слово virtual указывает на метод, который  можно переопределить после наследования.
    public virtual void Operate() {
        // поведение конкретного устройства
    }
}
