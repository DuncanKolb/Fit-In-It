using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Shake parameters
    public float shakeIntensity = 0.5f;
    public float shakeDuration = 0.5f;
    public float smoothness = 10f;

    // Store original camera position and rotation
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // Trigger the camera shake
    public void TriggerShake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        StartCoroutine(ShakeCoroutine());
    }

    // Coroutine to handle the camera shake over time
    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Generate Perlin noise values
            float x = UnityEngine.Random.Range(-1f, 1f);
            float y = UnityEngine.Random.Range(-1f, 1f);

            // Calculate the shake amount and apply it to the camera position
            Vector3 shakeAmount = new Vector3(x, y, 0f) * shakeIntensity;
            transform.position = originalPosition + shakeAmount;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the camera to its original position
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}

