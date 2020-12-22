using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyProcessor
{
    public int HorizontalAction { get { 
            if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) return -1;
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) return 1;
            return 0;
        } }
    
    public int VerticalAction { get {
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) return 1;
            if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) return -1;
            return 0;
        } }
    
    public int JumpAction { get {
            if (Input.GetKey(KeyCode.Space)) return 1;
            return 0;
        } }

}

