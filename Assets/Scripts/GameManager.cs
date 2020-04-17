using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject cardPrefab;
    public Transform spawnPoint;
    public Transform[] cardZones;
    //public DropZone dropZone;
    public Color[] spiteColors;
    public float cardDelay = 0.4f;
    public int numCards = 9;


    private Player currentTurnPlayer;
    private GameObject[] cards;
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
        currentTurnPlayer = null;
        cards = new GameObject[numCards];
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
                    //Debug.Log("card zone " + i + " , card " + j + " player " + k);

                    cards[cardIndex] = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity) as GameObject;
                    cards[cardIndex].GetComponent<Transform>().SetParent(cardZones[i]);

                    Card card = cards[cardIndex].GetComponent<Card>();
                    card.SetRankAndSpite(Random.Range(1, 10), spiteColors[Random.Range(0, 3)]);

                    if (cardZones[i].name == "BlindZone")
                    {
                        card.BlindCard();
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
