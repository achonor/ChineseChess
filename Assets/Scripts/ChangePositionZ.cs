using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePositionZ : MonoBehaviour
{
    public float PositionZ;

    private void Awake() {
        PositionZ = transform.position.z;
    }

    private void FixedUpdate() {
        if (PositionZ != transform.position.z) {
            transform.position = new Vector3(transform.position.x, transform.position.y, PositionZ);
        }
    }
}
