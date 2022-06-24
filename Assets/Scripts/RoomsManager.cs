using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    public static RoomsManager roomsmanagerInstance;    

    [Space]
    [Header("Room Controller")]
    [SerializeField] private GameObject roomsParent;    // Odalarýn tutulduðu parent obje
    public List<GameObject> Rooms = new List<GameObject>();   // Oyundaki oda sayýlarý
    public List<GameObject> roomItems = new List<GameObject>();   // Leveldeki odanýn eþya sayýsý
    public List<GameObject> activeItems = new List<GameObject>();   // Odalardaki aktif eþyalar
    public int currentRoom = 0;

    private void Awake()
    {
        if (roomsmanagerInstance == null)
        {
            roomsmanagerInstance = this;
        }
    }
    void Start()
    {
        PullList();
    }
    
    void Update()
    {
        
    }
    private void PullList()
    {
        for (int i = 0; i < roomsParent.transform.childCount; i++)
        {
            Rooms.Add(roomsParent.transform.GetChild(i).transform.gameObject);   // Oyundaki odalarý listeye ekler
        }
        for (int i = 0; i < roomsParent.transform.GetChild(currentRoom).transform.GetChild(0).transform.childCount; i++)
        {
            roomItems.Add(roomsParent.transform.GetChild(currentRoom).transform.GetChild(0).transform.GetChild(i).transform.gameObject);   // Açýk olan odadaki eþyalarý listeye ekler
        }
    }
    public void PlaceItem(GameObject contactObject)
    {
        // Toplanan eþyayý odaya yerleþtir  

        if (roomItems.Where(obj => obj.name == contactObject.name).SingleOrDefault())
        {
            // Eðer toplanan eþya odada var ise aktif hale getir
            Debug.Log("Mevcut");
            GameObject temp = roomItems.Where(obj => obj.name == contactObject.name).SingleOrDefault(); // Toplanan eþyayý Temp objesine eþleþtir
            temp.SetActive(true);
            roomItems.Remove(temp);
            //temp.transform.parent = null;

            //PullList();
            temp.transform.parent = roomsParent.transform.GetChild(currentRoom).transform.GetChild(1);
            activeItems.Add(temp);

            PullList();
            StartCoroutine(nameof(NextLevelCoroutine));

        }
        else
        {
            // Mevcut olmayan objeyi sat
            Debug.Log("Mevcut Deðil!!!");
        }
    }

    IEnumerator NextLevelCoroutine()
    {
        yield return new WaitForSeconds(2f);
        GameManager.gamemanagerInstance.NextLevel();
        yield return null;
    }
}
