using UnityEngine;
using System.Collections;

public class DoorOpenDevice : MonoBehaviour {
    [SerializeField] private Vector3 dPos; // Смещение, применяемое при открывании двери.

    private bool _open; // Переменная типа Boolean для слежения за открытым состоянием двери.

    public void Operate() {
        if (_open) {
            // Открываем или закрываем дверь в зависимости от ее состояния.
            Vector3 pos = transform.position - dPos;
            transform.position = pos;
        }
        else {
            Vector3 pos = transform.position + dPos;
            transform.position = pos;
        }

        _open = !_open;
    }
}