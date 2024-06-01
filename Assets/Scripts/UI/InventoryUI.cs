using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    public Label[] labels = new Label[8];
    private VisualElement root;
    private int selected = 0;
    private int numItems;

    public int Selected
    {
        get { return selected; }
    }

    private void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        for (int i = 0; i < labels.Length; i++)
        {
            labels[i] = root.Q<Label>($"item{i + 1}");
            if (labels[i] == null)
            {
                Debug.LogError($"Label item{i + 1} not found in the UI document.");
            }
            else
            {
                Debug.Log($"Label item{i + 1} successfully found.");
            }
        }

        Clear();
        root.style.display = DisplayStyle.None;
    }

    private void Clear()
    {
        for (int i = 0; i < labels.Length; i++)
        {
            if (labels[i] != null)
            {
                labels[i].text = string.Empty;
                labels[i].style.backgroundColor = new StyleColor(Color.clear);
            }
        }
        selected = 0;
        numItems = 0;
    }

    private void UpdateSelected()
    {
        for (int i = 0; i < labels.Length; i++)
        {
            if (labels[i] != null)
            {
                if (i == selected)
                {
                    labels[i].style.backgroundColor = new StyleColor(Color.green);
                }
                else
                {
                    labels[i].style.backgroundColor = new StyleColor(Color.clear);
                }
            }
        }
    }

    public void SelectNextItem()
    {
        if (numItems == 0) return;
        selected = (selected + 1) % numItems;
        UpdateSelected();
    }

    public void SelectPreviousItem()
    {
        if (numItems == 0) return;
        selected = (selected - 1 + numItems) % numItems;
        UpdateSelected();
    }

    public void Show(List<Consumable> list)
    {
        selected = 0;
        numItems = list.Count;
        Clear();

        for (int i = 0; i < list.Count && i < labels.Length; i++)
        {
            if (list[i] != null && labels[i] != null)
            {
                labels[i].text = list[i].name;
            }
        }

        UpdateSelected();
        root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }
}
