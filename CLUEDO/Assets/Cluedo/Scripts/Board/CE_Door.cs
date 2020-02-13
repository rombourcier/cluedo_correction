using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CE_Door : MonoBehaviour
{
    #region Events
    #endregion

    #region Members
    #region Private
    [SerializeField] CE_Room connectedRoom = null;
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public CE_Room ConnectedRoom => connectedRoom;
    public CE_Cell Cell { get; private set; }
    public Vector3 Position => transform.position;
    public bool IsValid => connectedRoom;
    #endregion

    #region Methods
    #region Private

    private void Start()
    {
        Init();
    }

    void Init()
    {
        Cell = GetComponent<CE_Cell>();
        if (!IsValid) return;
        connectedRoom.AddDoorLink(this);
    }

    private void OnDrawGizmos()
    {
        if (!IsValid) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(Position, connectedRoom.Position);
    }
    #endregion
    #region Public
    #endregion
    #endregion
}