using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour {
    // Сериализованная ссылка на объект, вокруг которого производится облет.
    [SerializeField] private Transform target;

    public float rotSpeed = 1.5f;
    private float _rotY;
    private Vector3 _offset;

    void Start() {
//        Сохранение начального смещения между камерой и целью.
        _rotY = transform.eulerAngles.y;
        _offset = target.position - transform.position;
    }

    void LateUpdate() {
        float horInput = Input.GetAxis("Horizontal");
        if (horInput != 0) {
            // Медленный поворот камеры при помощи клавиш со стрелками...
            _rotY += horInput * rotSpeed;
        }
        else {
            // или быстрый поворот с помощью мыши. 
            _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
        }
        // Поддерживаем начальное смещение, сдвигаемое в соответствии с поворотом
        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        transform.position = target.position - (rotation * _offset);
        
        // Камера всегда направлена на цель, где бы относительно этой цели она ни располагалась.
        transform.LookAt(target);
    }
}