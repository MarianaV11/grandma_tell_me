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

    // Ahead is the game config
    //[Header("Game")]
    //[SerializeField] private GameObject memoryGameObject;
    
    //private Memory memoryGame;
    void Start()
    {
        if (interactionHint != null)
            interactionHint.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FoundMessage());
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
            
            //geniusGameObject.gameObject.SetActive(true);

            // Conecta o evento do MemoryGame

        }

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
}
