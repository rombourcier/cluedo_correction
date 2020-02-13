using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CE_Note
{
    #region Events
    #endregion

    #region Members
    #region Private
    [SerializeField] int id = 0;
    [SerializeField] string name = "";
    [SerializeField] bool isChecked = false;
    [SerializeField] CardType type = CardType.Character;
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public int ID { get => id; set => id = value; }
    public string NoteName { get => name; set => name = value; }
    public bool IsChecked { get => isChecked; set => isChecked = value; }
    public CardType Type { get => type; set => type = value; }
	#endregion

	#region Methods
	#region Private
	#endregion
	#region Public
    public CE_Note(int _id, string _name, CardType _type)
    {
        ID = _id;
        NoteName = _name;
        Type = _type;
    }

    public override string ToString()
    {
        return $"{id} {name} {isChecked}";
    }
    #endregion
    #endregion
}