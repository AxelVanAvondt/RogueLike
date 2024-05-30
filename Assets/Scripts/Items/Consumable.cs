using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public enum ItemType
    {
        HealthPotion,
        Fireball,
        ScrollOfConfusion
    }
    [SerializeField] private ItemType type;
    public ItemType Type { get { return type; } }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Get.AddItem(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
