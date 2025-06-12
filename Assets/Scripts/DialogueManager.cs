using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string speakerName;
        public Sprite speakerImage;
        [TextArea(2, 4)] public string text;
    }

    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    public float baseTypingSpeed = 0.05f; // Tempo entre letras
    public float acceleratedTypingSpeed = 0.01f; // Quando o botão estiver pressionado

    private Queue<DialogueLine> _dialogueQueue = new Queue<DialogueLine>();
    private Coroutine _typingCoroutine;
    private bool _isTyping = false;
    private bool _isDialogueActive = false;

    void Update()
    {

        if (!_isTyping && _dialogueQueue.Count == 0 && Input.GetMouseButton(0))
        {
            EndDialogue();
            return;
        }
        
        if (_isDialogueActive && Input.GetMouseButton(0))
        {
            // Avança diálogo com clique
            if (!_isTyping)
            {
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue(List<DialogueLine> dialogueLines)
    {
        _dialogueQueue.Clear();
        foreach (DialogueLine line in dialogueLines)
        {
            _dialogueQueue.Enqueue(line);
        }

        dialoguePanel.SetActive(true);
        _isDialogueActive = true;

        DisplayNextLine();
    }
    
    private Color GetSpeakerColor(string speakerName)
    {
        switch (speakerName)
        {
            case "Helena":
                return new Color32(0, 181, 255, 255); // Azul claro
            case "Elisa":
                return new Color32(178, 0, 255, 255); // Roxo
            default:
                return Color.white;
        }
    }

    void DisplayNextLine()
    {
        DialogueLine line = _dialogueQueue.Dequeue();

        // Atualiza UI
        nameText.text = line.speakerName;
        portraitImage.sprite = line.speakerImage;

        Color speakerColor = GetSpeakerColor(line.speakerName);

        // Muda a cor dos elementos visuais
        Transform headerTransform = dialoguePanel.transform.Find("Header");
        Transform bodyTransform = dialoguePanel.transform.Find("Body");

        if (headerTransform != null)
            headerTransform.GetComponent<Image>().color = speakerColor;

        if (bodyTransform != null)
            bodyTransform.GetComponent<Image>().color = speakerColor;

        dialogueText.color = speakerColor;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypeSentence(line.text));
    }


    IEnumerator TypeSentence(string sentence)
    {
        _isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;

            // Verifica se o botão está pressionado para acelerar
            float delay = Input.GetMouseButton(0) ? acceleratedTypingSpeed : baseTypingSpeed;
            yield return new WaitForSeconds(delay);
        }

        _isTyping = false;
    }

    void EndDialogue()
    {
        _isDialogueActive = false;
        dialoguePanel.SetActive(false);
    }
    
    public bool IsFinished()
    {
        return !_isDialogueActive;
    }

}
