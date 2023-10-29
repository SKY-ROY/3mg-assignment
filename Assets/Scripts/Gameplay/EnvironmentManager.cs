using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] List<EnvironmentObject> environmentObjects;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0, z = 0; i < environmentObjects.Count; i++, z++)
        {

            for (int j = 0, x = -5; j < environmentObjects[i].count; j++, x++)
            {
                GameObject obj = ObjectPooler.Instance.GetPooledObject(environmentObjects[i].spawnObject.name, new Vector3(x * 2, 2f, z * 2), Quaternion.identity, true);

            }
        }
    }
}