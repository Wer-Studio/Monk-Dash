using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class DamageController : MonoBehaviour
{

    public float impactCoefficient = 1f;
    public float slashCoefficient = 2f;
    public float impactThreshold = 15f;
    public float slashThreshold = 15f;
    public float damageThreshold = 1f;

    public float stunRatio = 0.04f;
    public float stunThreshold = 0.5f;

    public Color damageDebugColor = Color.red;
    public Color stunDebugColor = Color.blue;

    private const float debugLinesDuration = 0.2f;

    void OnCollisionEnter2D(Collision2D col)
    {
        applyDamageForces(col);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        applyDamageForces(col);
    }

    private void applyDamageForces(Collision2D col)
    {
        float impactForce;
        float slashForce;
        Vector2 point;
        Vector2 normal;
        if(col.contactCount == 1)
        {
            ContactPoint2D cp = col.GetContact(0);
            impactForce = cp.normalImpulse * impactCoefficient;
            slashForce = cp.tangentImpulse * slashCoefficient;
            point = cp.point;
            normal = cp.normal;
        }
        else
        {
            ContactPoint2D cp1 = col.GetContact(0);
            ContactPoint2D cp2 = col.GetContact(1);
            impactForce = (cp1.normalImpulse + cp2.normalImpulse) * impactCoefficient;
            slashForce = (cp2.tangentImpulse + cp2.tangentImpulse) * slashCoefficient;
            point = (cp1.point + cp2.point) / 2f;
            normal = cp1.normal;
        }
        //Debug.DrawRay(point, normal * impactForce * 0.01f, damageDebugColor, debugLinesDuration, false);
        //Debug.DrawRay(point, Vector2.Perpendicular(normal) * slashForce * 0.01f, damageDebugColor, debugLinesDuration, false);

        float impactDamage = Mathf.Max(impactForce - impactThreshold, 0);
        float slashDamage = Mathf.Max(slashForce - slashThreshold, 0);
        float damage = impactDamage + slashDamage;
        if (damage > damageThreshold)
        {
            Debug.Log(this.gameObject.name + " damage: " + damage);
            //TODO: send damage to object controller
        }

        float stun = impactDamage * stunRatio;
        if(stun > stunThreshold)
        {
            Debug.Log(this.gameObject.name + " stun: " + stun);
            //TODO: send stun to object controller
        }

    }
}
