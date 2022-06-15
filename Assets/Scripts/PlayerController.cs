using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    [Header("Player Controller")]
    [SerializeField] private float speed = 30f;    // Player hareket hýzý
    [SerializeField] private float horizontalspeed = 10f; // Player yön hareket hýzý
    [SerializeField] private float defaultSwipe = 4f;    // // Player default kaydýrma mesafesi
    

    private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (GameManager.gamemanagerInstance.startTheGame)
        {
            transform.Translate(0, 0, speed * Time.fixedDeltaTime);
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        MoveInput();    // Player hareket kontrolü
    }
    void MoveInput()
    {
        #region Mobile Controller 4 Direction

        float moveX = transform.position.x; // Player objesinin x pozisyonun deðerini alýr      
        float moveZ = transform.position.z; // Player objesinin z pozisyonun deðerini alýr           

        if (Input.GetKey(KeyCode.LeftArrow) || MobileInput.instance.swipeLeft)
        {   // Eðer klavyede sol ok tuþuna basýldýysa yada "MobileInput" scriptinin swipeLeft deðeri True ise  Sola hareket gider
            moveX = Mathf.Clamp(moveX - 1 * horizontalspeed * Time.fixedDeltaTime, -defaultSwipe, defaultSwipe);    // Pozisyon sýnýrlandýrýlmasý koyulacaksa
            // Player objesinin x (sol) pozisyonundaki gideceði min-max sýnýrý belirler
            //moveX = moveX - 1 * horizontalspeed * Time.fixedDeltaTime;    // Pozisyon sýnýrlandýrýlmasý yoksa 
        }
        else if (Input.GetKey(KeyCode.RightArrow) || MobileInput.instance.swipeRight)
        {   // Eðer klavyede sað ok tuþuna basýldýysa yada "MobileInput" scriptinin swipeRight deðeri True ise Saða hareket gider  
            moveX = Mathf.Clamp(moveX + 1 * horizontalspeed * Time.fixedDeltaTime, -defaultSwipe, defaultSwipe);    // Pozisyon sýnýrlandýrýlmasý koyulacaksa
            // Player objesinin x (sað) pozisyonundaki gideceði min-max sýnýrý belirler
            //moveX = moveX + 1 * horizontalspeed * Time.fixedDeltaTime;    // Pozisyon sýnýrlandýrýlmasý yoksa 
        }
        else
        {
            rb.velocity = Vector3.zero; // Eðer hareket edilmediyse Player objesi sabit kalsýn
        }

        transform.position = new Vector3(moveX, transform.position.y, moveZ);
        // Player objesinin pozisyonu moveX deðerine göre x ekseninde, moveZ deðerine göre z ekseninde hareket eder ve y ekseninde sabit kalýr 

        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wood"))
        {
            GameManager.gamemanagerInstance.Add(other.gameObject);
        }
    }
}
