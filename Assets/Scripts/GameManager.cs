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
    [SerializeField] private float distance,swipeSpeed, diffBetweenItems;
    [Space]
    [Header("Collected Controller")]
    [SerializeField] private GameObject collectedObjects;
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

                var DesireDistence = Vector3.Distance(sectItem.position, firstItem.position);

                //if (DesireDistence <= distance)
                //{
                // Stack (Toplama) i�lemi sonras� toplanan objelerin  s�ral� �ekilde gi�ini ayarlar
                    sectItem.position = new Vector3(Mathf.Lerp(sectItem.position.x, firstItem.position.x, swipeSpeed * Time.deltaTime),
                        sectItem.position.y,
                        Mathf.Lerp(sectItem.position.z, firstItem.position.z + diffBetweenItems, swipeSpeed * Time.deltaTime));
                //}
            }
        }
    }

    public void Add(GameObject x)
    {   
        // Stack (Toplama) i�lemi yapar
        x.transform.parent = null;
        x.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        x.gameObject.AddComponent<Stack>();
        x.gameObject.GetComponent<Collider>().isTrigger = true;
        x.tag = gameObject.tag;
        Collected.Add(x.transform); // Toplanan objeleri Collected listesine ekler

        PakuourItems.Remove(x.transform); // Objelerin tutuldu�u ana parent listesinden temas edilen objeler silinir 
    }
    public void Fail(GameObject x)
    {
        // Toplanan objeler silinir
        if (Collected.Count>0)
        {
            int totalCollect = Collected.Count;
            for (int i = 0; i < totalCollect-1; i++)
            {
                //Collected.ElementAt(Collected.Count - 1).gameObject.SetActive(false);
                Destroy(Collected.ElementAt(Collected.Count - 1).gameObject);
                Collected.RemoveAt(Collected.Count - 1);                
            }            
        }
        x.GetComponent<Collider>().enabled = false; // Fail(testere) kap�s�ndan ge�di�imizde kap�n�n mesh collider kapat        
    }
    public void Merge(GameObject x)
    {
        //modelYouWantToChange.mesh = modelYouWantToUse;    // De�i�ecek obje=secilen obje
        //modelYouWantToChange.name = "Cube";
        //modelYouWantToChange.tag = "Chair";

        Fail(x);
        for (int i = 0; i < PakuourItems.Count; i++)
        {
            // Parkurda bulunan t�m toplanacak objeleri de�i�tirir
            pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().mesh = modelYouWantToUse;
            pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().name="Chair";
            //pakuourItemsParent.transform.GetChild(i).GetComponent<MeshFilter>().tag="Chair";
            Destroy(pakuourItemsParent.transform.GetChild(i).GetComponent<BoxCollider>());
            pakuourItemsParent.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
        }
        x.GetComponent<Collider>().enabled = false; // Merge kap�s�ndan ge�di�imizde kap�n�n mesh collider kapat
    }
    public void Restart()
    {
        startTheGame = false;
        Debug.Log("Restart");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
