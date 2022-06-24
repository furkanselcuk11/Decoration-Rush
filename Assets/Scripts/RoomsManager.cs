using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    public static RoomsManager roomsmanagerInstance;    

    [Space]
    [Header("Room Controller")]
    [SerializeField] private GameObject roomsParent;    // Odalar�n tutuldu�u parent obje
    public List<GameObject> Rooms = new List<GameObject>();   // Oyundaki oda say�lar�
    public List<GameObject> roomItems = new List<GameObject>();   // Leveldeki odan�n e�ya say�s�
    public List<GameObject> activeItems = new List<GameObject>();   // Odalardaki aktif e�yalar
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
            Rooms.Add(roomsParent.transform.GetChild(i).transform.gameObject);   // Oyundaki odalar� listeye ekler
        }
        for (int i = 0; i < roomsParent.transform.GetChild(currentRoom).transform.GetChild(0).transform.childCount; i++)
        {
            roomItems.Add(roomsParent.transform.GetChild(currentRoom).transform.GetChild(0).transform.GetChild(i).transform.gameObject);   // A��k olan odadaki e�yalar� listeye ekler
        }
    }
    public void PlaceItem(GameObject contactObject)
    {
        // Toplanan e�yay� odaya yerle�tir  

        if (roomItems.Where(obj => obj.name == contactObject.name).SingleOrDefault())
        {
            // E�er toplanan e�ya odada var ise aktif hale getir
            Debug.Log("Mevcut");
            GameObject temp = roomItems.Where(obj => obj.name == contactObject.name).SingleOrDefault(); // Toplanan e�yay� Temp objesine e�le�tir
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
            Debug.Log("Mevcut De�il!!!");
        }
    }

    IEnumerator NextLevelCoroutine()
    {
        yield return new WaitForSeconds(2f);
        GameManager.gamemanagerInstance.NextLevel();
        yield return null;
    }
}
