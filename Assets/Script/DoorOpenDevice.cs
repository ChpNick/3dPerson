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

    public void Activate() {
        // Открывает дверь, только если она пока не открыта. 
        if (!_open) {
            Vector3 pos = transform.position + dPos;
            transform.position = pos;
            _open = true;
        }
    }

    public void Deactivate() {
//        Аналогично, закрывает дверь только при условии, что она уже не закрыта.
        if (_open) {
            Vector3 pos = transform.position - dPos;
            transform.position = pos;
            _open = false;
        }
    }
}