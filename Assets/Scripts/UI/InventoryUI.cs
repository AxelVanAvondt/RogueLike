using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    public Label[] labels = new Label[8];
    private VisualElement root;
    private int selected;
    private int numItems;

    public int Selected { get { return selected; } }

    public void Clear()
    {
        for (int i = 1; i < labels.Length; i++)
        {
            labels[i].text = string.Empty;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        for (int i = 1; i < labels.Length; i++)
        {
            labels[i] = root.Q<Label>($"item{i}");
        }
        Clear();
        root.style.display = DisplayStyle.None;
    }

    private void UpdateSelected()
    {
        for (int i = 1; i < labels.Length; i++)
        {
            if (i == selected)
            {
                labels[i].style.backgroundColor = Color.green;
            }
            else
            {
                labels[i].style.backgroundColor = Color.clear;
            }
        }
    }

    public void SelectNextItem()
    {
        if(selected < numItems)
        {
            selected++;
        }
        UpdateSelected();
    }
    public void SelectPreviousItem()
    {

        if (selected > 0)
        {
            selected--;
        }
        UpdateSelected();
    }
    public void Show(List<Consumable> list)
    {
        selected = 0;
        numItems = labels.Length;
        Clear();
        for (int i = 1; i < labels.Length; i++)
        {
            labels[i] = root.Q<Label>($"item{i}");
        }
        UpdateSelected();
        root.style.display = DisplayStyle.Flex;
    }
    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
