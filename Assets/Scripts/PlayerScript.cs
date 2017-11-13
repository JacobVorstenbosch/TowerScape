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
    Vector3 moveDir;
    public float DegreesPerSecond = 60.0f;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var camera = Camera.main;
        var forward = camera.transform.forward;
        var right = camera.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //Character Movement
        //getting input from controller left stick
        inputH = Input.GetAxis("LeftStickHorizontal");
        inputV = Input.GetAxis("LeftStickVertical");
        // = Input.GetButtonDown("Fire3");//get x button press
        //animator setting values
        anim.SetFloat("inputH", inputH);
        anim.SetFloat("inputV", inputV);
        //rbody movement
        // moveDir = new Vector3(inputH * player_Speed * Time.deltaTime, 0, inputV * -player_Speed * Time.deltaTime);
        var desiredMoveDir = -forward * inputV + right * inputH;
        transform.Translate(desiredMoveDir * player_Speed * Time.deltaTime);
       // rbody.velocity = new Vector3(inputH *player_Speed * Time.deltaTime, 0.0f, inputV * -player_Speed * Time.deltaTime);
        //rotate char
       //transform.Rotate(0, Input.GetAxis("LeftStickHorizontal") * DegreesPerSecond * Time.deltaTime, 0);

    }

}  