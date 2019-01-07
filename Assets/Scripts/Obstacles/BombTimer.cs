using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTimer : MonoBehaviour {

    Vector3 position;
    Quaternion rotation;
    void Awake()
    {
        rotation = transform.rotation;
    }
    void LateUpdate()
    {
        transform.rotation = rotation;
        position = transform.parent.position;
        position.y = transform.parent.position.y + 1.5f;
        transform.position = position;
    }
}
