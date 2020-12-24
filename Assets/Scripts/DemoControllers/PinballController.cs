using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballController : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 vector;
    public float speed;

    private Vector2 startingPos;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPos = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 trajectory = rb.position - startingPos;
        if(trajectory.magnitude >= vector.magnitude) {
            rb.position = startingPos;
        }
        rb.velocity = vector.normalized * speed;
    }
}
