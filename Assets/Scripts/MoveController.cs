using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private int speed;
    [SerializeField] private int laneSpeed;
    [SerializeField] private float jumpLenght;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float slideLenght;

    private Animator anim;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private float currentLane = 1;
    private Vector3 verticalTargetPosition;
    private bool jumping = false;
    private float jumpStart;
    private bool sliding = false;
    private float slideStart;
    private Vector3 boxColliderSize;
    private Vector3 boxColliderCenter;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;
        boxColliderCenter = boxCollider.center;
    }

    public void InputHandler()
    {
        if (SwipeController.swipeLeft)
        {
            ChangeLane(-3f);
        }
        else if (SwipeController.swipeRight)
        {
            ChangeLane(3f);
        }
        else if (SwipeController.swipeUp)
        {
            Jump();
        }
        else if (SwipeController.swipeDown)
        {
            Slide();
        }
    }

    public void Movebale()
    {
        if (jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLenght;
            if (ratio >= 1f)
            {
                jumping = false;
                anim.SetTrigger("Running");
                //anim.SetBool("Jumping", false);
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        }
        else
        {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
        }

        if (sliding)
        {
            float ratio = (transform.position.z - slideStart) / slideLenght;
            if (ratio >= 2f)
            {

                sliding = false;
                anim.SetTrigger("Running");
                //anim.SetBool("Sliding", false);
                boxCollider.size = boxColliderSize;
                boxCollider.center = boxColliderCenter;
            }
        }

        Vector3 targetPosition = new(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);
    }

    public void Move()
    {
        rb.velocity = Vector3.forward * speed;
    }

    public void UnMove()
    {
        rb.velocity = Vector3.forward * 0;
    }

    private void ChangeLane(float direction)
    {
        float targetLane = currentLane + direction;

        if (targetLane < -4 || targetLane > 5)
            return;

        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 1), 0, 0);
    }

    private void Jump()
    {
        if (!jumping)
        {
            jumpStart = transform.position.z;
            //anim.SetFloat("JumpSpeed", speed / jumpLenght);
            anim.SetTrigger("Jumping");

            jumping = true;
        }
    }

    private void Slide()
    {
        if (!jumping && !sliding)
        {
            slideStart = transform.position.z;
            //anim.SetFloat("JumpSpeed", speed / slideLenght);
            //anim.SetBool("Sliding", true);
            anim.SetTrigger("Slide");
            Vector3 newSize = boxCollider.size;
            Vector3 newCenter = boxCollider.center;
            newSize.y /= 2;
            newCenter.y /= 2;
            boxCollider.size = newSize;
            boxCollider.center = newCenter;
            sliding = true;
        }
    }
}
