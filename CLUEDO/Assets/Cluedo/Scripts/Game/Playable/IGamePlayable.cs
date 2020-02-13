using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGamePlayable
{
    #region Events
    event Action<bool> OnSelectPlayable;
    event Action OnStartTurn;
    event Action OnEndTurn;
    event Action<CE_Suggest, IGamePlayable> OnStartSuggest;
    event Action<CE_Suggest, IGamePlayable, IGamePlayable, CE_Card> OnSuggestProgress;
    event Action<CE_Card, IGamePlayable, IGamePlayable> OnSuggestProgressResult;
    event Action<string, List<CE_Note>, Action<CE_Card>> OnSuggestPhase;
    event Action<CE_Cell> OnNewStep;
    event Action<CE_Room, Action> OnCanEnterRoom;
    #endregion

    #region Members
    #region Private
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    CE_GameBoadCharacter CharacterRef { get; }
    CE_Cell StartCell { get; }
    CE_NoteSystem NoteSystem { get; }
    CE_HandCards HandCards { get; }
    CE_Room NextRoomInvestigate { get; }
    CE_Room LastRoom { get; }
    CE_Door NextDoorTarget { get; }
    bool IsInRoom { get; }
    #endregion

    #region Methods
    #region Private
    void Init(CE_GameBoadCharacter _character);
    void Select(bool _isSelected);
    void OnStart(IGamePlayable _gamePlayable, int _dice);
    void OnDrawGizmos();
    #endregion
    #region Public
    #endregion
    #endregion
}