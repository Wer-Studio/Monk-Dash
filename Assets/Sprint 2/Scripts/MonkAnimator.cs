using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkAnimator : MonoBehaviour
{
    // Start is called before the first frame update

    class BoneData
    {
        Vector2 position;
        float rotation;

    }

    enum PostureState
    {
        Standing, Crouching
    }

    enum FeetState
    {
        Grounded, Flying
    }

    enum MovementState
    {
        Idle, Moving
    }

    private PostureState postureS;
    private FeetState feetS;
    private MovementState movementS;
    float movementSpeed;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReadState();
        UpdateBones();
    }


    private void ReadState() {

    }

    private void UpdateBones() {

    }
}
