using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FloorInfo : MonoBehaviour
{
    private VisualElement root;
    private Label Floor;
    private Label Enemies;
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        Floor = root.Q<Label>("Floor");
        Enemies = root.Q<Label>("Enemies");
        InvokeRepeating("RainbowLabel", 0f, 0.15f);
    }
    public void SetFloor(int floof)
    {
        Floor.text = $"Floor: {floof}";
    }
    public void SetEnemies(int WhiteDiamond)
    {
        Enemies.text = $"{WhiteDiamond} enemies left";
    }
    void RainbowLabel()
    {
        Floor.style.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
