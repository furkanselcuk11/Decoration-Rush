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
        if (other.CompareTag("Wood"))
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
            GameManager.gamemanagerInstance.Merge();
        }
        if (other.CompareTag("Color"))
        {
            // Color içinden geçen objeler belirtilen rengi alýr
            Debug.Log("Color");
        }
        if (other.CompareTag("Customize"))
        {
            // Customize içinden geçen objeler cilalanýr
            Debug.Log("Customize");
        }
    }
}
