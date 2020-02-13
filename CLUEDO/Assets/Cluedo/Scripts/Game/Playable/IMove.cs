using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
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
    CE_Cell CurrentCell { get; }
    CE_CellNavigation Navigation { get; }
    bool IsMoving { get; }
    bool CanMove { get; }
    int NavigationDiceValue { get; }
    #endregion

    #region Methods
    #region Private
    void StartNavigation();
    void SetNavigation(CE_Cell _cell, bool _action);
    IEnumerator Move();
	#endregion
	#region Public
	#endregion
	#endregion
}