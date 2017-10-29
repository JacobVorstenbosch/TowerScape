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
    Vector3 offset;
    Vector2 totalInput;
    float timeSinceInput;
    float correctedMagnitude;

    private bool m_overriden;

	// Use this for initialization
	void Start () {
        totalInput = new Vector3(0, 0, 0);
        offset = baseOffset;
        timeSinceInput = resetTime;
	}

    void LateUpdate()
    {
        if (!m_overriden)
        {
            Vector3 desiredPosition;
            Vector3 targetPos = target.transform.position + targetToCenter;

            Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (input.magnitude > 0f)
                timeSinceInput = 0f;
            //controlled rotation
            if (timeSinceInput <= resetTime)
            {
                timeSinceInput += Time.deltaTime;
                Vector3 rotation = new Vector3(target.transform.rotation.eulerAngles.x, target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.z);
                rotation += new Vector3(totalInput.y, totalInput.x, 0);

                desiredPosition = Vector3.RotateTowards(transform.position, Quaternion.Euler(rotation.x, rotation.y, rotation.z) * transform.position, 2, 0);
                if (desiredPosition.magnitude < correctedMagnitude)
                    desiredPosition *= 1 / (correctedMagnitude - desiredPosition.magnitude);
                if (correctedMagnitude < baseOffset.magnitude)
                    correctedMagnitude = Mathf.Lerp(correctedMagnitude, baseOffset.magnitude, Time.deltaTime / 3);
            }
            //standard movement
            else
            {
                totalInput = new Vector2(0, 0);
                desiredPosition = targetPos + target.transform.rotation * offset;
            }

            Vector3 position = Vector3.Slerp(transform.position, desiredPosition, Time.deltaTime * damping);
            transform.position = position;

            transform.LookAt(targetPos);
            totalInput += input;

            //collision correction
            RaycastHit wallHit = new RaycastHit();
            if (Physics.Linecast(targetPos, transform.position, out wallHit, 1 << wallLayer))
            {
                Vector3 direction = (transform.position - targetPos).normalized;
                transform.position = wallHit.point - direction * 0.01f;
                correctedMagnitude = (targetPos - transform.position).magnitude;
            }

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
