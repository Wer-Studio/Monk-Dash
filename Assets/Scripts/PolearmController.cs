using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolearmController : MonoBehaviour
{

    public RelativeJoint2D polearmJoint;

    public float maxDistance = 2;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //polearmJoint.linearOffset = targetVector = targetVector.normalized* maxDistance;
        //Debug.Log(targetVector);
        polearmJoint.linearOffset = Vector2.ClampMagnitude(targetVector, maxDistance);
        float targetAngle = Vector2.SignedAngle(Vector2.right, targetVector);
        if (targetAngle < 0) targetAngle += 360;
        float currentAngle = polearmJoint.angularOffset % 360;
        if (currentAngle < 0) currentAngle += 360;
        float offset = targetAngle - currentAngle;
        if (offset > 180) offset -= 360;
        if (offset < -180) offset += 360;
        Debug.Log(currentAngle);
        Debug.Log(offset);
        polearmJoint.angularOffset += offset;
    }
}
