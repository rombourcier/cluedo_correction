using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CE_Board : MonoBehaviour
{
    #region Events
    #endregion

    #region Members
    #region Private
    static CE_Board instance = null;
    [SerializeField] List<CE_StartCharacter> allStartPoints = new List<CE_StartCharacter>();
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public static CE_Board Instance => instance;
    public List<CE_Cell> AllBoardCells { get; private set; } = new List<CE_Cell>();
    public Dictionary<int, CE_Room> AllRooms { get; private set; } = new Dictionary<int, CE_Room>();
        #endregion

    #region Methods
    #region Private
    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    public void Add(CE_Cell _cell) => AllBoardCells.Add(_cell);
    public void AddRoom(int _id, CE_Room _room) => AllRooms.Add(_id, _room);
    public CE_Room GetRoom(int _id) => AllRooms[_id];
    public CE_Cell GetNearestCell(Vector3 _position) => AllBoardCells.OrderBy(c => Vector3.Distance(c.Position, _position)).FirstOrDefault();
    public Transform GetStartPos(CE_GameCharacter _chara) => allStartPoints.FirstOrDefault(s => s.Character == _chara).StartCell;
    #endregion
    #region Public
    #endregion
    #endregion
}