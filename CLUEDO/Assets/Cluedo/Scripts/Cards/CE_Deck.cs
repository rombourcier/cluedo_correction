using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CE_Deck : MonoBehaviour
{
    #region Events
    #endregion

    #region Members
    #region Private
    CE_CardsDB gameCardsDB;
    [SerializeField] List<CE_Card> availableCards = new List<CE_Card>();
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public bool IsReady { get; private set; } = false;
    public int DeckCount => availableCards.Count;
    public CE_Card[] AllCardsDB => gameCardsDB.AllCards;
    public CE_Card GetCard(int _id) => gameCardsDB.AllCards.Where(c => c.ID == _id).FirstOrDefault();
    #endregion

    #region Methods
    #region Private
    private void Start()
    {
        LoadDB();
    }

    void LoadDB()
    {
        TextAsset _data = Resources.Load<TextAsset>("Data/Cards");
        if (!_data) return;
        gameCardsDB = JsonUtility.FromJson<CE_CardsDB>(_data.text);
        if (!gameCardsDB.IsValid) return;
        Sprite[] _rooms = Resources.LoadAll<Sprite>("UI/room_cards");
        Sprite[] _characters = Resources.LoadAll<Sprite>("UI/suspect_cards");
        Sprite[] _weapons = Resources.LoadAll<Sprite>("UI/weapons_cards");
        ApplyPictureData(_rooms, CardType.Room, gameCardsDB);
        ApplyPictureData(_characters, CardType.Character, gameCardsDB);
        ApplyPictureData(_weapons, CardType.Weapon, gameCardsDB);
        availableCards = new List<CE_Card>(gameCardsDB.AllCards);
        IsReady = true;
    }

    void ApplyPictureData(Sprite[] _sprite, CardType _type, CE_CardsDB _db)
    {
        if (_sprite == null) return;
        List<CE_Card> _cards = _db.AllCards.ToList().Where(c => c.Type == _type).ToList();
        _cards.ForEach(c => c.SetSprite(_sprite));
    }
    #endregion
    #region Public
    public CE_Card PickRandomCard(CardType _type)
    {
        List<CE_Card> _cards = availableCards.Where(c => c.Type == _type).ToList();
        if (_cards == null) return null;
        int _randomIndex = UnityEngine.Random.Range(0, _cards.Count);
        CE_Card _card = _cards[_randomIndex];
        availableCards.Remove(_card);
        return _card;
    }

    CE_Card PickRandomCard()
    {
        if (!gameCardsDB.IsValid) return null;
        int _randomIndex = UnityEngine.Random.Range(0, availableCards.Count);
        CE_Card _card = availableCards[_randomIndex];
        availableCards.Remove(_card);
        return _card;
    }


    public void GiveCard(IGamePlayable _character)
    {
        CE_Card _card = PickRandomCard();
        _character.HandCards.AddCard(_card);
        _character.NoteSystem.MatchCard(_card.ID);
    }
    #endregion
    #endregion
}

public struct CE_CardsDB
{
    [SerializeField] CE_Card[] Cards;
    public CE_Card[] AllCards => Cards;
    public bool IsValid => Cards != null;
}