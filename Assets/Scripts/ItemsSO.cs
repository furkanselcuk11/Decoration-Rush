using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Items Type", menuName = "ItemsSO")]
public class ItemsSO : ScriptableObject
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private string _tag = "Item";
    [SerializeField] private bool _active;

    public GameObject itemPrefab
    {
        get { return _itemPrefab; }
        set { _itemPrefab = value; }
    }
    public bool active
    {
        get { return _active; }
        set { _active = value; }
    }
    public string tag
    {
        get { return _tag; }
        set { _tag = value; }
    }
}
