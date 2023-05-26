using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElectroSpawn : MonoBehaviour
{
    public GameObject electro;
    public List<GameObject> electroSpawnList = new List<GameObject>();

    private void Update()
    {
        if (electroSpawnList.Count < 16)
        {
            var add = Instantiate(electro);
            add.transform.position = electro.transform.position;
            electroSpawnList.Add(add);
        }
    }
}
