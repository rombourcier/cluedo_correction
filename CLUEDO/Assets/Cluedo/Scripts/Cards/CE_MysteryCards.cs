using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CE_MysteryCards
{
    #region Events
    #endregion

    #region Members
    #region Private
    [SerializeField] CE_Card firstCard, secondCard, thirdCard;
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public CE_Card FirstCard => firstCard;
    public CE_Card SecondCard => secondCard;
    public CE_Card ThirdCard => thirdCard;
    #endregion

    #region Methods
    #region Private
    #endregion
    #region Public
    public CE_MysteryCards(CE_Card _first, CE_Card _second, CE_Card _third)
    {
        firstCard = _first;
        secondCard = _second;
        thirdCard = _third;
    }
    #endregion
    #endregion
}