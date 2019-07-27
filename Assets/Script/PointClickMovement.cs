using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))] // Проверка на обязательное присутствие данного компонента
public class PointClickMovement : MonoBehaviour {
    private CharacterController _charController;
    private ControllerColliderHit _contact; // Нужно для сохранения данных о столкновении между функциями.

    private Animator _animator;

    // Сценарию нужна ссылка на объект, относительно которого будет происходить перемещение
//    [SerializeField] private Transform target;
    public float rotSpeed = 15.0f;

    public float moveSpeed = 6.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    private float _vertSpeed;

    public float pushForce = 3.0f;             // Величина прилагаемой силы.

    public float deceleration = 20.0f;         // показатель снижения скорости
    public float targetBuffer = 1.5f;          // минимальное расстояние до целевой точки
    private float _curSpeed = 0f;              // скорость во время движения к точке, изменяемая
    private Vector3 _targetPos = Vector3.one;  // позиция целевой точки


    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip JumpSound;

    private void Start() {
//        Этот паттерн, знакомый вам по предыдущим главам, используется для доступа к другим компонентам.
        _charController = GetComponent<CharacterController>();

        _animator = GetComponent<Animator>();

        // Инициализируем скорость по вертикали, присваивая ей минимальную скорость падения в начале существующей функции.
        _vertSpeed = minFall;
    }

    void Update() {
        Vector3 movement = Vector3.zero; // Начинаем с вектора (0, 0, 0), непрерывно добавляя компоненты движения.

        // Задаем целевую точку по щелчку мыши.
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Испускаем луч в точку щелчка мышью. 
            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit)) {
                GameObject hitObject = mouseHit.transform.gameObject;
                
                if (hitObject.layer == LayerMask.NameToLayer("Ground")) {
                    _targetPos = mouseHit.point; // Устанавливаем цель в точке попадания луча.
                    _curSpeed = moveSpeed;
                }
            }
        }

        // Перемещаем при заданной целевой точке. 
        if (_targetPos != Vector3.one) {
            // создаем вектор, который указывает позицию щелчка, затем создаем кватернион поворота
            // путем вычитания из вектора куда нам надо повернуться и вектора тек. положения
            Vector3 adjustedPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
            Quaternion targetRot = Quaternion.LookRotation(adjustedPos - transform.position);

            // Поворачиваем по направлению к цели.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

            movement = _curSpeed * Vector3.forward;
            movement = transform.TransformDirection(movement);

            if (Vector3.Distance(_targetPos, transform.position) < targetBuffer) {
                _targetPos = Vector3.one;
//                _curSpeed -= deceleration * Time.deltaTime; // Снижаем скорость до нуля при приближении к цели.
//                if (_curSpeed <= 0) {
//                    _targetPos = Vector3.one;
//                }
            }
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

                soundSource.PlayOneShot(JumpSound);
//                soundSource.clip=JumpSound; soundSource.Play();
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