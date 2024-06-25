using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

    [SerializeField] private Camera cam;

    private Vector3 lastPosition;

    public void Start()
    {
        lastPosition = transform.position;
    }

    public void FixedUpdate()
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

    public void Update()
    {
        cam.transform.position = new Vector3(gameObject.transform.position.x, cam.transform.position.y, gameObject.transform.position.z - 6.9f);

        if (animator != null && Time.deltaTime > 0.0f)
        {
            float distanceTravelledSinceLastFrame = (transform.position - lastPosition).magnitude;
            float distancePerSecond = distanceTravelledSinceLastFrame / Time.deltaTime;

            animator.SetFloat("Speed", distancePerSecond);
        }

        lastPosition = transform.position;
    }
}
