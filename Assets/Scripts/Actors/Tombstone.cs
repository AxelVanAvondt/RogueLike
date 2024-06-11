using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Get.Tombstones.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
