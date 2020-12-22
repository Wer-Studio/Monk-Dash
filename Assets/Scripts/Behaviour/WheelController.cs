using UnityEngine;

public class WheelController : MonoBehaviour
{
    //Public Properties
    [Header("Required Components")]
    public HingeJoint2D legHingeJoint;
    public SliderJoint2D legSliderJoint;

    [Header("Vertical Movement")]
    public float maxExtensionForce;
    public float extensionJumpSpeed;         //Speed when jumping.
    public float extensionMoveSpeed;    //Speed when extending.
    public float restingExtensionSelector;  //0 CROUCHED | 0.5 UPRIGHT (DEFAULT) | 1 EXTENDED (MAX JUMP POS).

    [Header("Horizontal Movement")]
    public float maxTranslationForce;
    public float translationMoveSpeed;

    //..................................

    //Private States
    private JointMotor2D legHinge = new JointMotor2D();
    private JointMotor2D legSlider = new JointMotor2D();
    //.........................................................

    //Private Utility Objects
    KeyProcessor kp = new KeyProcessor();
    //................................


    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        //Get input
        int hAction = kp.HorizontalAction;
        int vAction = kp.VerticalAction;
        int jumpAction = kp.JumpAction;

        float extensionSelector = restingExtensionSelector;
        float extensionSpeed = extensionMoveSpeed;
        float translationVelocity = hAction * translationMoveSpeed;

        if(vAction == -1) {
            extensionSelector = 0;
        }else if(vAction == 1) {
            extensionSelector = 1;
        }

        if(jumpAction == 1) {
            extensionSelector = 1;
            extensionSpeed = extensionJumpSpeed;
        }

        MoveLegHinge(translationVelocity);
        MoveLegSlider(extensionSelector, extensionSpeed);
    }

    private void MoveLegHinge(float translationVelocity)
    {
        Debug.Log("Translation: " + translationVelocity);
        legHinge.maxMotorTorque = maxTranslationForce;
        legHinge.motorSpeed = - Mathf.Rad2Deg * translationVelocity;
        legHingeJoint.motor = legHinge;
    }

    // This function decides the position of the vertical slider.
    // Semantically it decides how crouched or extended the monk's back is (so jumping has more inertia and looks more real)
    private void MoveLegSlider(float extensionSelector, float maxExtensionSpeed)
    {
        float limMin = legSliderJoint.limits.min;
        float limMax = legSliderJoint.limits.max;
        float targetExtension = Mathf.Lerp(limMin, limMax, extensionSelector);
        float currentExtension = legSliderJoint.jointTranslation;
        float deltaExtension =  targetExtension - currentExtension;
        //Debug.Log("Speed: " + currentSpeed + "Delta Ext: " + deltaExtension);
        legSlider.motorSpeed = Mathf.Clamp(deltaExtension / Time.fixedDeltaTime, -maxExtensionSpeed, maxExtensionSpeed);
        legSlider.maxMotorTorque = maxExtensionForce;
        legSliderJoint.motor = legSlider;
    }


    void OnValidate()
    {

    }
}
