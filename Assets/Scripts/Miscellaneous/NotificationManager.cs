using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public TMP_Text notificationText;
    private Coroutine displayCoroutine;

    private static NotificationManager instance;

    // Singleton pattern: Access the instance via a static property
    public static NotificationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NotificationManager>();

                if (instance == null)
                {
                    // Create an empty GameObject with the NotificationManager script attached if not found
                    GameObject go = new GameObject("NotificationManager");
                    instance = go.AddComponent<NotificationManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Ensure there is only one instance
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        // Initialize the text component
        if (notificationText == null)
        {
            notificationText = GetComponentInChildren<TMP_Text>();
        }

        // Hide the text initially
        notificationText.gameObject.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        // Cancel any ongoing display coroutine
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }

        // Set the new message
        notificationText.text = message;

        Debug.Log(message);

        // Show the text
        notificationText.gameObject.SetActive(true);

        // Start a coroutine to hide the text after 5 seconds
        displayCoroutine = StartCoroutine(HideAfterDelay(5f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Hide the text after the delay
        notificationText.gameObject.SetActive(false);
    }
}
