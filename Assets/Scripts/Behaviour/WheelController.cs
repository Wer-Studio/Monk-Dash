using UnityEngine;

public class WheelController : MonoBehaviour
{
    //Public Properties
    [Header("Required Components")]
    public HingeJoint2D legHingeJoint;
    public SliderJoint2D legSliderJoint;

    [Header("Joint Settings")]
    public float maxExtensionForce;
    public float jumpSpeed;                 //Speed when jumping.
    public float extensionSpeed;            //Speed when extending.
    public float uprightExtensionSelector;  // 0 CROUCHED | 0.5 UPRIGHT (DEFAULT) | 1 EXTENDED (MAX JUMP POS).
    public float hSpeed;


    [Header("Control Settings")]
    //..................................

    //Private States
    private JointMotor2D legHinge = new JointMotor2D();
    private JointMotor2D legSlider = new JointMotor2D();

    //Private Utility Objects
    
    //.........................................................

    // Start is called before the first frame update
    void Start()
    {
        legHingeJoint.motor = legHinge;
        legSliderJoint.motor = legSlider;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        //Get movement delta
        float hDelta = 0;
        float sliderSelector = uprightExtensionSelector;// 0 CROUCHED | 0.5 UPRIGHT (DEFAULT) | 1 EXTENDED (MAX JUMP POS)
        float sliderSpeed = extensionSpeed;

        //Vertical exclusive switch
        if (Input.GetKey("w") && !Input.GetKey("s")) // CASE MONK HAS TO EXTEND.
        {
            print("w");
            sliderSelector = uprightExtensionSelector;
            sliderSpeed = extensionSpeed;
        }
        if (Input.GetKey("s") && !Input.GetKey("w")) // CASE MONK HAS TO CROUCH DOWN.
        {
            print("s");
            sliderSelector = 0;
            sliderSpeed = extensionSpeed;
        }

        //Horitzontal exclusive switch
        if (Input.GetKey("a") && !Input.GetKey("d")) // CASE MONK HAS TO GO RIGHT. 
        {
            print("a");
            hDelta = -hSpeed;
        }
        if (Input.GetKey("d") && !Input.GetKey("a")) // CASE MONK HAS TO GO LEFT.
        {
            print("d");
            hDelta = hSpeed;
        }

        //Jump. Overrides W and S.
        if (Input.GetKey(KeyCode.Space))    // Jumping conditional is independent from other inputs.
        {
            print("JUMP!");
            sliderSelector = 1;
            sliderSpeed = jumpSpeed;
        }

        //MovelegHinge(hDelta);
        MovelegSlider(sliderSelector, sliderSpeed);
    }

    private void MovelegHinge(float hDelta)
    {
       
    }

    // This function decides the position of the vertical slider.
    // Semantically it decides how crouched or extended the monk's back is (so jumping has more inertia and looks more real)
    private void MovelegSlider(float sliderSelector, float sliderVelocity)
    {
        float limMin= legSliderJoint.limits.min;
        float limMax = legSliderJoint.limits.max;
        float targetExtension = Mathf.Lerp(limMin, limMax, sliderSelector);
        float currentExtension = legSliderJoint.jointTranslation;
        float currentSpeed = legSliderJoint.jointSpeed;
        float deltaExtension =  + targetExtension - currentExtension - currentSpeed*Time.fixedDeltaTime; //TODO Correct bouncing

        legSlider.motorSpeed = Mathf.Clamp(deltaExtension/Time.fixedDeltaTime, -sliderVelocity, sliderVelocity);
        legSlider.maxMotorTorque = maxExtensionForce;
        legSliderJoint.motor = legSlider;
    }


    void OnValidate()
    {

    }
}
