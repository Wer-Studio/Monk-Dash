using UnityEngine;
using System;

public class PolearmController : MonoBehaviour
{
    //Public Properties
    [Header("Required Components")]
    public HingeJoint2D armHingeJoint;
    public SliderJoint2D armSliderJoint;
    public Collider2D polearmCollider;

    [Header("Mouse Settings")]
    public float sensitivity = 10f;
    public float smoothing = 4f;

    [Header("Joint Settings")]
    public float maxLinearForce;
    public float minLinearForce;
    public float maxAngularForce;
    public float minAngularForce;
    public float maxArmLength;

    [Header("Control Settings")]
    public float armSpeed;
    public float minRotationRadius;
    public float maxRotationRadius;
    public float rotationCorrection = 0;
    //..................................

    //Private States
    private JointMotor2D armHinge = new JointMotor2D();
    private JointMotor2D armSlider = new JointMotor2D();

    //Private Utility Objects
    private MouseAxisProcessor mAxis = new MouseAxisProcessor();
    //.........................................................

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        armHingeJoint.motor = armHinge;
        armSliderJoint.motor = armSlider;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), polearmCollider);
    }

    // Update is called once per frame
    void Update()
    {
        mAxis.PollInput();
    }

    void FixedUpdate() 
    {
        //Get movement delta
        Vector2 mDelta = mAxis.ReturnInput();
        mDelta = Vector2.ClampMagnitude(mDelta, 1f)*armSpeed;

        HackMovement(mDelta);

        //MoveArmHinge(mDelta);
        //MoveArmSlider(mDelta);
    }

    private void HackMovement(Vector2 mDelta) {
        //Get arm state
        float armAngle = -armHingeJoint.jointAngle;
        Vector2 armDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * armAngle), Mathf.Sin(Mathf.Deg2Rad * armAngle));
        Vector2 armTangent = new Vector2(-armDir.y, armDir.x);

        //Get delta components
        float deltaCos = -Vector2.Dot(armTangent, mDelta);
        float deltaSin = Vector2.Dot(armDir, mDelta);
        Vector2 deltaC = new Vector2(deltaCos, deltaSin);
        if(deltaC != Vector2.zero && rotationCorrection > 0) {
            float m = deltaC.magnitude;
            float cos = deltaC.x / m;
            cos = (float)(Math.Tanh(cos*rotationCorrection) / Math.Tanh(rotationCorrection));
            float tsin = Mathf.Sqrt(1 - cos * cos);
            float sin = deltaC.y > 0 ? tsin : -tsin;
            deltaC.x = m * cos;
            deltaC.y = m * sin;
            //Debug.Log(deltaC.y);
        }
        //Slighty transform cosine to aid in rotation.
        

        //Calculate rotation movement
        float r = (armSliderJoint.jointTranslation + maxArmLength) / maxArmLength;
        float rotationRadius = Mathf.Lerp(minRotationRadius, maxRotationRadius, r);
        float rotationDelta = deltaC.x / rotationRadius;

        //Calculate extension movement
        //float extensionDelta = Vector2.Dot(armDir, mDelta);
        float extensionDelta = deltaC.y;
        //Debug.Log(extensionDelta);

        //Apply values to joints
        armHinge.motorSpeed = Mathf.Rad2Deg * rotationDelta;
        armHinge.maxMotorTorque = Mathf.Lerp(minAngularForce, maxAngularForce, mDelta.magnitude/armSpeed);
        armHingeJoint.motor = armHinge;
        
        armSlider.motorSpeed = extensionDelta;
        armSlider.maxMotorTorque = Mathf.Lerp(minLinearForce, maxLinearForce, mDelta.magnitude);
        armSliderJoint.motor = armSlider;
        Debug.Log((mDelta.magnitude/armSpeed).ToString("F2"));
    }

    private void MoveArmHinge(Vector2 mDelta) {
        Vector2 rotationDelta = mDelta;
        float armAngle = -armHingeJoint.jointAngle;
        Vector2 armDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * armAngle), Mathf.Sin(Mathf.Deg2Rad * armAngle));
        Vector2 armTangent = new Vector2(-armDir.y, armDir.x);
        float r = (armSliderJoint.jointTranslation + maxArmLength) / maxArmLength;
        float rotationRadius = Mathf.Lerp(minRotationRadius, maxRotationRadius, r);
        float deltaTangent = -Vector2.Dot(armTangent, rotationDelta)/rotationRadius;
        //Debug.DrawRay(transform.position, armTangent * rotationDelta, Color.red, Time.fixedDeltaTime);
        //Debug.DrawRay(transform.position, armDir*rotationRadius, Color.yellow, Time.fixedDeltaTime);
        armHinge.motorSpeed = Mathf.Rad2Deg*deltaTangent;
        armHinge.maxMotorTorque = maxAngularForce;
        armHingeJoint.motor = armHinge;
    }

    private void MoveArmSlider(Vector2 mDelta) {
        Vector2 extensionDelta = mDelta;
        float armAngle = -armHingeJoint.jointAngle;
        Vector2 armDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * armAngle), Mathf.Sin(Mathf.Deg2Rad * armAngle));
        float deltaNormal = Vector2.Dot(armDir, extensionDelta);
        armSlider.motorSpeed = deltaNormal;
        armSlider.maxMotorTorque = maxLinearForce;
        armSliderJoint.motor = armSlider;
    }

    void OnValidate() {
        //Update mouse sensitivity
        mAxis.mouseSensitivity = sensitivity;
        mAxis.smoothing = smoothing;
        
        //Update Joint paramaters
        JointTranslationLimits2D l = new JointTranslationLimits2D();
        l.min = -maxArmLength;
        l.max = maxArmLength;
        armHinge.maxMotorTorque = maxAngularForce;
        armSlider.maxMotorTorque = maxLinearForce;

    }
}
