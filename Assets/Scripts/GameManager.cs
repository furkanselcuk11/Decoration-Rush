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
    public List<Transform> Collected = new List<Transform>();

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

                if (DesireDistence <= distance)
                {
                    sectItem.position = new Vector3(Mathf.Lerp(sectItem.position.x, firstItem.position.x, swipeSpeed * Time.deltaTime),
                        sectItem.position.y,
                        Mathf.Lerp(sectItem.position.z, firstItem.position.z + diffBetweenItems, swipeSpeed * Time.deltaTime));
                }
            }
        }
    }

    public void Add(GameObject x)
    {
        x.transform.parent = null;
        x.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        x.gameObject.AddComponent<Stack>();
        x.gameObject.GetComponent<Collider>().isTrigger = true;
        x.tag = gameObject.tag;
        Collected.Add(x.transform);
    }
    public void Fail(GameObject x)
    {
        if (Collected.Count>0)
        {
            int totalCollect = Collected.Count;
            for (int i = 0; i < totalCollect-1; i++)
            {
                Collected.ElementAt(Collected.Count - 1).gameObject.SetActive(false);
                Destroy(Collected.ElementAt(Collected.Count - 1).gameObject);
                Collected.RemoveAt(Collected.Count - 1);                
            }            
        }
        x.GetComponent<Collider>().enabled = false; // Fail(testere) kapýsýndan geçdiðimizde kapýnýn mesh collider kapat        
    }
    public void Restart()
    {
        startTheGame = false;
        Debug.Log("Restart");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
