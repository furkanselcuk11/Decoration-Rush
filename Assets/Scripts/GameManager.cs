using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gamemanagerInstance;

    [HideInInspector] public bool startTheGame; // Oyun ba�lad�m�
    [HideInInspector] public bool isFinish; // Level bittimi
    [Space]
    [Header("Game Controller")]
    [SerializeField] private GameObject Player;
    [SerializeField] private float swipeSpeed, diffBetweenItems;    // Toplana objelerin yana kayma h�z� ve objeler aras� mesafesi
    [Space]
    [Header("Collected Controller")]
    public List<Transform> Collected = new List<Transform>();   // Toplanan objelerin listesi
    [Space]
    [Header("Merge Controller")]
    [SerializeField] private List<Transform> PakuourItems = new List<Transform>(); // Parkurdaki objelerin  tutuldu�u liste
    [SerializeField] private GameObject pakuourItemsParent;    // Parkurda tutulan objelerin bulundu�u parent obje
    [SerializeField] private Mesh[] modelYouWantToUse;    // Kullan�lacak(de�i�im olmas� istenilen) modelin meshi
    [SerializeField] private Material[] metarialYouWantToUse;    // Kullan�lacak(de�i�im olmas� istenilen) modelin materyali
    [SerializeField] private int currentModel;
    [Space]
    [Header("Score Controller")]
    public TextMeshProUGUI totalMoneyTxt;
    public TextMeshProUGUI moneyTxt;
    public int totalMoney;
    private int money;
    [Space]
    [Header("Room Controller")]
    [SerializeField] private GameObject roomsParent;    // Odalar�n tutuldu�u parent obje
    public List<GameObject> Rooms = new List<GameObject>();   // Oyundaki oda say�lar�
    public List<GameObject> roomItems = new List<GameObject>();   // Leveldeki odan�n e�ya say�s�

    private void Awake()
    {
        if (gamemanagerInstance == null)
        {
            gamemanagerInstance = this;
        }
    }
    void Start()
    {
        Collected.Add(Player.transform);    // Player objesini Toplanan Objeler listesine ekler
        PullList(); // Oyundaki listeleri �eker
        currentModel = 0;
        money = 0;
        moneyTxt.text = money.ToString();
    }
    void Update()
    {
        if (currentModel == 0)
        {
            ParkourItemsChange();
        }
    }
    private void FixedUpdate()
    {
        if (Collected.Count > 1)
        {
            for (int i = 1; i < Collected.Count; i++)
            {
                var firstItem = Collected.ElementAt(i - 1);
                var sectItem = Collected.ElementAt(i);

                // Stack (Toplama) i�lemi sonras� toplanan objelerin  s�ral� �ekilde gidi�ini ayarlar
                sectItem.position = new Vector3(Mathf.Lerp(sectItem.position.x, firstItem.position.x, swipeSpeed * Time.deltaTime),
                    sectItem.position.y,
                    Mathf.Lerp(sectItem.position.z, firstItem.position.z + diffBetweenItems, swipeSpeed * Time.deltaTime));
            }            
        }
    }
    private void PullList()
    {
        for (int i = 0; i < pakuourItemsParent.transform.childCount; i++)
        {
            PakuourItems.Add(pakuourItemsParent.transform.GetChild(i).transform);   // Parkurda tutulan objelerin bulundu�u parent objenin alt�nda ka� adet obje varsa listeye ekler
            pakuourItemsParent.transform.GetChild(i).gameObject.AddComponent<BoxCollider>(); //pakuourItemsParent alt�ndaki t�m objeler collider ekler
        }
        for (int i = 0; i < roomsParent.transform.childCount; i++)
        {
            Rooms.Add(roomsParent.transform.GetChild(i).transform.gameObject);   // Oyundaki odalar� listeye ekler
        }
        for (int i = 0; i < roomsParent.transform.GetChild(0).transform.GetChild(0).transform.childCount; i++)
        {
            roomItems.Add(roomsParent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).transform.gameObject);   // A��k olan odadaki e�yalar� listeye ekler
        }
    }

    public void Add(GameObject collectedObject)
    {
        // Stack (Toplama) i�lemi yapar
        collectedObject.transform.parent = null;
        collectedObject.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        collectedObject.gameObject.AddComponent<Stack>(); // Toplanan objeler Stack Componenti eklernir
        collectedObject.gameObject.GetComponent<Collider>().isTrigger = true; // Toplanan objelerin isTrigger aktif eder(Di�er objeler temas edince toplama yapmas� i�in)
        collectedObject.tag = gameObject.tag;
        Collected.Add(collectedObject.transform); // Toplanan objeleri Collected listesine ekler
        AudioController.audioControllerInstance.Play("Money");

        PakuourItems.Remove(collectedObject.transform); // Objelerin tutuldu�u ana parent listesinden temas edilen objeler silinir 
    }
    public void Fail(GameObject failGate)
    {
        // Toplanan objeler silinir
        currentModel = 0;
        if (Collected.Count > 0)
        {
            int totalCollect = Collected.Count;
            for (int i = 0; i < totalCollect - 1; i++)
            {
                // totalCollect-1 olmas� player objesinin i�inde olmas�ndan ve silmemesi i�in
                Destroy(Collected.ElementAt(Collected.Count - 1).gameObject);   // T�m objeler silinir 
                Collected.RemoveAt(Collected.Count - 1); // Silinnen objeler Collected listesinden at�l�r               
            }
        }
        failGate.GetComponent<Collider>().enabled = false; // Fail(testere) kap�s�ndan ge�di�imizde kap�n�n mesh collider kapat     
        AudioController.audioControllerInstance.Play("Saw");
    }
    public void Restart()
    {
        // E�er Toplanm�� obje yok ise ve Fail duvar�ndan ge�ilmi�se oyunu yeniden ba�lat
        startTheGame = false;   // Oyuna ba�lamak pasif olur
        Debug.Log("GameOver");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Merge(GameObject mergeGate)
    {
        // Parkurdaki t�m objeleri toplanmas� gereken objeye d�n��t�r�r
        if (currentModel < modelYouWantToUse.Length - 1)
        {
            currentModel++;
        }
        else
        {
            currentModel = modelYouWantToUse.Length - 1;
        }
        ParkourItemsChange();

        // Birle�tirme i�lemi yap�l�r -- Toplanan objeleri tek bir objeye d�n��t�r�r
        if (Collected.Count > 0)
        {
            int totalCollect = Collected.Count;
            for (int i = 1; i < totalCollect - 1; i++)
            {
                // Toplanan objelerden en ba�taki hari� t�m� silinir - Objeleri yeni objeye y�kseltme i�lemi
                Destroy(Collected.ElementAt(Collected.Count - 1).gameObject);
                Collected.RemoveAt(Collected.Count - 1);
            }
            // Birle�imden sonraki objeyi de�i�tirir
            Collected.ElementAt(1).transform.GetComponent<MeshFilter>().mesh = modelYouWantToUse[currentModel]; // En ba�taki objenin mesh �zelli�ini de�i�tirir
            Collected.ElementAt(1).transform.GetComponent<MeshFilter>().name = modelYouWantToUse[currentModel].name;    // En ba�taki objenin ismini de�i�tirir
            Collected.ElementAt(1).transform.GetComponent<MeshRenderer>().material = metarialYouWantToUse[currentModel];    // En ba�taki objenin materyalini de�i�tirir
            // pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().tag="Chair";
        }
        mergeGate.GetComponent<Collider>().enabled = false; // Merge kap�s�ndan ge�di�imizde kap�n�n mesh collider kapat
    }
    private void ParkourItemsChange()
    {
        // Parkurda bulunan t�m toplanacak objeleri de�i�tirir
        for (int i = 0; i < PakuourItems.Count; i++)
        {            
            pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().mesh = modelYouWantToUse[currentModel];   // pakuourItemsParent(parkurda) listesinde bulunan t�m objelerin mesh �zelli�ini de�i�tirir
            pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().name = modelYouWantToUse[currentModel].name;    // pakuourItemsParent(parkurda) listesinde bulunan t�m objelerin ismini de�i�tirir
            pakuourItemsParent.transform.GetChild(i).GetComponent<MeshRenderer>().material = metarialYouWantToUse[currentModel];    // pakuourItemsParent(parkurda) listesinde bulunan t�m objelerin materyalini de�i�tirir
            // pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().tag="Chair";
            Destroy(pakuourItemsParent.transform.GetChild(i).GetComponent<BoxCollider>());  // pakuourItemsParent(parkurda) listesinde bulunan t�m objelerin BoxCollider kald�r�r
            pakuourItemsParent.transform.GetChild(i).gameObject.AddComponent<BoxCollider>(); // pakuourItemsParent(parkurda) listesinde bulunan t�m objelerin BoxCollider ekler (De�i�en meshe g�re Collider boyut almas� i�in)
        }
    }
    public void ColorChange(GameObject contactObject, GameObject colorGate)
    {
        // Temas edilen objelerin rengi de�i�ir
        colorGate.transform.GetComponent<MeshRenderer>().material = contactObject.transform.GetComponent<MeshRenderer>().material;
    }
    public void Polishing(GameObject contactObject)
    {
        // Temas edilen objelerin cilalamas� yap�l�r
        money++;
        moneyTxt.text = money.ToString();   // Para kazan
        AudioController.audioControllerInstance.Play("Polish");
        Debug.Log("Polish");
        // Efekt ekle        
    }
    public void PlaceItem(GameObject contactObject)
    {
        // Toplanan e�yay� odaya yerle�tir
        Debug.Log(contactObject.name);
        if (contactObject.name==Collected.ElementAt(1).transform.name)
        {
            Debug.Log("Next");
        }
    }
}
