using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;              // ką rodys slote
    public GameObject worldPrefab;   // ką padėsim į pasaulį, kai numesi
}
