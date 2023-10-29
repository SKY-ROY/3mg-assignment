using System.Collections;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 90.0f; // Degrees per second

    private void Start()
    {
        // Start the rotation coroutine
        StartCoroutine(RotateObjectOverTime(2.0f)); // Rotate over 2 seconds
    }

    private IEnumerator RotateObjectOverTime(float duration)
    {
        float elapsedTime = 0.0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90, 0); // Rotate 90 degrees around Y-axis

        while (elapsedTime < duration)
        {
            // Interpolate between start and target rotations
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches the target rotation exactly
        transform.rotation = targetRotation;
    }
}
