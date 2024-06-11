using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public static UIManager Get { get => instance; }

    [Header("Documents")]
    public HealthBar healthBar;
    public Messages messages;
    public GameObject inventory;
    public FloorInfo floorInfo;
    public InventoryUI Inventory { get { return inventory.GetComponent<InventoryUI>(); } }
    public void UpdateHealth(int current, int max)
    {
        healthBar.GetComponent<HealthBar>().SetValues(current, max);
    }
    public void UpdateLevel(int level)
    {
        healthBar.GetComponent<HealthBar>().SetLevel(level);
    }
    public void UpdateXP(int xp)
    {
        healthBar.GetComponent<HealthBar>().SetXP(xp);
    }
    public void AddMessage(string message, Color color)
    {
        messages.GetComponent<Messages>().AddMessage(message, color);
    }
    public void UpdateFloor(int fl)
    {
        floorInfo.GetComponent<FloorInfo>().SetFloor(fl);
    }
    public void UpdateEnemies(int em)
    {
        floorInfo.GetComponent<FloorInfo>().SetEnemies(em);
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
