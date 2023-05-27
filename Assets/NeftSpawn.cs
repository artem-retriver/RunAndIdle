using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeftSpawn : MonoBehaviour
{
    public Neft neft;
    public List<Neft> neftSpawnList = new();

    private void Update()
    {
        if (neftSpawnList.Count < 10)
        {
            var add = Instantiate(neft);
            add.transform.position = neft.transform.position;
            neftSpawnList.Add(add);
        }
    }
}
