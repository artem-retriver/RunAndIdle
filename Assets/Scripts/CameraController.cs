using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform character;
    public Camera camera;
    public bool isIdleCamera;

    private Vector3 offSet;

    private void Start()
    {
        camera = GetComponent<Camera>();
        offSet = transform.position - character.position;
    }

    private void FixedUpdate()
    {
        if (isIdleCamera == false)
        {
            Vector3 newPos = new(transform.position.x, transform.position.y, offSet.z + character.position.z);
            transform.position = newPos;
        }
        else
        {
            camera.orthographic = true;
            camera.orthographicSize = 8;
            Vector3 newPos = new(offSet.x + character.position.x + 1, character.position.y + 7.73f, offSet.z + character.position.z + 0.7183f);
            //Vector3 newPos = new(offSet.x + character.position.x + 2.3f, offSet.y + character.position.y + 6.85f, offSet.z + character.position.z - 7.1813f);
            transform.position = newPos;
        }

    }
}
