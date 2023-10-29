using System.Collections;
using UnityEngine;

public class ItemDistributor : MonoBehaviour
{
    public string playerTag = "Player"; // The tag of the player GameObject

    private bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            playerInside = true;
            StartCoroutine(StartTiming());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            playerInside = false;
            StopCoroutine(StartTiming());
        }
    }

    private IEnumerator StartTiming()
    {
        float timer = 0f;

        while (playerInside)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                Debug.Log("Time: " + Time.time);
                timer = 0f;
            }
            yield return null;
        }
    }
}
