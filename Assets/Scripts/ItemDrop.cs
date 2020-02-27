using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDrop : MonoBehaviour
{

    public List<GameObject> items = new List<GameObject>();

    public Pawn Source { get; set; }

    public void AddItem(GameObject _obj)
    {
        if (_obj != null)
            items.Add(_obj);
    }

    public void DropAllItems()
    {
        foreach(GameObject item in items)
        {
            item.SetActive(true);
            item.transform.SetParent(item.transform);
            item.transform.position = Source.transform.position;
            items.Remove(item);
        }
    }
}
