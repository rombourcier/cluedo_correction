using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CE_GameManager : MonoBehaviour
{
    #region Events
    public static event Action OnPlayerReady = null;
    public static event Action<IGamePlayable> OnPlayerInit = null;
    public static event Action<IGamePlayable> OnEndTurn = null;
    public static event Action<IGamePlayable> OnStartTurn = null;
    public static event Action<IGamePlayable, CE_MysteryCards> OnEndGame = null;
    public static Action<IGamePlayable, int> OnDiceRoll = null;
    #endregion

    #region Members
    #region Private
    static CE_GameManager instance = null;
    [SerializeField] List<CE_GameBoadCharacter> allGamePlayable = new List<CE_GameBoadCharacter>();
    [SerializeField] CE_Deck gameDeck = null;
    [SerializeField] CE_MysteryCards mysteryCards;
    [SerializeField, Range(0, 5)] int playerCharacterIndex = 0;
    [SerializeField, Range(2, 6)] int charactersNumber = 4;
    [SerializeField] int currentTurn = 0;
    [SerializeField] int currentCharacterTurn = -1;
    [SerializeField] bool demoMode = false;
    #endregion
    #region Public
    #endregion
    #endregion

    #region Getters/Setters
    public static CE_GameManager Instance => instance;
    public List<IGamePlayable> AllCharacterInGame { get; private set; } = new List<IGamePlayable>();
    public IGamePlayable CurrentCharacterTurn => currentCharacterTurn > -1 ? AllCharacterInGame[currentCharacterTurn] : null;
    public int CurrentCharacterTurnIndex
    {
        get => currentCharacterTurn;
        set
        {
            currentCharacterTurn = value;
            currentCharacterTurn = currentCharacterTurn > AllCharacterInGame.Count - 1 ? 0 : currentCharacterTurn;
        }
    }

    public CE_Deck GameDeck => gameDeck;

    public bool IsValid => gameDeck;
    public bool StartGame { get; private set; } = false;
    #endregion

    #region Methods
    #region Private
    private void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        if (!IsValid) yield break;
        while (!gameDeck.IsReady)
            yield return null;
        yield return StartCoroutine(SetPlayerAndAI());
        yield return StartCoroutine(LoadMysteryCard());
        yield return StartCoroutine(CardDeckShare());
        StartGame = true;
        SetNextTurn();

    }

    IEnumerator SetPlayerAndAI()
    {
        CE_Player _player = allGamePlayable[playerCharacterIndex].CharacterTransform.gameObject.AddComponent<CE_Player>();
        _player.Init(allGamePlayable[playerCharacterIndex]);
        AllCharacterInGame.Add(_player);
        _player.NoteSystem.AddAllItems(gameDeck.AllCardsDB);
        OnPlayerInit?.Invoke(_player);
        yield return new WaitForSeconds(.5f);
        int _count = 0;
        for (int i = 0; i < allGamePlayable.Count; i++)
        {
            if(i != playerCharacterIndex && _count < charactersNumber -1)
            {
                CE_AI _ai = allGamePlayable[i].CharacterTransform.gameObject.AddComponent<CE_AI>();
                _ai.Init(allGamePlayable[i]);
                _ai.NoteSystem.AddAllItems(gameDeck.AllCardsDB);
                AllCharacterInGame.Add(_ai);
                yield return new WaitForSeconds(.5f);
                _count++;
            }
        }
    }

    IEnumerator LoadMysteryCard()
    {
        if (!IsValid) yield break;
        yield return new WaitForSecondsRealtime(1);
        CE_Card _character = gameDeck.PickRandomCard(CardType.Character);
        CE_Card _room = gameDeck.PickRandomCard(CardType.Room);
        CE_Card _weapon = gameDeck.PickRandomCard(CardType.Weapon);
        mysteryCards = new CE_MysteryCards(_character, _room, _weapon);
    }

    IEnumerator CardDeckShare()
    {
        if (!IsValid) yield break;
        int _characterIndex = 0;
        while(gameDeck.DeckCount != 0)
        {
            _characterIndex++;
            _characterIndex = _characterIndex > AllCharacterInGame.Count - 1 ? 0 : _characterIndex;
            IGamePlayable _character = AllCharacterInGame[_characterIndex];
            gameDeck.GiveCard(_character);
            yield return new WaitForEndOfFrame();
            //yield return null;
        }
        OnPlayerReady?.Invoke();
    }

    void SetNextTurn()
    {
        if (!StartGame) return;
        Transform _currentPlayableTransform = null;
        if(CurrentCharacterTurn != null)
        {
            OnEndTurn?.Invoke(CurrentCharacterTurn);
            CurrentCharacterTurn.Select(false);
            CurrentCharacterTurn.OnEndTurn -= SetNextTurn;
        }
        currentTurn++;
        CurrentCharacterTurnIndex++;
        _currentPlayableTransform = CurrentCharacterTurn.CharacterRef.CharacterTransform;
        OnDiceRoll?.Invoke(CurrentCharacterTurn, UnityEngine.Random.Range(2, 13));
        OnStartTurn?.Invoke(CurrentCharacterTurn);
        CurrentCharacterTurn.OnEndTurn += SetNextTurn;
        if (_currentPlayableTransform && _currentPlayableTransform.GetComponent<CE_Player>())
        {
            if (demoMode)
                SetNextTurn();
        }
        else CurrentCharacterTurn.Select(true);
    }

    public void EndGame()
    {
        Debug.Log($"{CurrentCharacterTurn.CharacterRef.ColorName} wins !");
        OnEndGame?.Invoke(CurrentCharacterTurn, mysteryCards);
        CurrentCharacterTurn.OnEndTurn -= SetNextTurn;
        StartGame = false;
    }
	#endregion
	#region Public
	#endregion
	#endregion
}