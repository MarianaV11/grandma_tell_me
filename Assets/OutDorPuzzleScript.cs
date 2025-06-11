using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class OutDorPuzzleScript : MonoBehaviour
{
    private bool isPlayerNearby = false;
    public GameObject interactionHint;
    
    [Header("Dialogue Interation Rules")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string foundMessage = "O que é isso pendurado na porta??";
    [SerializeField] private string speakerName = "Helena";
    [SerializeField] private Sprite speakerImage;

    [Header("Action Definition")]
    [SerializeField] private bool isHouseByLake = false;

    [Header("Game")]
    [SerializeField] private GameObject memoryGameObject;
    
    [SerializeField] private GameObject imageDisplayPanel; // o panel que você criou
    [SerializeField] private RectTransform imageRectTransform; // o objeto Image dentro do panel
    private bool isImageDisplayed = false;

    void Start()
    {
        if (interactionHint != null)
            interactionHint.SetActive(false);
        
        CardsController cards = memoryGameObject.GetComponentInChildren<CardsController>();
        if (cards != null)
        {
            cards.OnGameFinished += OnMemoryGameFinished;
            Debug.Log("Listener inscrito no evento OnGameFinished");
        }
        else
        {
            Debug.LogError("MemoryGame não encontrado no GameObject ou filhos!");
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FoundMessage());
        }
        
        if (isImageDisplayed && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            if (!RectTransformUtility.RectangleContainsScreenPoint(imageRectTransform, mousePos))
            {
                imageDisplayPanel.SetActive(false);
                isImageDisplayed = false;
                StartCoroutine(ImageMessage());
            }
        }
    }
    
    private IEnumerator FoundMessage()
    {
        List<string> mensagens = new List<string>();
        if (isHouseByLake)
        {
            mensagens.Add(foundMessage);
            mensagens.Add("Uma foto!!!");
            mensagens.Add("Mas ela está cheia de poeira \n Preciso chegar mais perto para ver o que é...");
            
            memoryGameObject.gameObject.SetActive(true);

        }

        yield return ShowMessages(mensagens, speakerName, speakerImage);
    }
    
    private IEnumerator ImageMessage()
    {
        List<string> mensagens = new List<string>();
        
        mensagens.Add("É o meu irmão... Como eu não lembrava disso?");
        mensagens.Add("Se ele é meu irmão...");
        mensagens.Add("E também é irmão da vó...");
        mensagens.Add("Então... eu sou...");
        
        yield return ShowMessages(mensagens, speakerName, speakerImage);
    }
    
    private IEnumerator ShowMessages(List<string> textos, string nome, Sprite imagem)
    {
        var falas = new List<DialogueManager.DialogueLine>();
        foreach (var texto in textos)
        {
            falas.Add(new DialogueManager.DialogueLine
            {
                speakerName = nome,
                speakerImage = imagem,
                text = texto
            });
        }

        dialogueManager.StartDialogue(falas);
        yield return FoundMessageTerminate();
    }
    
    private IEnumerator FoundMessageTerminate()
    {
        while (!dialogueManager.IsFinished() || !Input.GetMouseButtonDown(0))
            yield return null;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        isPlayerNearby = true;
        if (interactionHint != null)
            interactionHint.SetActive(true);

    }

    void OnTriggerExit2D(Collider2D other)
    {
        isPlayerNearby = false;
        if (interactionHint != null)
            interactionHint.SetActive(false);
    }
    
    private void OnMemoryGameFinished()
    {
        memoryGameObject.gameObject.SetActive(false);
        imageDisplayPanel.SetActive(true);
        isImageDisplayed = true;
    }
}
