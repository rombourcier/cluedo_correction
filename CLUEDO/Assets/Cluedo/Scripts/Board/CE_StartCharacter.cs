using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CE_StartCharacter
{
    #region Events
    #endregion

    #region Members
    #region Private
    [SerializeField] Transform startCell;
    [SerializeField] CE_GameCharacter character;
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public Transform StartCell => startCell;
    public CE_GameCharacter Character => character;
    #endregion

    #region Methods
    #region Private
    #endregion
    #region Public
    #endregion
    #endregion
}