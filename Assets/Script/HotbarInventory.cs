using UnityEngine;
using UnityEngine.UI;

public class HotbarInventory : MonoBehaviour
{
    [Header("UI slot icon images (vaiko Image viduje sloto)")]
    public Image[] slotIconImages;  // čia geriau naudoti ikonų Image, o ne sloto foną

    [Header("Inventory size")]
    public ItemData[] items; // tiek pat kiek slotų

    void Awake()
    {
        if (items == null || items.Length != slotIconImages.Length)
            items = new ItemData[slotIconImages.Length];

        RefreshUI();
    }

    public bool TryAdd(ItemData item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            Debug.Log($"TryAdd called. item={(item? item.name : "NULL")}");
            if (items[i] == null)
            {
                items[i] = item;
                RefreshUI();
                return true;
            }
        }
        return false; // nėra vietos
    }

    public ItemData GetSelected(int selectedIndex)
    {
        if (selectedIndex < 0 || selectedIndex >= items.Length) return null;
        return items[selectedIndex];
    }

    public void ClearSelected(int selectedIndex)
    {
        if (selectedIndex < 0 || selectedIndex >= items.Length) return;
        items[selectedIndex] = null;
        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slotIconImages.Length; i++)
        {
            var img = slotIconImages[i];
            var item = items[i];

            if (item == null)
            {
                img.enabled = false;
                img.sprite = null;
            }
            else
            {
                img.enabled = true;
                img.sprite = item.icon;
            }
        }
    }
}
