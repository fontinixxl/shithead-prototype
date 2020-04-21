using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Deck deck;
    public Transform[] cardZones;
    public float cardDelay = 0.4f;

    private Player currentTurnPlayer;
    private List<Player> players;

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
            Debug.Log("Player " + player.name);
            currentTurnPlayer = player;
            yield return StartCoroutine(player.PlayRound());

            if (currentTurnPlayer.HasNoCardsLeft())
            {
                Debug.Log("Player " + currentTurnPlayer.name + " Won!!");
                yield break;
                // TODO: Remove the player from the list of active players as it can't be shitHead
            }
        }

        StartCoroutine(GameLoop());
    }


    private IEnumerator DealCards()
    {
        //Debug.Log("DealCards method()");
        int cardIndex = 0;
        int cardsPerRow = 3;

        for (int i = 0; i < cardZones.Length; i++)
        {
            for (int j = 0; j < cardsPerRow; j++)
            {
                for (int k = 0; k < players.Count; k++)
                {

                    Card card = deck.DrawCard();
                    card.GetComponent<Transform>().SetParent(cardZones[i]);

                    // Cards are already blind in the Deck
                    if (cardZones[i].name != "BlindZone")
                    {
                        card.FaceUpCard();
                    }

                    yield return new WaitForSeconds(cardDelay);
                    cardIndex++;
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
