using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Animator anim;
    public CollisionTreeManager ctm;
    private IntakeGenerator ig;
    //public Rigidbody rbody;
    public float player_Speed = 100.0f;
    private float inputH;
    private float inputV;
    private bool attacc;
    public int attaccPhase;
    private bool rollyPolly;
    //public float DegreesPerSecond = 60.0f;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
       // rbody = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void UpdateIG(IntakeGenerator _ig)
    {
        ig = _ig;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ig)
        {
            ig = ctm.weaponHand.transform.GetComponentInChildren<IntakeGenerator>();
        }

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
        Vector2 input = new Vector2(inputH, inputV);
        if (input.magnitude < 0.1f)
            inputH = inputV = 0;
        attacc = Input.GetButtonDown("Fire3");//get x button press
        rollyPolly = Input.GetButtonDown("Fire2"); // get b button press
        

        if(attacc)
        {
            if (attaccPhase < 3)
                attaccPhase++;
            else
                attaccPhase = 1;
        }

        //animator setting values
        anim.SetFloat("inputH", inputH);
        anim.SetFloat("inputV", inputV);
        anim.SetBool("attacc", attacc);
        anim.SetInteger("attaccPhase", attaccPhase);
        anim.SetBool("rollyPolly", rollyPolly);

        if (anim.GetCurrentAnimatorStateInfo(0).tagHash == 1080829965)
            ig.active = true;
        else
            ig.active = false;


        Vector3 desiredMoveDir = -forward * inputV + right * inputH;

        if(desiredMoveDir.magnitude >= 0.3f)
            transform.forward = desiredMoveDir.normalized;

        transform.position += (desiredMoveDir * player_Speed * Time.deltaTime);
        
        //rbody movement
        // rbody.velocity = new Vector3(inputH *player_Speed * Time.deltaTime, 0.0f, inputV * -player_Speed * Time.deltaTime);
        //rotate char
        //transform.Rotate(0, Input.GetAxis("RightJoystickHorizontal") * DegreesPerSecond, 0);
    }
}  