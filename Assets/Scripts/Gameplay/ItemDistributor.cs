using System;
using System.Collections;
using UnityEngine;

public class ItemDistributor : MonoBehaviour
{
    public string playerTag = "Player"; // The tag of the player GameObject

    private bool playerInside = false;

    public Action OnItemSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            playerInside = true;
            StartCoroutine(StartTiming(1f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            playerInside = false;
            StopCoroutine(StartTiming(1f));
        }
    }

    private IEnumerator StartTiming(float interval)
    {
        float timer = 0f;

        while (playerInside)
        {
            timer += interval;
            Debug.Log("Time: " + timer);

            OnItemSpawn?.Invoke();

            yield return new WaitForSeconds(interval);
        }
    }
}
