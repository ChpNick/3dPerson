using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]  // Проверка на обязательное присутствие данного компонента
public class RelativeMovement : MonoBehaviour {
    
    public float moveSpeed = 6.0f;
    private CharacterController _charController;
    
    // Сценарию нужна ссылка на объект, относительно которого будет происходить перемещение
    [SerializeField] private Transform target;
    public float rotSpeed = 15.0f;

    private void Start() {
//        Этот паттерн, знакомый вам по предыдущим главам, используется для доступа к другим компонентам.
        _charController = GetComponent<CharacterController>(); 
    }

    void Update() {
        Vector3 movement = Vector3.zero; // Начинаем с вектора (0, 0, 0), непрерывно добавляя компоненты движения.

        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        

        // Движение обрабатывается только при нажатии клавиш со стрелками
        if (horInput != 0 || vertInput != 0) {
//            movement.x = horInput;
//            movement.z = vertInput;
            
            movement.x = horInput * moveSpeed;
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);
            
            
            // Сохраняем начальную ориентацию, чтобы вернуться к ней после завершения работы с целевым объектом.
            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            
            // Преобразуем направления движения из локальных в глобальные координаты
            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            // Метод LookRotation() вычисляет кватернион, смотрящий в этом направлении
//            transform.rotation = Quaternion.LookRotation(movement);
            Quaternion direction = Quaternion.LookRotation(movement);
                                                 // из какого на-я,   в какое,      с какой скоростью
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
            
            movement *= Time.deltaTime;
            _charController.Move(movement);
        }
    }
}