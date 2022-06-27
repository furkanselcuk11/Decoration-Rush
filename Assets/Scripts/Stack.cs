using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            // Wood objesi ekler
            GameManager.gamemanagerInstance.Add(other.gameObject);
        }
        if (other.CompareTag("Fail"))
        {
            // Topladýðý tüm objeler yok olur
            GameManager.gamemanagerInstance.Fail(other.gameObject);
            Debug.Log("Fail");
        }
        if (other.CompareTag("Merge"))
        {
            // Merge içinden geçerse bir sonraki objeye dönüþür
            Debug.Log("Merge");
            GameManager.gamemanagerInstance.Merge(other.gameObject);
        }
        if (other.CompareTag("Color"))
        {
            // Color içinden geçen objeler belirtilen rengi alýr
            Debug.Log("Color");
            GameManager.gamemanagerInstance.ColorChange(other.gameObject, this.gameObject);
        }
        if (other.CompareTag("Polish"))
        {
            // Customize içinden geçen objeler cilalanýr            
            GameManager.gamemanagerInstance.Polishing(other.gameObject);
        }
        if (other.CompareTag("Finish"))
        {
            // Finish içinden geçen obje odaye yerleþtirilir            
            //GameManager.gamemanagerInstance.PlaceItem(this.gameObject);
            RoomsManager.roomsmanagerInstance.PlaceItem(this.gameObject);
        }
    }
}
