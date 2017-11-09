using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rbody;
    public float player_Speed = 100.0f;
    private float inputH;
    private float inputV;
    private Vector3 move_direction;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        move_direction = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //Character Movement
        //getting input from controller left stick
        inputH = Input.GetAxis("LeftStickHorizontal");
        inputV = Input.GetAxis("LeftStickVertical");

        Vector2 LStickInput = new Vector2(inputH, inputV).normalized;

        //animator setting values
        anim.SetFloat("inputH", inputH);
        anim.SetFloat("inputV", inputV);
        //rbody movement
        float moveX = inputH * player_Speed * Time.deltaTime;
        float moveZ = inputV * -player_Speed * Time.deltaTime;
        rbody.velocity = new Vector3(moveX, 0.0f, moveZ);

        MoveDir(LStickInput);




       //testing
        if (Input.GetKey("w"))
        {
            anim.SetFloat("inputV", 1);
        }

        if (Input.GetKey("s"))
        {
            anim.SetFloat("inputV", -1);
        }

        if (Input.GetKey("a"))
        {
            anim.SetFloat("inputH", -1);
        }
        if (Input.GetKey("d"))
        {
            anim.SetFloat("inputH", 1);
        }
    }

    void MoveDir(Vector3 joystick_Direction)
    {
        float sign = (move_direction.x < inputH) ? -1.0f : 1.0f;
        float angle = Vector3.Angle(move_direction, joystick_Direction) * sign;

        float step = 60.0f * (Time.deltaTime * 60.0f);
        move_direction = Vector3.RotateTowards(move_direction, joystick_Direction, step, 0.0f);
        move_direction.Normalize();
    }
}