using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CE_Room : MonoBehaviour
{
    #region Events
    #endregion

    #region Members
    #region Private
    [SerializeField] int id = 0;
    [SerializeField] string roomName = "room";
    [SerializeField] Color roomColor = Color.white;
    [SerializeField] List<CE_Door> roomDoors = new List<CE_Door>();
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public int ID => id;
    public string RoomName => roomName;
    public Vector3 Position => transform.position;
    public Color RoomColor => roomColor;
    #endregion

    #region Methods
    #region Private
    private void Start()
    {
        CE_Board.Instance?.AddRoom(ID, this);
    }
    #endregion
    #region Public
    public CE_Door GetNearestDoor(IGamePlayable _char) => roomDoors.OrderBy(d => Vector3.Distance(_char.CharacterRef.CharacterTransform.position, d.Position)).ToList().FirstOrDefault();

    public void AddDoorLink(CE_Door _door) => roomDoors.Add(_door);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Position, 1);
    }
    #endregion
    #endregion
}