using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CE_Suggest
{
 	#region Events
	#endregion
	
	#region Members
	#region Private
    
	#endregion
	#region Public
	#endregion
	#endregion

	#region Getters/Setters
    public CE_Card Room { get; private set; }
    public CE_Card Character { get; private set; }
    public CE_Card Weapon { get; private set; }
    #endregion

    #region Methods
    #region Private
    #endregion
    #region Public
    public CE_Suggest(CE_Card _room, CE_Card _character, CE_Card _weapon)
    {
        Room = _room;
        Character = _character;
        Weapon = _weapon;
  
    }

    public void GiveRoom(CE_Card _room) => Room = _room;

    public void GiveCharacter(CE_Card _character) => Character = _character;

    public void GiveWeapon(CE_Card _weapon) => Weapon = _weapon;
    #endregion
    #endregion
}