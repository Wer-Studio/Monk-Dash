using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkController : MonoBehaviour
{
    [SerializeField]
    private float health;

    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float runForce;

    [SerializeField]
    private float glideSpeed;
    [SerializeField]
    private float glideForce;

    [SerializeField]
    private float groundTurningForce;
    [SerializeField]
    private float airTurningForce;

    [SerializeField]
    private float armExtendingForce;
    [SerializeField]
    private float armRotatingForce;

    //internal states
    private int sustainability;
    public const int AIRBORNE = 0;
    public const int GROUNDED = 1;
    public const int SLIDING = 2;

    private int stability;
    public const int STABLE = 0;
    public const int RECOVERING = 2;
    public const int STUNNED = 1;

    private int stance;
    public const int OFFENSIVE = 0;
    public const int DEFENSIVE = 1;

    private int formation;
    public const int STRAIGHT = 0;
    public const int DIVING = 1;


    //internal variables
    private float currentHealth;
    private float knockDuration;


    //monk actions
    float horizontalAxis;
    float verticalAxis;
    bool jumpAction;
    bool diveAction;
    bool defenseAction;

    //internal controllers
    Rigidbody2D rb;



    // Start is called before the first frame update
    void Start()
    {
        //default starting states
        sustainability = AIRBORNE;
        stability = STABLE;
        stance = OFFENSIVE;
        formation = STRAIGHT;
        //init controllers
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //readActionInput();
        if (rb.IsSleeping())
        {
            //Debug.Log("Sleeping");
        }
    }

    private void readActionInput()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        jumpAction = Input.GetKey("w");
        diveAction = Input.GetKey("space");
        defenseAction = Input.GetMouseButtonDown(0);

    }

    private void recieveImpactKnocks()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        calculateImpactDamage(col);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        //calculateImpactDamage(col);
    }

    private float calculateImpactDamage(Collision2D col)
    {
        float result = 0.0f;
        const float MIN_DISTANCE = 0.1f;
        const float NORMAL_COEFF = 1f/10;
        const float TANGENT_COEFF = 1f/100;
        const float DAMAGE_THRESHOLD = 5;
        float normalPressure = 0;
        float tangentPressure = 0;
        if (col.contactCount == 1)
        {
            
            ContactPoint2D cp = col.GetContact(0);
            normalPressure = cp.normalImpulse / MIN_DISTANCE;
            tangentPressure = cp.tangentImpulse / MIN_DISTANCE;
        }
        else if(col.contactCount == 2)
        {
            ContactPoint2D cp1 = col.GetContact(0);
            ContactPoint2D cp2 = col.GetContact(1);
            float surfaceArea = Mathf.Max(Vector2.Distance(cp1.point, cp2.point), MIN_DISTANCE);

            normalPressure = (cp1.normalImpulse + cp2.normalImpulse) / surfaceArea;
            tangentPressure = (cp1.tangentImpulse + cp2.tangentImpulse) / surfaceArea;
        }

        Debug.Log("Normal Pressure: " + normalPressure + " Tangent Pressure: " + tangentPressure);

        float damage = normalPressure * NORMAL_COEFF + tangentPressure * TANGENT_COEFF - DAMAGE_THRESHOLD;
        Debug.Log("Damage: " + damage);
        damage = damage > 0 ? damage : 0;
        return 0;
    }

    private void impactDemo(Collision2D col)
    {
        Debug.Log(col.gameObject.name);
        foreach(ContactPoint2D cp in col.contacts)
        {
            float vel = Vector2.SqrMagnitude(cp.relativeVelocity);
            float impulse = cp.normalImpulse;
            Debug.Log("vel: " + vel + "impulse: " + impulse);
        }
    }

    private float calculateImpactStun(Collision2D col)
    {
        float result = 0.0f;

        return result;
    }
}
