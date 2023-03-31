using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    Vector3 offset;

    /// <summary>
    /// Method untuk mendapatkan offset antara target dan camera
    /// </summary>
    void Start()
    {
        offset = transform.position - target.position;
    }

    /// <summary>
    /// Meethod untuk set posisi camera
    /// </summary>
    void FixedUpdate()
    {
        // Mendapatkan posisi kamera
        Vector3 targetCamPos = target.position + offset;

        // Set posisi camera dengan smoothing (delay)
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
