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
            // Toplad��� t�m objeler yok olur
            GameManager.gamemanagerInstance.Fail(other.gameObject);
            Debug.Log("Fail");
        }
        if (other.CompareTag("Merge"))
        {
            // Merge i�inden ge�em t�m objeler belirtilen objelere d�n���r
            Debug.Log("Merge");
        }
        if (other.CompareTag("Color"))
        {
            // Color i�inden ge�en objeler belirtilen rengi al�r
            Debug.Log("Color");
        }
        if (other.CompareTag("Customize"))
        {
            // Customize i�inden ge�en objeler cilalan�r
            Debug.Log("Customize");
        }
    }
}