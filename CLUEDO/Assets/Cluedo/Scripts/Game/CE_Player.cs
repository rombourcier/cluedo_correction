using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CE_Player : MonoBehaviour, IGamePlayable
{
    #region Events
    public event Action<bool> OnSelectPlayable =  null;
    public event Action OnStartTurn = null;
    public event Action OnEndTurn = null;
    public event Action<CE_Suggest, IGamePlayable> OnStartSuggest = null;
    public event Action<CE_Suggest, IGamePlayable, IGamePlayable, CE_Card> OnSuggestProgress = null;
    public event Action<CE_Card, IGamePlayable, IGamePlayable> OnSuggestProgressResult = null;
    public event Action<string, List<CE_Note>, Action<CE_Card>> OnSuggestPhase = null;
    public event Action<CE_Cell> OnNewStep = null;
    public event Action<CE_Room, Action> OnCanEnterRoom = null;
    #endregion

    #region Members
    #region Private
    [SerializeField] CE_GameBoadCharacter characterRef = null;
    [SerializeField] CE_HandCards handCards = new CE_HandCards();
    [SerializeField] Light lightFeedBack = null;
    [SerializeField] CE_NoteSystem noteSystem = new CE_NoteSystem();
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public CE_GameBoadCharacter CharacterRef => characterRef;

    public CE_Cell StartCell => throw new NotImplementedException();

    public CE_HandCards HandCards => handCards;

    public CE_Room NextRoomInvestigate => throw new NotImplementedException();

    public CE_Room LastRoom => throw new NotImplementedException();

    public CE_Door NextDoorTarget => throw new NotImplementedException();

    public bool IsInRoom => throw new NotImplementedException();

    public CE_NoteSystem NoteSystem => noteSystem;
    #endregion

    #region Methods
    #region Private
    #endregion
    #region Public
    private void Awake()
    {
        CE_GameManager.OnDiceRoll += OnStart;
        OnSelectPlayable += LightFeedBack;
    }
    public void Init(CE_GameBoadCharacter _character)
    {
        characterRef = _character;
        transform.position = CE_Board.Instance.GetStartPos(CharacterRef.Character).position;
        name = $"{_character.Character} [PLAYER]";
        Select(false);
        lightFeedBack = GetComponentInChildren<Light>();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = characterRef.CharacterColor;
        Gizmos.DrawSphere(transform.position + Vector3.up * 10, 1);
    }

    public void OnStart(IGamePlayable _gamePlayable, int _dice)
    {
        
    }

    public void Select(bool _isSelected)
    {
        OnSelectPlayable?.Invoke(_isSelected);
    }

    void LightFeedBack(bool _enable)
    {
        if(lightFeedBack)
            lightFeedBack.enabled = _enable;
    }
    #endregion
    #endregion
}