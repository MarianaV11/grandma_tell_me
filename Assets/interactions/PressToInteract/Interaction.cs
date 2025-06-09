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
    
    [Header("Game")]
    [SerializeField] private GameObject geniusGameObject;
    

    void Start()
    {
        if (interactionHint != null)
            interactionHint.SetActive(false);
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
        List<string> mensagens = new List<string>();
        if (isFinded)
        {
            mensagens.Add(foundMessage);
            mensagens.Add("Uma caixa de memórias?");
            mensagens.Add("Mas esta trancado, por que?\nA tranca não parece dificil de abrir.");
            geniusGameObject.gameObject.SetActive(true);
        }
        else
        {
            mensagens.Add(notFoundMessage);
        }

        yield return MostrarDialogos(mensagens, speakerName, speakerImage);
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
}