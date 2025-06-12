using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsController : MonoBehaviour
{
    [Header("Prefabs e Layout")]
    [SerializeField] private Card cardPrefab;                  // Prefab da carta
    [SerializeField] private Transform gridTransform;          // Container para as cartas

    [Header("Recursos do Jogo")]
    [SerializeField] private Sprite[] sprites;                 // Sprites disponíveis para formar pares

    [Header("Controles de Interface")]
    [SerializeField] private Button playAgainButton;           // Botão "Jogar Novamente"
    [SerializeField] private Button startButton;               // Botão "Começar Jogo"

    private List<Sprite> spritePairs;                          // Lista embaralhada de sprites duplicados
    private Card firstSelected;
    private Card secondSelected;

    private int totalPairs;                                    // Total de pares usados no jogo
    private int matchedPairs;                                  // Quantos pares já foram encontrados
    private int numberOfPairs;                                 // Quantidade de pares escolhida pelo jogador

    [Header("Configuração de Dificuldade")]
    public bool usarDificuldadeManual = false;
    public Dificuldade dificuldadeSelecionada;
    private int difficulty = 2;

    public event System.Action OnGameFinished;
    void Start()
    {
        Dificuldade modo = usarDificuldadeManual ? dificuldadeSelecionada : ModoDificuldadeSelecionado.dificuldade;
        DefinirDificuldade(modo);
        
        // Eventos de clique
        playAgainButton.onClick.AddListener(FinishGame);
        startButton.onClick.AddListener(StartGame);

        // Define o máximo do slider como a quantidade de sprites disponíveis

        // Oculta os botões de reinício até o jogo terminar
        playAgainButton.gameObject.SetActive(false);
    }

    // Inicia o jogo com a dificuldade escolhida
    void StartGame()
    {
        numberOfPairs = difficulty;

        // Esconde controles iniciais
        startButton.gameObject.SetActive(false);

        PrepareSprites();
        CreateCards();
        matchedPairs = 0;
    }

    // Reinicia o jogo e exibe novamente as opções de dificuldade
    public void FinishGame()
    {
        OnGameFinished?.Invoke();
    }

    // Prepara os sprites duplicados e embaralhados
    void PrepareSprites()
    {
        spritePairs = new List<Sprite>();
        int maxPairs = Mathf.Min(numberOfPairs, sprites.Length);

        for (int i = 0; i < maxPairs; i++)
        {
            spritePairs.Add(sprites[i]);
            spritePairs.Add(sprites[i]); // adiciona o par
        }

        ShuffleSprites(spritePairs);
    }

    // Cria as cartas na tela com os sprites embaralhados
    void CreateCards()
    {
        totalPairs = spritePairs.Count / 2;

        for (int i = 0; i < spritePairs.Count; i++)
        {
            Card card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(spritePairs[i]);
            card.controller = this;
        }
    }

    // Ação ao clicar em uma carta
    public void SetSelected(Card card)
    {
        if (!card.isSelected)
        {
            card.Show();

            if (firstSelected == null)
            {
                firstSelected = card;
                return;
            }

            if (secondSelected == null)
            {
                secondSelected = card;
                StartCoroutine(CheckMatching(firstSelected, secondSelected));
                firstSelected = null;
                secondSelected = null;
            }
        }
    }

    // Verifica se duas cartas formam um par
    IEnumerator CheckMatching(Card a, Card b)
    {
        yield return new WaitForSeconds(0.5f);

        if (a.iconSprite == b.iconSprite)
        {
            matchedPairs++;

            // Se todos os pares foram encontrados, exibe botão de reinício
            if (matchedPairs == totalPairs)
            {
                playAgainButton.gameObject.SetActive(true);
            }
        }
        else
        {
            a.Hide();
            b.Hide();
        }
    }

    // Embaralhamento das cartas (Fisher-Yates)
    void ShuffleSprites(List<Sprite> spriteList)
    {
        for (int i = spriteList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (spriteList[i], spriteList[j]) = (spriteList[j], spriteList[i]);
        }
    }
    
    void DefinirDificuldade(Dificuldade modo)
    {
        switch (modo)
        {
            case Dificuldade.Facil:
                difficulty = 3;
                break;
            case Dificuldade.Medio:
                difficulty = 4;
                break;
            case Dificuldade.Dificil:
                difficulty = 5;
                break;
        }
    }
}
