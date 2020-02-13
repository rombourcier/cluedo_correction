using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CE_Card
{
    #region Events
    #endregion

    #region Members
    #region Private
    [SerializeField] string name = "";
    [SerializeField] int id = 0;
    [SerializeField] CardType type = CardType.Character;
    [SerializeField] Sprite picture = null;
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public int ID => id;
    public string Name => name;
    public CardType Type => type;
    public Sprite Picture => picture;
    #endregion

    #region Methods
    #region Private
    #endregion
    #region Public
    public void SetSprite(Sprite[] _sprites)
    {
        for (int i = 0; i < _sprites.Length; i++)
        {
            if (_sprites[i].name.ToLower() == name.ToLower())
                picture = _sprites[i];
        }
    }
    #endregion
    #endregion
}

public enum CardType
{
    Weapon,
    Character,
    Room,
}
