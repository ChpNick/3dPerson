using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))] // Проверка на обязательное присутствие данного компонента
public class RelativeMovement : MonoBehaviour {
    private CharacterController _charController;
    private ControllerColliderHit _contact; // Нужно для сохранения данных о столкновении между функциями.

    private Animator _animator;

    // Сценарию нужна ссылка на объект, относительно которого будет происходить перемещение
    [SerializeField] private Transform target;
    public float rotSpeed = 15.0f;

    public float moveSpeed = 6.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    private float _vertSpeed;

    public float pushForce = 3.0f; // Величина прилагаемой силы.


    private void Start() {
//        Этот паттерн, знакомый вам по предыдущим главам, используется для доступа к другим компонентам.
        _charController = GetComponent<CharacterController>();

        _animator = GetComponent<Animator>();

        // Инициализируем скорость по вертикали, присваивая ей минимальную скорость падения в начале существующей функции.
        _vertSpeed = minFall;
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
        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);

        bool hitGround = false;
        RaycastHit hit;

        // Проверяем, падает ли персонаж. 
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) {
//            Расстояние, с которым производится сравнение (слегка выходит за нижнюю часть капсулы). 
            float check = (_charController.height + _charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }


        // Свойство isGrounded компонента CharacterController проверяет, соприкасается ли контроллер с поверхностью.
//        Вместо проверки свойства isGrounded смотрим на результат бросания луча.
        if (hitGround) {
            if (Input.GetButtonDown("Jump")) {
                // Реакция на кнопку Jump при нахождении на поверхности.
                _vertSpeed = jumpSpeed;
            }
            else {
                _vertSpeed = minFall;
                _animator.SetBool("Jumping", false);
            }
        }
        else {
            // Если персонаж не стоит на поверхности, применяем гравитацию, пока не будет достигнута предельная скорость.
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity) {
                _vertSpeed = terminalVelocity;
            }

            // Не вводите в действие это значение в самом начале уровня.
            if (_contact != null) {
                _animator.SetBool("Jumping", true);
            }


//        Метод бросания луча не обнаруживает поверхности, но капсула с ней соприкасается
            if (_charController.isGrounded) {
//            Реакция слегка меняется в зависи- мости от того, смотрит ли персонаж в сторону точки контакта.
                if (Vector3.Dot(movement, _contact.normal) < 0) {
                    movement = _contact.normal * moveSpeed;
                }
                else {
                    movement += _contact.normal * moveSpeed;
                }
            }
        }

        movement.y = _vertSpeed;
        movement *= Time.deltaTime;
        _charController.Move(movement);
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        _contact = hit;

        // Проверка, есть ли у участвующего в столкновении, объекта Rigitbody, обеспечивающий
        // реакцию на приложенную силу.
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body != null && !body.isKinematic) {
            // Назначение физическому телу скорости.
            body.velocity = hit.moveDirection * pushForce;
        }
    }
}