using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput instance;   // Diger Script'ler uzrerinden erisimi saglar

    // Mouse Positions
    private Vector2 start_pos;
    Vector2 last_pos;
    Vector2 delta;

    [Header("Controllers")]
    public bool tap;
    [HideInInspector] public bool swipeLeft;
    [HideInInspector] public bool swipeRight;
    [HideInInspector] public bool swipeUp;
    [HideInInspector] public bool swipeDown;
    public bool swipe;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        // Butın boollari sifirliyoruz
        tap = swipe = false;
        swipeLeft = false;  // Sola kaydirma
        swipeRight = false; // Saga kaydirma
    }
    private void FixedUpdate()
    {
        SwipeMove();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {   // Mosue tusuna basildiginda veya ekranda parmak ile basildigındaki ilk pozisyon degerini al�ir
            start_pos = Input.mousePosition;    // ilk posizsyon de�geri tutulur
            GameManager.gamemanagerInstance.startTheGame = tap = true; // Dokunma aktif olur  
            // Oyuna başlamak için ekrana dokun (StartGame) True olur
        }

        if (Input.GetMouseButton(0))
        {   // Mosue tusuna basili tutuldugunda veya ekranda parmak ile basili tutularak gidildigindeki son pozisyonun degerini al�r
            last_pos = Input.mousePosition; // Son pozisyon de�eri tutulur
            delta = start_pos - last_pos;   // Toplam kaydirilan mesafe hesaplanir ve delta degerinde tutulur
            swipe = true;   // Kaydirma aktif olur

        }

        if (Input.GetMouseButtonUp(0))
        {   // Mosue tusuna basma birakildiginda veya ekranda parmak basma birakildiginda
            if (start_pos == last_pos) swipe = false;
            // Eger dokunulan ilk pozisyon ile son pozisyon degeri ayni ise kaydirma pasif olur
            start_pos = Vector2.zero;
            last_pos = Vector2.zero;
            delta = Vector2.zero;
            // Tum degerler sifirlanir tekrar dokunma islemine kadar

            swipeRight = false;
            swipeLeft = false;
            tap = false;
            // Tum bool degerler sifirlanir tekrar dokunma islemine kadar
        }
    }
    void SwipeMove()
    {
        #region Mobile Controller 4 Direction
        if (tap)    // Eger dokunma islemi aktif ise calisir
        {
            if (swipe)  // Eger swipe(kaydirma) islemi aktif ise calisir
            {
                if (delta.magnitude > 100)  // delta degerinin uzunluk bilgisini alir ve 100 degerinden buyukse calisir - 100 degeri minimum kaydirma mesafesi
                {
                    float x = delta.x;  // Kaydirma mesafesinin x degerini alir
                    float y = delta.y;  // Kaydirma mesafesinin y degerini alir
                    if (Mathf.Abs(x) > Mathf.Abs(y))    // Eger kaydirma mesafesinin x ekseni y ekseninden daha buyukse (Right-Left) degilse (Up-Down) kaydirma aktif olur
                    {
                        // Right-Left
                        if (x < 0)
                        {
                            // Saga kaydirma aktif olur 
                            swipeRight = true;
                            swipeLeft = false;
                            swipeUp = false;
                            swipeDown = false;
                        }
                        else
                        {
                            // Sola kaydirma aktif olur 
                            swipeRight = false;
                            swipeLeft = true;
                            swipeUp = false;
                            swipeDown = false;
                        }
                    }
                    else
                    {
                        // Up-Down
                        if (y < 0)
                        {
                            // ileri kaydirma aktif olur 
                            swipeRight = false;
                            swipeLeft = false;
                            swipeUp = true;
                            swipeDown = false;
                        }
                        else
                        {
                            // Geri kaydirma aktif olur 
                            swipeRight = false;
                            swipeLeft = false;
                            swipeUp = false;
                            swipeDown = true;
                        }
                    }
                }
            }
            else
            {   // Eger kaydirma islemi pasif ise 
                tap = false;    // Dokunma pasif olur
            }
        }
        #endregion
    }
}