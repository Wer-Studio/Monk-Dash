using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAxisProcessor
{
    public float mouseSensitivity = 1f;
    public float smoothing = 1f;

    private Vector2 rawMouse;
    private Vector2 smoothMouse;

    private Vector2 value;
    private int fetchCount;

    public MouseAxisProcessor() {
        fetchCount = 0;
        value.x = 0;
        value.y = 0;
    }

    
    public void PollInput() {
        //Get raw mouse delta input
        rawMouse.Set(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        //Get the average over frame length. (mouse axis is accumulated within the frame)
        rawMouse.x /= Time.deltaTime;
        rawMouse.y /= Time.deltaTime;
        //mouse movements can be very erratic due to bad mouse pads, bad friction, etc.. weight in old mouse value with new value to smooth the movement. 
        smoothMouse.x = smoothMouse.x * (1f / smoothing) + rawMouse.x * (1f - 1f / smoothing);
        smoothMouse.y = smoothMouse.y * (1f / smoothing) + rawMouse.y * (1f - 1f / smoothing);
        //scale value with sensitivity
        value.x += smoothMouse.x * mouseSensitivity * 0.01f;
        value.y += smoothMouse.y * mouseSensitivity * 0.01f;
        fetchCount++;
        //Debug.Log("Value: " + _value.ToString("F4"));
    }

    public Vector2 ReturnInput() {
        if(fetchCount == 0) {
            return Vector2.zero;
        }
        Vector2 result;
        //resulting input is the average of all polls until this moment.
        result.x = value.x / fetchCount;
        result.y = value.y / fetchCount;
        value.x = 0;
        value.y = 0;
        fetchCount = 0;
        return result;
    }
}
