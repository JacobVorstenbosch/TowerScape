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
    Vector2 totalInput;
    float timeSinceInput;
    float dampingsqrd;

    private bool m_overriden;

	// Use this for initialization
	void Start () {
        totalInput = new Vector2(0,0);
        offset = baseOffset + targetToCenter;
        timeSinceInput = resetTime;
        dampingsqrd = damping * damping;
	}

    void LateUpdate()
    {
        if (!m_overriden)
        {
            Vector3 targetPos = target.transform.position + targetToCenter;

            Vector2 input = new Vector2(Input.GetAxis("RightJoystickHorizontal"), Input.GetAxis("RightJoystickVertical"));
            //input *= Mathf.Deg2Rad;
            if (input.magnitude >= 0.1f)
                timeSinceInput = 0f;
            //controlled rotation
            if (timeSinceInput <= resetTime)
            {
                float verticalRot = 0; 
                if ((transform.rotation.eulerAngles.x > minmaxVerticalRotation.x && input.y < 0) || (transform.rotation.eulerAngles.x < minmaxVerticalRotation.y && input.y > 0)) 
                    verticalRot = input.y;
                transform.RotateAround(targetPos, Vector3.up, input.x);
                transform.RotateAround(targetPos, transform.right, verticalRot);
                transform.LookAt(targetPos);
            }
            //standard movement
            else
            {
                totalInput = Vector2.zero;
                transform.position = Vector3.Slerp(transform.position, target.transform.rotation * offset, Time.deltaTime * damping);
                transform.LookAt(targetPos);
            }

            timeSinceInput += Time.deltaTime;

            //collision correction
            RaycastHit wallHit = new RaycastHit();
            Vector3 direction = (transform.position - targetPos).normalized;
            Vector3 uncorrectedPos = targetPos + direction * offset.magnitude;
            if (Physics.Linecast(targetPos, uncorrectedPos, out wallHit, 1 << wallLayer))
                transform.position = wallHit.point - direction;
            else
                transform.position = Vector3.Lerp(transform.position, uncorrectedPos, Time.deltaTime * dampingsqrd);
            
            return;
        }

        //Do overriden stuff
    }

    public void Override(/*Path movement, Path lookat, float duration*/)
    {
        //TODO
        m_overriden = true;
    }
}
