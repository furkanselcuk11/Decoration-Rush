using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gamemanagerInstance;

    [HideInInspector] public bool startTheGame; // Oyun baþladýmý
    [Space]
    [Header("Game Controller")]
    [SerializeField] private GameObject Player;
    [SerializeField] private float swipeSpeed, diffBetweenItems;    // Toplana objelerin yana kayma hýzý ve objeler arasý mesafesi
    [Space]
    [Header("Collected Controller")]
    public List<Transform> Collected = new List<Transform>();   // Toplanan objelerin listesi
    [Space]
    [Header("Merge Controller")]
    [SerializeField] private List<Transform> PakuourItems = new List<Transform>(); // Parkurdaki objelerin  tutulduðu liste
    [SerializeField] private GameObject pakuourItemsParent;    // Parkurda tutulan objelerin bulunduðu parent obje
    [SerializeField] private Mesh modelYouWantToUse;    // Kullanýlacak(deðiþim olmasý istenilen) modelin meshi

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
        for (int i = 0; i < pakuourItemsParent.transform.childCount; i++)
        {
            PakuourItems.Add(pakuourItemsParent.transform.GetChild(i).transform);   // Parkurda tutulan objelerin bulunduðu parent objenin altýnda kaç adet obje varsa listeye ekler
            pakuourItemsParent.transform.GetChild(i).gameObject.AddComponent<BoxCollider>(); //pakuourItemsParent altýndaki tüm objeler collider ekler
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

                // Stack (Toplama) iþlemi sonrasý toplanan objelerin  sýralý þekilde gidiþini ayarlar
                sectItem.position = new Vector3(Mathf.Lerp(sectItem.position.x, firstItem.position.x, swipeSpeed * Time.deltaTime),
                    sectItem.position.y,
                    Mathf.Lerp(sectItem.position.z, firstItem.position.z + diffBetweenItems, swipeSpeed * Time.deltaTime));
            }
        }
    }

    public void Add(GameObject collectedObject)
    {
        // Stack (Toplama) iþlemi yapar
        collectedObject.transform.parent = null;
        collectedObject.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        collectedObject.gameObject.AddComponent<Stack>(); // Toplanan objeler Stack Componenti eklernir
        collectedObject.gameObject.GetComponent<Collider>().isTrigger = true; // Toplanan objelerin isTrigger aktif eder(Diðer objeler temas edince toplama yapmasý için)
        collectedObject.tag = gameObject.tag;
        Collected.Add(collectedObject.transform); // Toplanan objeleri Collected listesine ekler

        PakuourItems.Remove(collectedObject.transform); // Objelerin tutulduðu ana parent listesinden temas edilen objeler silinir 
    }
    public void Fail(GameObject failGate)
    {
        // Toplanan objeler silinir
        if (Collected.Count > 0)
        {
            int totalCollect = Collected.Count;
            for (int i = 0; i < totalCollect - 1; i++)
            {
                // totalCollect-1 olmasý player objesinin içinde olmasýndan ve silmemesi için
                Destroy(Collected.ElementAt(Collected.Count - 1).gameObject);   // Tüm objeler silinir 
                Collected.RemoveAt(Collected.Count - 1); // Silinnen objeler Collected listesinden atýlýr               
            }
        }
        failGate.GetComponent<Collider>().enabled = false; // Fail(testere) kapýsýndan geçdiðimizde kapýnýn mesh collider kapat        
    }
    public void Restart()
    {
        // Eðer Toplanmýþ obje yok ise ve Fail duvarýndan geçilmiþse oyunu yeniden baþlat
        startTheGame = false;   // Oyuna baþlamak pasif olur
        Debug.Log("GameOver");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Merge(GameObject mergeGate)
    {
        // Parkurdaki tüm objeleri toplanmasý gereken objeye dönüþtürür
        for (int i = 0; i < PakuourItems.Count; i++)
        {
            // Parkurda bulunan tüm toplanacak objeleri deðiþtirir
            pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().mesh = modelYouWantToUse;   // pakuourItemsParent(parkurda) listesinde bulunan tüm objelerin mesh özelliðini deðiþtirir
            pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().name = "Chair";
            // Materyal rengi
            // pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().tag="Chair";
            // pakuourItemsParent(parkurda) listesinde bulunan tüm objelerin isim ve tagýný deðiþtirir
            Destroy(pakuourItemsParent.transform.GetChild(i).GetComponent<BoxCollider>());  // pakuourItemsParent(parkurda) listesinde bulunan tüm objelerin BoxCollider kaldýrýr
            pakuourItemsParent.transform.GetChild(i).gameObject.AddComponent<BoxCollider>(); // pakuourItemsParent(parkurda) listesinde bulunan tüm objelerin BoxCollider ekler (Deðiþen meshe göre Collider boyut almasý için)
        }

        // Birleþtirme iþlemi yapýlýr -- Toplanan objeleri tek bir objeye dönüþtürür
        if (Collected.Count > 0)
        {
            int totalCollect = Collected.Count;
            for (int i = 1; i < totalCollect - 1; i++)
            {
                // Toplanan objelerden en baþtaki hariç tümü silinir - Objeleri yeni objeye yükseltme iþlemi
                Destroy(Collected.ElementAt(Collected.Count - 1).gameObject);
                Collected.RemoveAt(Collected.Count - 1);
            }
            // En baþtaki objenin mesh özelliðini deðiþtirir
            Collected.ElementAt(1).transform.GetComponent<MeshFilter>().mesh = modelYouWantToUse;
            Collected.ElementAt(1).transform.GetComponent<MeshFilter>().name = "Chair";
            // Materyal rengi
            // pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().tag="Chair";
            // pakuourItemsParent(parkurda) listesinde bulunan tüm objelerin isim ve tagýný deðiþtirir
        }
        mergeGate.GetComponent<Collider>().enabled = false; // Merge kapýsýndan geçdiðimizde kapýnýn mesh collider kapat
    }
    public void ColorChange(GameObject contactObject, GameObject colorGate)
    {
        // Temas edilen objelerin rengi deðiþir
        colorGate.transform.GetComponent<MeshRenderer>().material = contactObject.transform.GetComponent<MeshRenderer>().material;
    }
    public void Polishing(GameObject contactObject)
    {
        // Temas edilen objelerin cilalamasý yapýlýr
        // Efekt ekle
        // Para kazan
    }
}
