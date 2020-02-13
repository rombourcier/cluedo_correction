using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class CE_NoteSystem
{
    #region Events
    public event Action<List<CE_Note>> OnInitNotes = null;
    public event Action<CE_Note> OnUpdateNotes = null;
    #endregion

    #region Members
    #region Private
    Dictionary<int, CE_Note> allNotesItems = new Dictionary<int, CE_Note>();
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public List<CE_Note> GetNotes => allNotesItems.Select(n => n.Value).ToList();
	#endregion

	#region Methods
	#region Private
	#endregion
	#region Public
    public void AddAllItems(CE_Card[] _cards)
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            CE_Note _note = new CE_Note(_cards[i].ID, _cards[i].Name, _cards[i].Type);
            allNotesItems.Add(_cards[i].ID, _note);
        }
        OnInitNotes?.Invoke(GetNotes);
    }

    public void MatchCard(int _id)
    {
        if(allNotesItems.ContainsKey(_id))
        {
            allNotesItems[_id].IsChecked = true;
            Debug.Log(allNotesItems[_id]);
            OnUpdateNotes?.Invoke(allNotesItems[_id]);
        }
    }

    public List<CE_Note> PickRoomNotes(bool _checked = false) => allNotesItems.Where(n => n.Value.IsChecked == _checked && n.Value.Type == CardType.Room).Select(n => n.Value).ToList();

    public List<CE_Note> PickCharacterNotes(bool _checked = false) => allNotesItems.Where(n => n.Value.IsChecked == _checked && n.Value.Type == CardType.Character).Select(n => n.Value).ToList();

    public List<CE_Note> PickWeaponNotes(bool _checked = false) => allNotesItems.Where(n => n.Value.IsChecked == _checked && n.Value.Type == CardType.Weapon).Select(n => n.Value).ToList();

    public CE_Note PickRandomNotes(CardType _type, bool _checked = false)
    {
        List<CE_Note> _notes = allNotesItems.Where(n => n.Value.IsChecked == _checked && n.Value.Type == _type).Select(n => n.Value).ToList();
        return _notes[UnityEngine.Random.Range(0, _notes.Count)];
    }
    #endregion
    #endregion
}