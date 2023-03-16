// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float anglePerSecond = 1.0f;


    private void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * anglePerSecond);
    }
}
