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
    [SerializeField] private Slider difficultySlider;          // Slider para escolher o número de pares

    private List<Sprite> spritePairs;                          // Lista embaralhada de sprites duplicados
    private Card firstSelected;
    private Card secondSelected;

    private int totalPairs;                                    // Total de pares usados no jogo
    private int matchedPairs;                                  // Quantos pares já foram encontrados
    private int numberOfPairs;                                 // Quantidade de pares escolhida pelo jogador

    void Start()
    {
        // Eventos de clique
        playAgainButton.onClick.AddListener(RestartGame);
        startButton.onClick.AddListener(StartGame);

        // Define o máximo do slider como a quantidade de sprites disponíveis
        difficultySlider.maxValue = sprites.Length;
        difficultySlider.minValue = 1;
        difficultySlider.value = 2; // valor padrão

        // Oculta os botões de reinício até o jogo terminar
        playAgainButton.gameObject.SetActive(false);
    }

    // Inicia o jogo com a dificuldade escolhida
    void StartGame()
    {
        numberOfPairs = (int)difficultySlider.value;

        // Esconde controles iniciais
        startButton.gameObject.SetActive(false);
        difficultySlider.gameObject.SetActive(false);

        PrepareSprites();
        CreateCards();
        matchedPairs = 0;
    }

    // Reinicia o jogo e exibe novamente as opções de dificuldade
    public void RestartGame()
    {
        // Remove todas as cartas da cena
        foreach (Transform child in gridTransform)
        {
            Destroy(child.gameObject);
        }

        // Reseta estado
        firstSelected = null;
        secondSelected = null;

        // Mostra novamente controles iniciais
        startButton.gameObject.SetActive(true);
        difficultySlider.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(false);
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
}
