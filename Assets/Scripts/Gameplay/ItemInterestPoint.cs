using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ItemInterestPoint : MonoBehaviour
{
    [SerializeField] ItemInterestPointType interestPointType;
    public ItemInterestPointType InterestPointType => interestPointType;
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text timerText;
    [SerializeField] GameObject clothingItemPrefab;
    private string playerTag = "Player";
    private bool playerInside = false;

    public Action<GameObject> OnItemSpawn;
    public Action OnItemCollect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            playerInside = true;
            Backpack backpack = other.gameObject.GetComponent<Player>().BackPack;
            Debug.Log("Spawn Cycle started");
            StartCoroutine(StartTiming(1f, backpack));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            playerInside = false;
            Debug.Log("Spawn Cycle stopped");
            StopCoroutine(nameof(StartTiming));
        }
    }

    private IEnumerator StartTiming(float interval, Backpack assignedBackpack = null)
    {
        float timer = 0f;
        canvas.SetActive(true);

        while (playerInside)
        {
            yield return new WaitForSeconds(interval);

            timer += interval;
            timerText.text = $"{timer}";
            Debug.Log("Time: " + timer);

            if (!assignedBackpack.IsFull && interestPointType == ItemInterestPointType.Distributor && playerInside)
            {
                GameObject spawnItem = ObjectPooler.Instance.GetPooledObject(clothingItemPrefab.name, Vector3.zero, Quaternion.identity, true);

                OnItemSpawn?.Invoke(spawnItem);
            }

            if (interestPointType == ItemInterestPointType.Collector && playerInside)
            {
                OnItemCollect?.Invoke();
            }
        }

        canvas.SetActive(false);
    }
}
