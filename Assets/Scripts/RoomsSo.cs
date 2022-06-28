using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room Item", menuName = "RoomItem")]
public class RoomsSo : ScriptableObject
{
    [SerializeField] private List<GameObject> _Rooms = new List<GameObject>();   // Oyundaki oda sayýlarý
    [SerializeField] private List<GameObject> _roomItems = new List<GameObject>();   // Leveldeki odanýn eþya sayýsý
    [SerializeField] private List<GameObject> _activeItems = new List<GameObject>();   // Odalardaki aktif eþyalar
    [SerializeField] private int _currentRoom = 0;

    public List<GameObject> Rooms
    {
        get { return _Rooms; }
        set { _Rooms = value; }
    }
    public List<GameObject> roomItems
    {
        get { return _roomItems; }
        set { _roomItems = value; }
    }
    public List<GameObject> activeItems
    {
        get { return _activeItems; }
        set { _activeItems = value; }
    }
    public int currentRoom
    {
        get { return _currentRoom; }
        set { _currentRoom = value; }
    }
}
