using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    private float normalSpeed;
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVel;
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        normalSpeed = speed;
    }
    void Update(){
        Movement();
    }

    private void Movement(){
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }


        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, 0, z).normalized;

        if(dir.magnitude >= 0.1f){

            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        Jump();

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(Input.GetButtonDown("Fire3")){
            speed = speed * 2;
        }else if(Input.GetButtonUp("Fire3")){
            speed = normalSpeed;
        }
    }

    private void Jump(){
        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
