using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public float mouseSensitivity = 1f;
    public float maxImpulseRate = 1.1f;
    public float smoothing = 1f;

    private TargetJoint2D targetJoint;
    private Rigidbody2D rb;

    private Vector2 deltaPos;

    private Vector2 smoothMouseAxis;
    private Vector2 mouseAxis;
    private Vector2 mouseOutput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetJoint = GetComponent<TargetJoint2D>();
        Cursor.lockState = CursorLockMode.Locked;
        smoothMouseAxis.Set(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        mouseAxis.Set(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        smoothMouseAxis = Vector2.Lerp(smoothMouseAxis, mouseAxis, 1/smoothing);
        mouseOutput = smoothMouseAxis * mouseSensitivity;
        Debug.Log("Axis: " + smoothMouseAxis.ToString("F6"));
        
        targetJoint.target = rb.position + mouseOutput;
    }
}
