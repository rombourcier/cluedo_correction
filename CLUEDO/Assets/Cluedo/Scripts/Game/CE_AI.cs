using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CE_AI : MonoBehaviour, IGamePlayable, IMove
{
    #region Events
    public event Action<bool> OnSelectPlayable;
    public event Action OnStartTurn;
    public event Action OnEndTurn;
    public event Action<CE_Suggest, IGamePlayable> OnStartSuggest;
    public event Action<CE_Suggest, IGamePlayable, IGamePlayable, CE_Card> OnSuggestProgress;
    public event Action<CE_Card, IGamePlayable, IGamePlayable> OnSuggestProgressResult;
    public event Action<string, List<CE_Note>, Action<CE_Card>> OnSuggestPhase;
    public event Action<CE_Cell> OnNewStep;
    public event Action<CE_Room, Action> OnCanEnterRoom;
    #endregion

    #region Members
    #region Private
    [SerializeField] CE_GameBoadCharacter characterRef = null;
    [SerializeField] CE_HandCards handCards = new CE_HandCards();
    [SerializeField] int diceCount = 0;
    [SerializeField] int stepCount = 0;
    [SerializeField] Light lightFeedBack = null;
    [SerializeField] CE_NoteSystem noteSystem = new CE_NoteSystem();
    [SerializeField] AIPhase currentAIPhase = AIPhase.Idle;
    [SerializeField] CE_Room nextRoomInvestigate = null;
    [SerializeField] CE_Room lastRoom = null;
    [SerializeField] CE_Door nextDoorTarget = null;
    [SerializeField] bool isInRoom = false;
    [SerializeField] bool endMoveThinking = false;
    CE_CellNavigation iaNavigation = new CE_CellNavigation();
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public CE_GameBoadCharacter CharacterRef => characterRef;

    public CE_Cell StartCell => throw new NotImplementedException();

    public CE_HandCards HandCards => handCards;

    public CE_Room NextRoomInvestigate => nextRoomInvestigate;

    public CE_Room LastRoom
    {
        get
        {
            CE_Room lastRoom1 = lastRoom;
            return lastRoom1;
        }
    }

    public CE_Door NextDoorTarget => nextDoorTarget;

    public bool IsInRoom => isInRoom;

    public CE_NoteSystem NoteSystem => noteSystem;

    public CE_Cell CurrentCell {get; private set;} = null;

    public CE_CellNavigation Navigation => iaNavigation;

    public bool IsMoving { get; private set; } = false;

    public bool CanMove => throw new NotImplementedException();

    public int NavigationDiceValue =>diceCount;
    #endregion

    #region Methods
    #region Private
    private void Awake()
    {
        CE_GameManager.OnDiceRoll += OnStart;
        OnSelectPlayable += LightFeedBack;
    }
    IEnumerator AIFSM()
    {
        currentAIPhase = AIPhase.Idle;
        if (!nextRoomInvestigate)
            nextRoomInvestigate = GetNextRoom();
        if ((!nextDoorTarget))
            nextDoorTarget = GetNextDoor();
        //Debug.Log($"it's my turn {characterRef.ColorName} with {nextRoomInvestigate.RoomName}");
        yield return StartCoroutine(IAMove());
        yield return StartCoroutine(IAEndMove());
        yield return StartCoroutine(IASuggest());
        OnEndTurn?.Invoke();
    }

    public IEnumerator Move()
    {
        while (iaNavigation.PathCompleted && transform.position != iaNavigation.EndCell.Position)
        {
            CurrentCell = iaNavigation.Path[stepCount];
            transform.position = CurrentCell.Position;
            stepCount++;
            if(stepCount == NavigationDiceValue)
            {
                yield return new WaitForSeconds(1);
                IsMoving = false;
                yield break;
            }
            yield return new WaitForSeconds(.3f);
            IsMoving = false;
        }
         yield break;
    }

    IEnumerator IAMove()
    {
        if (!nextDoorTarget) yield break;
        SetNavigation(nextDoorTarget.Cell, true);
        currentAIPhase = AIPhase.Move;
        yield return StartCoroutine(Move());
        endMoveThinking = true;
    }

    IEnumerator IAEndMove()
    {
        while(endMoveThinking)
        {
            currentAIPhase = AIPhase.RoomEnterThink;
            if (stepCount < diceCount && CurrentCell == nextDoorTarget.Cell)
            {
                endMoveThinking = false;
                EnterInRoom();
            }
            else endMoveThinking = false;
            yield return null;
        }
        nextDoorTarget = null;
        IsMoving = false;
        stepCount = 0;
        diceCount = 0;
        iaNavigation.Reset();
    }

    void EnterInRoom()
    {
        isInRoom = true;
        transform.position = NextRoomInvestigate.Position;
        currentAIPhase = AIPhase.Suggest;
    }
    IEnumerator IASuggest()
    {
        if (!isInRoom) yield break;
        CE_Card _pickCharacterCard = CE_GameManager.Instance.GameDeck.GetCard(NoteSystem.PickRandomNotes(CardType.Character).ID);
        CE_Card _pickWeaponCard = CE_GameManager.Instance.GameDeck.GetCard(NoteSystem.PickRandomNotes(CardType.Weapon).ID);
        CE_Card _pickRoomCard = CE_GameManager.Instance.GameDeck.GetCard(nextRoomInvestigate.ID);
        CE_Suggest _suggest = new CE_Suggest(_pickCharacterCard, _pickRoomCard, _pickWeaponCard);
        OnStartSuggest?.Invoke(_suggest, this);
        int _askIndex = CE_GameManager.Instance.CurrentCharacterTurnIndex;
        Debug.Log($"{characterRef.ColorName} is suggesting {_suggest.Room.Name} with {_suggest.Weapon.Name} at {_suggest.Character.Name}");
        while(isInRoom && currentAIPhase == AIPhase.Suggest)
        {
            _askIndex++;
            _askIndex = _askIndex > CE_GameManager.Instance.AllCharacterInGame.Count - 1 ? 0 : _askIndex;
            IGamePlayable _askTo = CE_GameManager.Instance.AllCharacterInGame[_askIndex];
            CE_Card _result = _askTo.HandCards.GetSuggestCard(_suggest);
            OnSuggestProgress?.Invoke(_suggest, this, _askTo, _result);
            Debug.Log($"{_askTo.CharacterRef.ColorName} {(_result == null ? "can't" : "can")} answer.");
            if(_result != null)
            {
                NoteSystem.MatchCard(_result.ID);
                currentAIPhase = AIPhase.Idle;
            }
            if(CE_GameManager.Instance.CurrentCharacterTurnIndex == _askIndex)
            {
                CE_GameManager.Instance.EndGame();
                currentAIPhase = AIPhase.Idle;
            }
            Debug.Log($"{_askTo.CharacterRef.ColorName} {(_result == null ? "can't" : "can")} answer.");
            yield return new WaitForSeconds(1);
        }

        nextRoomInvestigate = null;
        currentAIPhase = AIPhase.Idle;
        yield return new WaitForSeconds(5);
    }

    CE_Room GetNextRoom()
    {
        List<CE_Note> _nextRoomEmpty = noteSystem.PickRoomNotes(false);
        List<CE_Room> _correctRooms = !lastRoom ? CE_Board.Instance.AllRooms.Where(r => _nextRoomEmpty.Any(n => n.ID == r.Key)).Select(r => r.Value).ToList()
            :
            CE_Board.Instance.AllRooms.Where(r => r.Value != lastRoom && _nextRoomEmpty.Any(n => n.ID == r.Key)).Select(r => r.Value).ToList();
        CE_Room _room = _correctRooms.OrderBy(r => Vector3.Distance(r.Position, transform.position)).FirstOrDefault();
        return _room;
    }

    CE_Door GetNextDoor()
    {
        if (!nextRoomInvestigate) return null;
        CE_Door _door = nextRoomInvestigate.GetNearestDoor(this);
        CE_Door _outDoor = null;
        if (IsInRoom)
        {
            _outDoor = lastRoom.GetNearestDoor(this);
            transform.position = _outDoor.Position;
            isInRoom = false;
        }
        lastRoom = nextRoomInvestigate;
        return _door;
    }

    void LightFeedBack(bool _enable)
    {
        if (lightFeedBack)
            lightFeedBack.enabled = _enable;
    }
    #endregion
    #region Public
    public void Init(CE_GameBoadCharacter _character)
    {
        characterRef = _character;
        transform.position = CE_Board.Instance.GetStartPos(CharacterRef.Character).position;
        name = $"{_character.Character} [IA]";
        Select(false);
        lightFeedBack = GetComponentInChildren<Light>();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = characterRef.CharacterColor;
        Gizmos.DrawSphere(transform.position + Vector3.up * 10, 1);
        if(nextRoomInvestigate)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(nextRoomInvestigate.Position, 2);
            Gizmos.DrawLine(transform.position, nextRoomInvestigate.Position);
        }
        if(nextDoorTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(nextDoorTarget.Position, 2);
            Gizmos.DrawLine(transform.position, nextDoorTarget.Position);
        }
    }

    public void OnStart(IGamePlayable _gamePlayable, int _dice)
    {
        if (_gamePlayable.CharacterRef != characterRef)
            return;
        diceCount = _dice;
        OnStartTurn?.Invoke();
        Select(true);
        StartCoroutine(AIFSM());

    }

    public void Select(bool _isSelected)
    {
        OnSelectPlayable?.Invoke(_isSelected);
    }

    public void StartNavigation()
    {
        throw new NotImplementedException();
    }

    public void SetNavigation(CE_Cell _cell, bool _action)
    {
        iaNavigation.ComputePath(CE_Board.Instance, CE_Board.Instance.GetNearestCell(transform.position), _cell);
        stepCount = 0;
        IsMoving = true;
    }
    #endregion
    #endregion
}

public enum AIPhase
{
    Idle,
    RollDice,
    FindTarget,
    RoomEnterThink,
    Move,
    Suggest
}