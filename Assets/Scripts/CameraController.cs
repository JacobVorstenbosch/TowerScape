using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target;
    public Vector3 targetToCenter;
    public float damping = 1;
    public Vector3 baseOffset;
    public float resetTime = 3;
    public int wallLayer;
    public Vector2 minmaxVerticalRotation;

    Vector3 offset;
    float timeSinceInput;
    float correctedMagnitude;
    Vector2 totalInput;

    private bool m_overriden;

	// Use this for initialization
	void Start () {
        totalInput = new Vector2(0,0);
        offset = baseOffset + targetToCenter;
        timeSinceInput = resetTime;
	}

    void LateUpdate()
    {
        if (!m_overriden)
        {
            Vector3 targetPos = target.transform.position + targetToCenter;

            Vector2 input = new Vector2(Input.GetAxis("RightJoystickHorizontal"), Input.GetAxis("RightJoystickVertical"));
            if (input.magnitude >= 0.1f)
                timeSinceInput = 0f;
            //controlled rotation
            if (timeSinceInput <= resetTime)
            {
                //float verticalRot = 0;
                //if ((transform.rotation.eulerAngles.x > minmaxVerticalRotation.x && input.y < 0) || (transform.rotation.eulerAngles.x < minmaxVerticalRotation.y && input.y > 0))
                //    verticalRot = input.y;
                //
                //print(verticalRot);
                //Quaternion rotation = Quaternion.Euler(verticalRot, input.x, 0);
                //transform.position = target.transform.position + (rotation * transform.position);
                //
                //transform.LookAt(target.transform);
            }
            //standard movement
            else
            {
                totalInput = Vector2.zero;
                transform.position = Vector3.Slerp(transform.position, target.transform.rotation * offset, Time.deltaTime * damping);
                transform.LookAt(target.transform);
            }

            timeSinceInput += Time.deltaTime;

            //collision correction
            //RaycastHit wallHit = new RaycastHit();
            //if (Physics.Linecast(targetPos, transform.position, out wallHit, 1 << wallLayer))
            //{
            //    Vector3 direction = (transform.position - targetPos).normalized;
            //    transform.position = wallHit.point - direction * 0.01f;
            //    correctedMagnitude = (targetPos - transform.position).magnitude;
            //}
            //
            //return;
        }

        //Do overriden stuff
    }

    public void Override(/*Path movement, Path lookat, float duration*/)
    {
        //TODO
        m_overriden = true;
    }
}
