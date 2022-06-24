using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rooms", menuName = "Rooms")]
public class RoomsSo : ScriptableObject
{    
    public List<GameObject> RoomsSO = new List<GameObject>();   // Oyundaki oda sayýlarý
    public List<GameObject> roomItemsSO = new List<GameObject>();   // Leveldeki odanýn eþya sayýsý
    public List<GameObject> activeItemsSO = new List<GameObject>();   // Odalardaki aktif eþyalar
    public int currentRoomSO = 0;
}
