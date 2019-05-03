using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;
    private GameObject _enemy;
    private float _enemy_speed;

    void Awake() {
        _enemy_speed = WanderingAI.baseSpeed;
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }
    void OnDestroy() {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }
    private void OnSpeedChanged(float value) {
        _enemy_speed = WanderingAI.baseSpeed * value;
    }

    void Update() {
        if (_enemy == null) {
            _enemy = Instantiate(enemyPrefab);
            _enemy.transform.position = new Vector3(0, 1, 0);

            WanderingAI wanderingAi = _enemy.GetComponent<WanderingAI>();
            wanderingAi.speed = _enemy_speed;
            
            float angle = Random.Range(0, 360);
            _enemy.transform.Rotate(0, angle, 0);
        }
    }
}