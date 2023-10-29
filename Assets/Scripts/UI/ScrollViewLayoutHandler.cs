using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public enum ListDirection
{
    HORIZONTAL,
    VERTICAL
}

public class ScrollViewLayoutHandler : MonoBehaviour
{
    ScrollRect scrollView;
    private int selectedIndex;

    [SerializeField] GameObject contentListRoot;
    List<GameObject> listElements = new List<GameObject>();
    public List<GameObject> ListElements => listElements;

    void Awake()
    {
        scrollView = GetComponent<ScrollRect>();
    }

    public GameObject PopulateScrollViewList(GameObject listElementPrefab)
    {
        GameObject obj = ObjectPooler.Instance.GetPooledObject(listElementPrefab.name, Vector3.zero, Quaternion.identity);

        obj.transform.parent = contentListRoot.transform;

        listElements.Add(obj);
        return obj;
    }

    public void RemoveElementFromScollViewList(GameObject obj)
    {
        listElements.Remove(obj);
        obj.SetActive(false);
    }

    public void AddSequenceDelay()
    {
        for (int i = 0; i < listElements.Count; i++)
        {
            // listElements[i].GetComponentInChildren<IEditSequenceProperties>().AddSequenceStartDelay(i * subsequentAnimationDelay);
        }
    }

    public void HandleSelection(int index)
    {
        IScrollViewListItem svli = listElements[selectedIndex].GetComponentInChildren<IScrollViewListItem>();
        svli.ActivateSelectionIndicator(false);

        selectedIndex = index;
        svli = listElements[selectedIndex].GetComponentInChildren<IScrollViewListItem>();
        svli.ActivateSelectionIndicator(true);
    }
}
