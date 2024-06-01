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
    public InventoryUI Inventory { get { return inventory.GetComponent<InventoryUI>(); } }
    public void UpdateHealth(int current, int max)
    {
        healthBar.GetComponent<HealthBar>().SetValues(current, max);
    }

    public void AddMessage(string message, Color color)
    {
        messages.GetComponent<Messages>().AddMessage(message, color);
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
