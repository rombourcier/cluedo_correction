using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CE_GameBoadCharacter
{
    #region Events
    #endregion

    #region Members
    #region Private
    [SerializeField] string characterName = "";
    [SerializeField] CE_GameCharacter character = CE_GameCharacter.Green;
    [SerializeField] Transform characterTransform = null;
    [SerializeField] Color characterColor = Color.green;
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    //<balise=todo>valeur</balise>
    //color
    //b bold
    //i italique
    public string ColorName => $"<color=#{ColorUtility.ToHtmlStringRGBA(characterColor)}>{characterName}</color>";
    public Transform CharacterTransform => characterTransform;
    public string CharacterName => characterName;
    public CE_GameCharacter Character => character;
    public Color CharacterColor => characterColor;
    #endregion

    #region Methods
    #region Private
    #endregion
    #region Public
    #endregion
    #endregion
}