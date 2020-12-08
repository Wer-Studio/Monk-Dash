using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolearmControllerV1 : MonoBehaviour
{
    //Public properties
    [Header("Required Components")]
    public RelativeJoint2D armJoint;
    public DistanceJoint2D armLimit;
    public HingeJoint2D handLimit;
    public BoxCollider2D polearmCollider;
    public Rigidbody2D handle;

    [Header("Mouse Settings")]
    public float sensitivity = 10f;
    public float smoothing = 4f;

    [Header("Joint Settings")]
    public float maxArmActiveForce;
    public float maxArmRestingForce;
    public float maxTargetVelocity;
    public float maxArmLength;

    [Header("Control Settings")]
    public float targetRotationOffset = 0.5f;
    public float maxTargetOffset = 0.5f;
    //public float handAngleOffset = 0;
    //.......................................

    //Private States
    Vector2 targetPoint;
    //.............................................

    //Debug States
    float maxTargetOffsetDebug = 0;
    float maxTargetProximityDebug = 0;

    //Private Utility Objects
    private MouseAxisProcessor mAxis = new MouseAxisProcessor();
    //.........................................................

    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mAxis.mouseSensitivity = sensitivity;
        mAxis.smoothing = smoothing;
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), polearmCollider);
        targetPoint = GetTargetPivot();
    }

    // Update is called once per frame
    void Update()
    {
        mAxis.PollInput();
    }

    void FixedUpdate() 
    {
        //Correct internal arm state to match real state
        //Calculate New Target Vector
        Vector2 mDelta = mAxis.ReturnInput();
        mDelta = Vector2.ClampMagnitude(mDelta, 1f);
        Vector2 deltaTarget = mDelta * maxTargetVelocity * Time.fixedDeltaTime;
        Vector2 newTargetPoint = targetPoint + deltaTarget;
        newTargetPoint = ClampTargetPoint(newTargetPoint);


        Debug.DrawRay(transform.position, newTargetPoint, Color.yellow, Time.fixedDeltaTime);
        //Debug.DrawRay(transform.position-Vector3.down, armDir, Color.red, Time.fixedDeltaTime);
        //Debug.DrawRay(transform.position-Vector3.down*2f, armDir*armExtension, Color.blue, Time.fixedDeltaTime);

        //Calculate Arm Pos increment and Angle increment
        float deltaAngle = Vector2.SignedAngle(targetPoint, newTargetPoint);

        //Debug.Log("Delta Angle: " + deltaAngle + "Delta Extension: " + deltaExtension);
        //Update Pos and Angle
        armJoint.linearOffset = GetTargetArmVector(newTargetPoint);
        armJoint.angularOffset += deltaAngle;

        //Calculate and Set Force
        float magnitude = mDelta.magnitude;
        float force = Mathf.Lerp(maxArmRestingForce, maxArmActiveForce, magnitude);
        float targetProximity = (newTargetPoint - GetTargetPivot()).magnitude/maxTargetOffset;
        maxTargetProximityDebug = Mathf.Max(maxTargetProximityDebug, targetProximity);
        armJoint.maxForce = force;
        armJoint.maxTorque = force;
        armJoint.correctionScale = Mathf.Lerp(0.2f, 0.6f, targetProximity*targetProximity);
        targetPoint = newTargetPoint;
        Debug.Log("Max Target Proximity: " + maxTargetProximityDebug);
    }

    void OnValidate() {
        mAxis.mouseSensitivity = sensitivity;
        mAxis.smoothing = smoothing;
        armLimit.distance = maxArmLength;
    }

    private Vector2 GetTargetPivot() {
        return handle.transform.position + handle.transform.right.normalized * (maxArmLength + targetRotationOffset);
    }

    private Vector2 ClampTargetPoint(Vector2 point) {
        float mag = point.magnitude;
        mag = Mathf.Clamp(mag, targetRotationOffset, maxArmLength * 2 + targetRotationOffset);
        point = point.normalized * mag;

        Vector2 pivot = handle.transform.position + handle.transform.right.normalized * (maxArmLength + targetRotationOffset);
        Vector2 offset = point - pivot;
        if (offset.magnitude > maxTargetOffsetDebug) maxTargetOffsetDebug = offset.magnitude;
        offset = Vector2.ClampMagnitude(offset, maxTargetOffset);
        return pivot + offset;
    }

    private Vector2 GetTargetArmVector(Vector2 targetPoint) {
        float magnitude = targetPoint.magnitude;
        return targetPoint.normalized * (magnitude - maxArmLength - targetRotationOffset);
    }
}
