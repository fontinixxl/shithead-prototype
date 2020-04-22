using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Deck deck;
    public float cardDelay = 0.4f;
    // TODO: Refactor to count from the playZone Prefab
    public int playZones = 3;       // Total zones to place cards

    private Player currentTurnPlayer;
    private List<Player> players;
    private const int CARDS_PER_ROW = 3;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        players = new List<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {
        deck.Initialize();
        currentTurnPlayer = null;
        StartCoroutine(InitGame());
    }

    private IEnumerator InitGame()
    {
        // Start off by running the "DealCards' coroutine but don't return until it's finished.
        yield return StartCoroutine(DealCards());
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        foreach (Player player in players)
        {
            currentTurnPlayer = player;
            yield return StartCoroutine(player.PlayRound());

            if (currentTurnPlayer.HasNoCardsLeft())
            {
                Debug.Log("Player " + currentTurnPlayer.name + " Won!!");
                yield break;
                // TODO: Remove the player from the list of active players as it can't be shitHead
            }

            // If player has played at least one card and there are cards left on the deck
            if (currentTurnPlayer.HasPlayedCards() && !deck.IsEmpty())
            {
                Card card = deck.Draw();
                card.FaceUpCard();
                yield return new WaitForSeconds(1);
                card.PlaceCardOnZone(currentTurnPlayer.handTransform);
                
            }
        }

        StartCoroutine(GameLoop());
    }


    private IEnumerator DealCards()
    {
        for (int i = (playZones - 1); i >= 0; i--)
        {
            for (int j = 0; j < CARDS_PER_ROW; j++)
            {
                foreach (var player in players)
                {

                    Card card = deck.Draw();
                    card.PlaceCardOnZone(player.layoutZones[i].GetComponent<Transform>());

                    // Cards are already blind in the Deck
                    if (player.layoutZones[i].name != "BlindZone")
                    {
                        card.FaceUpCard();
                    }

                    yield return new WaitForSeconds(cardDelay);
                }
            }
        }
    }

    public Player GetCurrentTurnPlayer()
    {
        return currentTurnPlayer;
    }

    //Call this to add the passed in Player to the List of Player objects.
    public void RegisterNewPlayer(Player player)
    {
        // Add player to List players.
        players.Add(player);
    }
}
