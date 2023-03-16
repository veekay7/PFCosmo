// Copyright 2018 Winterpetal. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    // Converts time in seconds to MM:SS
    public static string ConvertTimeToMMSS(float time)
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        return string.Format("{0:D1}:{1:D2}", t.Minutes, t.Seconds);
    }


    // Clamps an angle between minimum and maximum. Angle can never exceed 360 or negative 360. All values are calculated in angles and not radians.
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }


    // Ensures that the variable value will always cycle between min and max.
    public static int Cycle(int value, int min, int max)
    {
        if (value < min)
            value = max;
        else if (value > max)
            value = min;
        return value;
    }


    // Check is mouse position hovering over a Canvas object
    public static bool IsPointerOverCanvasObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }


    // Resets a Transform to their original values
    public static void ResetTransform(Transform transform)
    {
        transform.position = Vector3.zero;
        transform.localPosition = Vector3.one;
        transform.rotation = Quaternion.identity;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
