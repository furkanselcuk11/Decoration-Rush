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
    [SerializeField] private List<Transform> PakuourItems = new List<Transform>(); // Parkurdaki objelerin  tutulduðu liste
    [SerializeField] private GameObject pakuourItemsParent;    // Parkurda tutulan objelerin bulunduðu parent obje
    [SerializeField] private MeshFilter modelYouWantToChange;   // Deðiþecek obje
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
                                
                // Stack (Toplama) iþlemi sonrasý toplanan objelerin  sýralý þekilde giþini ayarlar
                sectItem.position = new Vector3(Mathf.Lerp(sectItem.position.x, firstItem.position.x, swipeSpeed * Time.deltaTime),
                    sectItem.position.y,
                    Mathf.Lerp(sectItem.position.z, firstItem.position.z + diffBetweenItems, swipeSpeed * Time.deltaTime));                
            }
        }
    }

    public void Add(GameObject x)
    {   
        // Stack (Toplama) iþlemi yapar
        x.transform.parent = null;
        x.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        x.gameObject.AddComponent<Stack>(); // Toplanan objeler Stack Componenti eklernir
        x.gameObject.GetComponent<Collider>().isTrigger = true; // Toplanan objelerin isTrigger aktif eder(Diðer objeler temas edince toplama yapmasý için)
        x.tag = gameObject.tag;
        Collected.Add(x.transform); // Toplanan objeleri Collected listesine ekler

        PakuourItems.Remove(x.transform); // Objelerin tutulduðu ana parent listesinden temas edilen objeler silinir 
    }
    public void Fail(GameObject x)
    {
        // Toplanan objeler silinir
        if (Collected.Count>0)
        {
            int totalCollect = Collected.Count;
            for (int i = 0; i < totalCollect-1; i++)
            {
                // totalCollect-1 olmasý player objesinin içinde olmasýndan ve silmemesi için
                Destroy(Collected.ElementAt(Collected.Count - 1).gameObject);   // Tüm objeler silinir 
                Collected.RemoveAt(Collected.Count - 1); // Silinnen objeler Collected listesinden atýlýr               
            }            
        }
        x.GetComponent<Collider>().enabled = false; // Fail(testere) kapýsýndan geçdiðimizde kapýnýn mesh collider kapat        
    }
    public void Merge(GameObject x)
    {
        //modelYouWantToChange.mesh = modelYouWantToUse;    // Deðiþecek obje=secilen obje
        // Birleþtirme iþlemi yapýlýr
        for (int i = 0; i < PakuourItems.Count; i++)
        {
            // Parkurda bulunan tüm toplanacak objeleri deðiþtirir
            pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().mesh = modelYouWantToUse;   // pakuourItemsParent(parkurda) listesinde bulunan tüm objelerin mesh özelliðini deðiþtirir
            pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().name="Chair";
            // pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().tag="Chair";
            // pakuourItemsParent(parkurda) listesinde bulunan tüm objelerin isim ve tagýný deðiþtirir
            Destroy(pakuourItemsParent.transform.GetChild(i).GetComponent<BoxCollider>());  // pakuourItemsParent(parkurda) listesinde bulunan tüm objelerin BoxCollider kaldýrýr
            pakuourItemsParent.transform.GetChild(i).gameObject.AddComponent<BoxCollider>(); // pakuourItemsParent(parkurda) listesinde bulunan tüm objelerin BoxCollider ekler (Deðiþen meshe göre Collider boyut almasý için)
        }

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
        }
        x.GetComponent<Collider>().enabled = false; // Merge kapýsýndan geçdiðimizde kapýnýn mesh collider kapat
    }
}
