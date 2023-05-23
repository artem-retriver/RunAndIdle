using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FixedJoystick joystick;
    private Animator anim;

    [SerializeField] private float speed;
    public bool isRun;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(joystick.Horizontal * speed, rb.velocity.y, joystick.Vertical * speed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            isRun = true;
            anim.Play("Running");
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
        else
        {
            isRun = false;
            anim.Play("Idle");
        }
    }

    public void UnMove()
    {
        rb.velocity = Vector3.zero;
    }
}
