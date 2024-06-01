using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Consumable> Items = new List<Consumable>();
    public int MaxItems;

    public void AddItem(Consumable item)
    {
        //if(Items.Count < MaxItems)
        //{
        //    Items.Add(item);
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        Items.Add(item);
    }
    public void DropItem(Consumable item)
    {
        Items.Remove(item);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
