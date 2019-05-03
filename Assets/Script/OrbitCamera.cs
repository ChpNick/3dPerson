using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour {
    // Сериализованная ссылка на объект, вокруг которого производится облет.
    [SerializeField] private Transform target;

    public float rotSpeed = 1.5f;
    private float _rotY;
    private float _rotX;
    private Vector3 _offset;

    void Start() {
//        Сохранение начального смещения между камерой и целью.
        _rotY = transform.eulerAngles.y;
        _rotX = transform.eulerAngles.x;

        _offset = target.position - transform.position;
    }

    void LateUpdate() {
        FromKeyRotate(-1);


        // Поддерживаем начальное смещение, сдвигаемое в соответствии с поворотом
        Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
        transform.position = target.position - (rotation * _offset);

        // Камера всегда направлена на цель, где бы относительно этой цели она ни располагалась.
        transform.LookAt(target);
    }

    void FromKeyRotate(int direction = 1) {
        _rotY += Input.GetAxis("Horizontal") * rotSpeed * direction;
    }

    void FromMouseRotate() {
        _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
        _rotX += Input.GetAxis("Mouse Y") * rotSpeed * 3;
    }
}