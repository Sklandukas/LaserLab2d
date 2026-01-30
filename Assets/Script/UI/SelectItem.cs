using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SelectItem : MonoBehaviour
{
    [Header("Slot Images (UI)")]
    public Image[] slotImages;

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite selectedSprite;

    [Header("Selection")]
    public int selectedIndex = 0;

    [Header("Optional")]
    public float scrollCooldown = 0.12f;
    private float nextScrollTime = 0f;

    void Start()
    {
        ApplySelection();
    }

    void Update()
    {
        if (Mouse.current == null) return;

        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) < 0.01f) return;

        if (Time.time < nextScrollTime) return;
        nextScrollTime = Time.time + scrollCooldown;

        if (scroll > 0f) SelectPrevious();
        else SelectNext();
    }

    void SelectNext()
    {
        if (slotImages == null || slotImages.Length == 0) return;
        selectedIndex = (selectedIndex + 1) % slotImages.Length;
        ApplySelection();
    }

    void SelectPrevious()
    {
        if (slotImages == null || slotImages.Length == 0) return;
        selectedIndex = (selectedIndex - 1 + slotImages.Length) % slotImages.Length;
        ApplySelection();
    }

    void ApplySelection()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (slotImages[i] == null) continue;

            // visiems uždedam normalų
            slotImages[i].sprite = normalSprite;
        }

        // pasirinktam uždedam selected
        if (selectedIndex >= 0 && selectedIndex < slotImages.Length && slotImages[selectedIndex] != null)
            slotImages[selectedIndex].sprite = selectedSprite;
    }
}
