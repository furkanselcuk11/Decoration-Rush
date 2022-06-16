using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target; // Takip edilecek obje
    [SerializeField] private Vector3 _offset;   // Kamera ile takip edilecek obje arasındaki mesafe
    [SerializeField] private float _chaseSpeed = 5; // Takip etme hızı

    private void LateUpdate()
    {
        Vector3 desPos = _target.position + _offset;  // Kamera ile takip edilen obje arasındaki mesafe
        transform.position = Vector3.Lerp(transform.position, desPos, _chaseSpeed);   // Kamera pozisyonu yumuşak geçiş ile aradaki mesafe kadar uzaktan takip eder
    }
}