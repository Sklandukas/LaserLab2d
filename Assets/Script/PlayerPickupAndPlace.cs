using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickupAndPlace : MonoBehaviour
{
    public HotbarInventory hotbarInventory;
    public SelectItem selectItem;

    [Header("Where to place dropped item")]
    public Transform dropPoint;  // jei tuščia – dės prie player pozicijos

    private PickupItem nearbyPickup;

    void Update()
    {
        if (Keyboard.current == null) return;

        // Paimti (E)
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryPickup();
        }

        // Padėti (Q)
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            TryPlaceSelected();
        }
    }

    void TryPickup()
    {
        if (nearbyPickup == null) return;
        if (nearbyPickup.itemData == null) return;

        bool added = hotbarInventory.TryAdd(nearbyPickup.itemData);
        if (added)
        {
            Destroy(nearbyPickup.gameObject); // dingsta iš pasaulio
            nearbyPickup = null;
        }
        // jei neadded – reiškia inventorius pilnas (galėsim vėliau parodyti UI žinutę)
    }

    void TryPlaceSelected()
    {
        int idx = selectItem.selectedIndex;
        ItemData item = hotbarInventory.GetSelected(idx);
        if (item == null) return;
        if (item.worldPrefab == null) return;

        Vector3 pos = (dropPoint != null) ? dropPoint.position : transform.position;

        Instantiate(item.worldPrefab, pos, Quaternion.identity);

        hotbarInventory.ClearSelected(idx); // dingsta iš sloto
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var pickup = other.GetComponent<PickupItem>();
        if (pickup != null)
            nearbyPickup = pickup;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var pickup = other.GetComponent<PickupItem>();
        if (pickup != null && pickup == nearbyPickup)
            nearbyPickup = null;
    }
}
