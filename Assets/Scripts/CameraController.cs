using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform character;

    private Vector3 offSet;

    private void Start()
    {
        offSet = transform.position - character.position;
    }

    private void FixedUpdate()
    {
        Vector3 newPos = new(transform.position.x, transform.position.y, offSet.z + character.position.z);
        transform.position = newPos;
    }
}
