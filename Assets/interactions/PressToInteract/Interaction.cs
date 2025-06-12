using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class InteractionTrigger : MonoBehaviour
{
    private bool isPlayerNearby = false;
    public GameObject interactionHint;
    
    [Header("Dialogue Interation Rules")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private bool isFinded = false;
    [SerializeField] private string notFoundMessage = "Nada foi encontrado";
    [SerializeField] private string foundMessage = "Ah, mas o que é isso?";
    [SerializeField] private string speakerName = "Helena";
    [SerializeField] private Sprite speakerImage;
    
    [SerializeField] private GameObject doorsInteration;
    
    [Header("Game")]
    [SerializeField] private GameObject geniusGameObject;
    
    private GeniusGame geniusGame;
    private bool isGeniusFinished = false;
    
    void Start()
    {
        if (interactionHint != null)
            interactionHint.SetActive(false);
        
        GeniusGame genius = geniusGameObject.GetComponentInChildren<GeniusGame>();
        if (genius != null)
        {
            genius.OnGameFinished += OnGeniusGameFinished;
            Debug.Log("Listener inscrito no evento OnGameFinished");
        }
        else
        {
            Debug.LogError("GeniusGame não encontrado no GameObject ou filhos!");
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Dialogo());
        }
    }

    private IEnumerator Dialogo()
    {
        if (isFinded)
        {
            List<string> mensagens = new List<string>();
            mensagens.Add(foundMessage);
            mensagens.Add("Uma caixa de memórias?");
            mensagens.Add("Mas esta trancado, por que?\nA tranca não parece dificil de abrir.");
            
            yield return MostrarDialogos(mensagens, speakerName, speakerImage);
            
            geniusGameObject.gameObject.SetActive(true);
            
            // Conecta o evento do Genius
            GeniusGame genius = geniusGameObject.GetComponent<GeniusGame>();
            
            if (genius != null)
            {
                genius.OnGameFinished += OnGeniusGameFinished;
            }
            
            while (!isGeniusFinished)
            {
                yield return null;
            }
            
            List<string> postGameMessages = new List<string>();
            postGameMessages.Add("Nossa um diário antigo que a vovó escrevia!!!");
            postGameMessages.Add("Será que nele eu consigo achar alguma pista sobre quem era aquele menino no retrato com o rosto borrado?");
            postGameMessages.Add("Hummm!!");
            postGameMessages.Add("Nesse desenho que vovó fez quando era pequena tem uma casa no lago, e na porta desse lago parece ter algo.");
            
            postGameMessages.Add("Decido!!\nEu vou lá investigar, mas por onde devo começar?");
            postGameMessages.Add("Acho que primeiro devo proucurar por um lago lá fora!");
        
            yield return MostrarDialogos(postGameMessages, speakerName, speakerImage);
            
            doorsInteration.SetActive(true);
            
        }
        else
        {
            List<string> mensagens = new List<string>();
            mensagens.Add(notFoundMessage);
            yield return MostrarDialogos(mensagens, speakerName, speakerImage);
        }
    }
    
    private IEnumerator MostrarDialogos(List<string> textos, string nome, Sprite imagem)
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
        yield return EsperarDialogoTerminar();
    }

    private IEnumerator EsperarDialogoTerminar()
    {
        while (!dialogueManager.IsFinished() || !Input.GetMouseButtonDown(0))
            yield return null;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            isPlayerNearby = true;
            if (interactionHint != null)
                interactionHint.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            isPlayerNearby = false;
            if (interactionHint != null)
                interactionHint.SetActive(false);
        }
    }
    
    private void OnGeniusGameFinished()
    {
        isGeniusFinished = true;
        geniusGameObject.gameObject.SetActive(false);
    }
}