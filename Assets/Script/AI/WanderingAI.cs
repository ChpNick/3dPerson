using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour {
    public float speed = 3.0f;
    public const float baseSpeed = 3.0f;

    public float obstacleRange = 1f;

    private bool _alive;
    

    [SerializeField] private GameObject fireballPrefab;
    private GameObject _fireball;

    void Awake() {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    void OnDestroy() {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    // Метод, объявленный в подписчике для события SPEED_CHANGED.
    private void OnSpeedChanged(float value) {
        speed = baseSpeed * value;
    }

    private void Start() {
        _alive = true;
    }


    void Update() {
        if (_alive) {
            Ray ray = new Ray(transform.position, transform.forward);

            RaycastHit hit;

            if (Physics.SphereCast(ray, 0.75f, out hit)) {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<PlayerCharacter>()) {
                    if (_fireball == null) {
                        _fireball = Instantiate(fireballPrefab) as GameObject;

                        _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                        _fireball.transform.rotation = transform.rotation;
                    }
                }
                else if (hit.distance < obstacleRange) {
                    float angle = Random.Range(-110, 110);
                    transform.Rotate(0, angle, 0);
                    return;
                }

                transform.Translate(0, 0, speed * Time.deltaTime);
            }
        }
    }

    public void SetAlive(bool alive) {
        _alive = alive;
    }
}