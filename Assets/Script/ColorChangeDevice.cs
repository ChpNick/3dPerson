using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ColorChangeDevice : BaseDevice {
    // Объявление метода с таким же именем, как в сценарии для двери
    public override void Operate() {
        Color random = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<Renderer>().material.color = random;
    }
}