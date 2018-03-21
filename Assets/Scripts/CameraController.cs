using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target;
    public Vector3 targetToCenter;
    public float damping = 0.5f;
    public Vector3 baseOffset;
    public float resetTime = 3;
    public float controllerSensitivity = 3;
    public int wallLayer;
    public Vector2 minmaxVerticalRotation;

    Vector3 offset;
    float timeSinceInput;
    float dampingsqrd;

    private bool m_overriden;

	// Use this for initialization
	void Start () {
        offset = baseOffset + targetToCenter;
        timeSinceInput = resetTime;
        dampingsqrd = damping * damping;
        transform.position = target.transform.position + offset;
	}

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void ResetPosition()
    {
        timeSinceInput = resetTime;
        transform.position = target.transform.position + offset;
        transform.LookAt(target.transform);
    }

    void LateUpdate()
    {
        if (!m_overriden)
        {
            Vector3 targetPos = target.transform.position + targetToCenter;

            Vector2 input = new Vector2(Input.GetAxis("RightJoystickHorizontal"), Input.GetAxis("RightJoystickVertical")) * controllerSensitivity;
            //input *= Mathf.Deg2Rad;
            if (input.magnitude >= 0.1f)
                timeSinceInput = 0f;
            //controlled rotation
            if (true || timeSinceInput <= resetTime && input.magnitude > 0.1f)
            {
                float verticalRot = 0;
                if ((transform.rotation.eulerAngles.x > minmaxVerticalRotation.x && input.y < 0) || (transform.rotation.eulerAngles.x < minmaxVerticalRotation.y && input.y > 0))
                    verticalRot = input.y;

                transform.RotateAround(targetPos, Vector3.up, input.x);

                if (transform.rotation.eulerAngles.x > minmaxVerticalRotation.y)
                    transform.RotateAround(targetPos, transform.right, minmaxVerticalRotation.y - transform.rotation.eulerAngles.x - 1);
                else if (transform.rotation.eulerAngles.x < minmaxVerticalRotation.x)
                    transform.RotateAround(targetPos, transform.right, minmaxVerticalRotation.x - transform.rotation.eulerAngles.x + 1);
                else
                    transform.RotateAround(targetPos, transform.right, verticalRot);


                transform.LookAt(targetPos);
            }
            //standard movement
            else
            {
                transform.position = target.transform.position + target.transform.rotation * offset;//Vector3.Slerp(transform.position, target.transform.position + target.transform.rotation * offset, Time.deltaTime * damping);
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
              transform.position = uncorrectedPos;//transform.position = Vector3.Lerp(transform.position, uncorrectedPos, 1/*Time.deltaTime * dampingsqrd*/);
            

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
