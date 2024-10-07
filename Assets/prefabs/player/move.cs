using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private float speed = 4;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

    [SerializeField] private Camera cam;

    private Vector3 lastPosition;

    private bool first = true;


    public void Start()
    {
        lastPosition = transform.position;
        speed *= (1 + (float)gm.i.upgrades.speed.lvl / 10);
    }

    public void FixedUpdate()
    {
        if(!gm.i.gc.gameOver)
        {
            if(joystick.Horizontal == 0 && joystick.Vertical == 0)
            {
                rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, .5f), rb.velocity.y, Mathf.Lerp(rb.velocity.z, 0, .2f));
            }
            else
            {
                rb.velocity = new Vector3(joystick.Horizontal * speed, rb.velocity.y, joystick.Vertical * speed);
            }     

            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
            }
        }
        else
            rb.velocity = new Vector3(0f, 0f, 0f);
        

    }

    public void Update()
    {
        
        if (animator != null && Time.deltaTime > 0.0f)
        {
            float distanceTravelledSinceLastFrame = (transform.position - lastPosition).magnitude;
            float distancePerSecond = distanceTravelledSinceLastFrame / Time.deltaTime;

            animator.SetFloat("Speed", distancePerSecond);
        }

        lastPosition = transform.position;
    }
}
