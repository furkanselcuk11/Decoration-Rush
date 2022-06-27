using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [SerializeField] private ItemsSO itemsType = null;    // Scriptable Objects erişir

    public GameObject itemPrefab;
    [SerializeField] private bool active;
    private GameObject newObj;

    private void Awake()
    {
        this.gameObject.transform.tag = itemsType.tag;
        this.itemPrefab = itemsType.itemPrefab;
        this.gameObject.SetActive(itemsType.active);
        this.transform.name = itemPrefab.name;

        newObj = Instantiate(itemPrefab, this.gameObject.transform.position, Quaternion.identity);
        newObj.transform.parent = this.transform;
    }
    void Start()
    {

    }    
    void Update()
    {
        
    }
    public void ItemsChange()
    {
        Destroy(this.newObj);
        this.newObj = Instantiate(this.itemPrefab, this.gameObject.transform.position, Quaternion.identity);
        this.newObj.transform.parent = this.transform;
        this.transform.name = itemPrefab.name;
    }
}
