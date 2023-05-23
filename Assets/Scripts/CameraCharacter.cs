using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCharacter : MonoBehaviour
{
    public Transform player;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void FixedUpdate()
    {
        Vector3 _newPosition = new(offset.x + player.position.x, transform.position.y, offset.z + player.position.z);
        transform.position = _newPosition;
    }
}
