using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDrop : MonoBehaviour
{

    public List<GameObject> items = new List<GameObject>();
    WeightedItemDrop weightedDrop;

    public Pawn Source { get; set; }

    void Awake()
    {
        weightedDrop = GetComponent<WeightedItemDrop>();
    }

    public void AddItem(GameObject _obj)
    {
        if (_obj != null)
            items.Add(_obj);
    }

    public void DropAllItems()
    {
        Debug.Log("Dropping All Items...");
        foreach (GameObject item in items)
        {
            item.SetActive(true);
            item.transform.parent = transform;
            item.transform.position = Source.transform.position;
            items.Remove(item);
            weightedDrop.OnDrop();
        }
    }
}
