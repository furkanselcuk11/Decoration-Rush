using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rooms", menuName = "Rooms")]
public class RoomsSo : ScriptableObject
{    
    public List<GameObject> RoomsSO = new List<GameObject>();   // Oyundaki oda say�lar�
    public List<GameObject> roomItemsSO = new List<GameObject>();   // Leveldeki odan�n e�ya say�s�
    public List<GameObject> activeItemsSO = new List<GameObject>();   // Odalardaki aktif e�yalar
    public int currentRoomSO = 0;
}
