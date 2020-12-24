using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSliderController : MonoBehaviour
{
    // Start is called before the first frame update
    public float period;
    public float cooldown;
    SliderJoint2D slider;
    JointTranslationLimits2D lim;
    float speed;
    float minSpeed = 0.5f;
    float marginTime;
    int orientation = -1;
    JointMotor2D m;
    Rigidbody2D rb;

    void Start()
    {
        slider = GetComponent<SliderJoint2D>();
        lim = slider.limits;
        speed = (lim.max - lim.min) / period;
        m.maxMotorTorque = slider.motor.maxMotorTorque;
        m.motorSpeed = speed * orientation;
        slider.motor = m;
        marginTime = 0;
        rb = GetComponent<Rigidbody2D>();
        minSpeed = (slider.motor.maxMotorTorque / rb.mass) * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(name == "Platform") {
            Debug.Log("Vel: " + rb.velocity.magnitude.ToString("F4") + "cooldown: " + marginTime);
        }
        if(rb.velocity.magnitude < minSpeed) {
            if(marginTime >= cooldown) {
                orientation *= -1;
                marginTime = 0;
            } else {
                marginTime += Time.fixedDeltaTime;
            }
        } else {
            marginTime = 0;
        }
        m.motorSpeed = speed * orientation;
        slider.motor = m;
    }
}
