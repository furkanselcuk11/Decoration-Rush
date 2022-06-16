using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gamemanagerInstance;

    [HideInInspector] public bool startTheGame;
    [Space]
    [Header("Game Controller")]
    [SerializeField] private GameObject Player;
    [SerializeField] private float swipeSpeed, diffBetweenItems;
    [Space]
    [Header("Collected Controller")]
    public List<Transform> Collected = new List<Transform>();   // Toplanan objelerin listesi
    [Space]
    [Header("Merge Controller")]
    [SerializeField] private List<Transform> PakuourItems = new List<Transform>(); // Parkurdaki objelerin  tutuldu�u liste
    [SerializeField] private GameObject pakuourItemsParent;    // Parkurda tutulan objelerin bulundu�u parent obje
    [SerializeField] private MeshFilter modelYouWantToChange;   // De�i�ecek obje
    [SerializeField] private Mesh modelYouWantToUse;    // secilen obje

    private void Awake()
    {
        if (gamemanagerInstance == null)
        {
            gamemanagerInstance = this;
        }
    }
    void Start()
    {
        Collected.Add(Player.transform);
        for (int i = 0; i < pakuourItemsParent.transform.childCount; i++)
        {
            PakuourItems.Add(pakuourItemsParent.transform.GetChild(i).transform);   // Parkurda tutulan objelerin bulundu�u parent objenin alt�nda ka� adet obje varsa listeye ekler
            pakuourItemsParent.transform.GetChild(i).gameObject.AddComponent<BoxCollider>(); //pakuourItemsParent alt�ndaki t�m objeler collider ekler
        }
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (Collected.Count > 1)
        {
            for (int i = 1; i < Collected.Count; i++)
            {
                var firstItem = Collected.ElementAt(i - 1);
                var sectItem = Collected.ElementAt(i);
                                
                // Stack (Toplama) i�lemi sonras� toplanan objelerin  s�ral� �ekilde gi�ini ayarlar
                sectItem.position = new Vector3(Mathf.Lerp(sectItem.position.x, firstItem.position.x, swipeSpeed * Time.deltaTime),
                    sectItem.position.y,
                    Mathf.Lerp(sectItem.position.z, firstItem.position.z + diffBetweenItems, swipeSpeed * Time.deltaTime));                
            }
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

        PakuourItems.Remove(collectedObject.transform); // Objelerin tutuldu�u ana parent listesinden temas edilen objeler silinir 
    }
    public void Fail(GameObject failGate)
    {
        // Toplanan objeler silinir
        if (Collected.Count>0)
        {
            int totalCollect = Collected.Count;
            for (int i = 0; i < totalCollect-1; i++)
            {
                // totalCollect-1 olmas� player objesinin i�inde olmas�ndan ve silmemesi i�in
                Destroy(Collected.ElementAt(Collected.Count - 1).gameObject);   // T�m objeler silinir 
                Collected.RemoveAt(Collected.Count - 1); // Silinnen objeler Collected listesinden at�l�r               
            }            
        }
        failGate.GetComponent<Collider>().enabled = false; // Fail(testere) kap�s�ndan ge�di�imizde kap�n�n mesh collider kapat        
    }
    public void Restart()
    {
        startTheGame = false;
        Debug.Log("GameOver");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Merge(GameObject mergeGate)
    {
        // Birle�tirme i�lemi yap�l�r -- Parkurdaki t�m objeleri d�n��t�r�r
        for (int i = 0; i < PakuourItems.Count; i++)
        {
            // Parkurda bulunan t�m toplanacak objeleri de�i�tirir
            pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().mesh = modelYouWantToUse;   // pakuourItemsParent(parkurda) listesinde bulunan t�m objelerin mesh �zelli�ini de�i�tirir
            pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().name="Chair";
            // pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().tag="Chair";
            // pakuourItemsParent(parkurda) listesinde bulunan t�m objelerin isim ve tag�n� de�i�tirir
            Destroy(pakuourItemsParent.transform.GetChild(i).GetComponent<BoxCollider>());  // pakuourItemsParent(parkurda) listesinde bulunan t�m objelerin BoxCollider kald�r�r
            pakuourItemsParent.transform.GetChild(i).gameObject.AddComponent<BoxCollider>(); // pakuourItemsParent(parkurda) listesinde bulunan t�m objelerin BoxCollider ekler (De�i�en meshe g�re Collider boyut almas� i�in)
        }

        // Birle�tirme i�lemi yap�l�r -- Toplanan objeleri tek bir objeye d�n��t�rmek i�in
        if (Collected.Count > 0)
        {
            int totalCollect = Collected.Count;
            for (int i = 1; i < totalCollect - 1; i++)
            {
                // Toplanan objelerden en ba�taki hari� t�m� silinir - Objeleri yeni objeye y�kseltme i�lemi
                Destroy(Collected.ElementAt(Collected.Count - 1).gameObject);
                Collected.RemoveAt(Collected.Count - 1);
            }
            // En ba�taki objenin mesh �zelli�ini de�i�tirir
            Collected.ElementAt(1).transform.GetComponent<MeshFilter>().mesh = modelYouWantToUse;
            Collected.ElementAt(1).transform.GetComponent<MeshFilter>().name = "Chair";
        }
        mergeGate.GetComponent<Collider>().enabled = false; // Merge kap�s�ndan ge�di�imizde kap�n�n mesh collider kapat
    }
    public void ColorChange(GameObject contactObject,GameObject colorGate)
    {
        // Temas edilen objelerin rengi de�i�ir
        colorGate.transform.GetComponent<MeshRenderer>().material= contactObject.transform.GetComponent<MeshRenderer>().material;     
    }
    public void Polishing(GameObject contactObject)
    {
        // Temas edilen objelerin cilalamas� yap�l�r
        // Efekt ekle
        // Para kazan
    }
}
