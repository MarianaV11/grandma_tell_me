using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GeniusGame : MonoBehaviour
{
    [Header("Botões")]
    public Button[] colorButtons; // 0 = verde, 1 = vermelho, 2 = azul, 3 = amarelo

    [Header("Configuração de Dificuldade")]
    public bool usarDificuldadeManual = false;
    public Dificuldade dificuldadeSelecionada = Dificuldade.Facil;

    [Header("Controle de Jogo")]
    private List<int> sequence = new List<int>();
    private List<int> playerInput = new List<int>();
    private bool inputEnabled = false;
    private int rodadaAtual = 0;
    private int maxRodadas = 10;
    private float tempoPiscar = 0.6f;
    private float intervaloPiscar = 0.5f;

    [Header("UI")]
    public TextMeshProUGUI rodadaLabel;

    [Header("Elementos de Finalização")]
    public GameObject cadeadoFechado;
    public GameObject cadeadoAberto;
    public GameObject buttonContainer;

    private CanvasGroup cadeadoFechadoGroup;
    private CanvasGroup buttonContainerGroup;

    [Header("Áudio")]
    public AudioClip somVerde;
    public AudioClip somVermelho;
    public AudioClip somAzul;
    public AudioClip somAmarelo;
    public AudioClip somErro;
    private AudioSource audioSource;
    
    
    public event System.Action OnGameFinished;

    void Start()
    {
        Dificuldade modo = usarDificuldadeManual ? dificuldadeSelecionada : ModoDificuldadeSelecionado.dificuldade;
        DefinirDificuldade(modo);

        audioSource = GetComponent<AudioSource>();

        cadeadoFechadoGroup = cadeadoFechado.GetComponent<CanvasGroup>();
        cadeadoFechadoGroup.alpha = 1f; // Cadeado fechado visível
        cadeadoFechadoGroup.interactable = false;
        cadeadoFechadoGroup.blocksRaycasts = false;

        buttonContainerGroup = buttonContainer.GetComponent<CanvasGroup>();
        buttonContainerGroup.alpha = 1f;

        cadeadoFechado.SetActive(true);     
        cadeadoAberto.SetActive(false);     // Inicia fechado

        StartCoroutine(StartSequence());
    }


    void DefinirDificuldade(Dificuldade modo)
    {
        switch (modo)
        {
            case Dificuldade.Facil:
                maxRodadas = 3;
                tempoPiscar = 0.6f;
                intervaloPiscar = 0.5f;
                break;
            case Dificuldade.Medio:
                maxRodadas = 5;
                tempoPiscar = 0.6f;
                intervaloPiscar = 0.5f;
                break;
            case Dificuldade.Dificil:
                maxRodadas = 8;
                tempoPiscar = 0.3f;
                intervaloPiscar = 0.2f;
                break;
        }
    }

    IEnumerator StartSequence()
    {
        if (rodadaAtual >= maxRodadas)
        {
            Debug.Log("Vitória! Fim do jogo.");
            StartCoroutine(FinalizarGenius());
            yield break;
        }

        inputEnabled = false;
        playerInput.Clear();
        rodadaAtual++;
        rodadaLabel.text = "Rodada: " + rodadaAtual;

        yield return new WaitForSeconds(0.8f);

        int next = Random.Range(0, colorButtons.Length);
        sequence.Add(next);

        for (int i = 0; i < sequence.Count; i++)
        {
            int index = sequence[i];
            yield return StartCoroutine(FlashButton(index));
            yield return new WaitForSeconds(intervaloPiscar);
        }

        inputEnabled = true;
    }

    IEnumerator FlashButton(int index)
    {
        Button btn = colorButtons[index];
        Image img = btn.GetComponent<Image>();
        Color originalColor = img.color;

        PlayCorSom(index);

        img.color = Color.white;
        yield return new WaitForSeconds(tempoPiscar);
        img.color = originalColor;
    }

    IEnumerator FlashOnClick(int index)
    {
        Button btn = colorButtons[index];
        Image img = btn.GetComponent<Image>();
        Color originalColor = img.color;

        PlayCorSom(index);

        img.color = Color.gray;
        yield return new WaitForSeconds(0.2f);
        img.color = originalColor;
    }

    public void OnColorButtonClicked(int index)
    {
        if (!inputEnabled) return;

        playerInput.Add(index);
        StartCoroutine(FlashOnClick(index));

        int currentStep = playerInput.Count - 1;

        if (playerInput[currentStep] != sequence[currentStep])
        {
            Debug.Log("Errou! Reiniciando...");
            if (somErro != null && audioSource != null)
                audioSource.PlayOneShot(somErro);

            inputEnabled = false;
            StartCoroutine(ErrorFeedback());
            return;
        }

        if (playerInput.Count == sequence.Count)
        {
            Debug.Log("Acertou a sequência!");
            StartCoroutine(StartSequence());
        }
    }

    IEnumerator ErrorFeedback()
    {
        foreach (Button btn in colorButtons)
            btn.GetComponent<Image>().color = Color.red;

        yield return new WaitForSeconds(0.6f);

        for (int i = 0; i < colorButtons.Length; i++)
            colorButtons[i].GetComponent<Image>().color = GetColorByIndex(i);

        sequence.Clear();
        rodadaAtual = 0;
        rodadaLabel.text = "Rodada: 0";

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(StartSequence());
    }

    Color GetColorByIndex(int index)
    {
        switch (index)
        {
            case 0: return Color.green;
            case 1: return Color.red;
            case 2: return Color.blue;
            case 3: return Color.yellow;
            default: return Color.white;
        }
    }

    void PlayCorSom(int index)
    {
        if (audioSource == null) return;

        switch (index)
        {
            case 0: if (somVerde != null) audioSource.PlayOneShot(somVerde); break;
            case 1: if (somVermelho != null) audioSource.PlayOneShot(somVermelho); break;
            case 2: if (somAzul != null) audioSource.PlayOneShot(somAzul); break;
            case 3: if (somAmarelo != null) audioSource.PlayOneShot(somAmarelo); break;
        }
    }

    IEnumerator FinalizarGenius()
    {
        Debug.Log("Finalizing Genius Game"); 
        inputEnabled = false;
        rodadaLabel.gameObject.SetActive(false);

        // Começa fadeOut do cadeadoFechado e buttonContainer
        yield return StartCoroutine(FadeTransitionDual(cadeadoFechado, cadeadoAberto, fadeOutFechado: true));
        
        OnGameFinished?.Invoke();
    }

    
    IEnumerator FadeTransitionDual(GameObject cadeadoFechado, GameObject cadeadoAberto, bool fadeOutFechado)
    {
        float duration = 1f;
        float elapsed = 0f;

        // Garantir objetos ativos
        cadeadoFechado.SetActive(true);
        cadeadoAberto.SetActive(true);
        buttonContainer.SetActive(true);

        // Obter ou adicionar CanvasGroup
        CanvasGroup grupoFechado = cadeadoFechado.GetComponent<CanvasGroup>() ?? cadeadoFechado.AddComponent<CanvasGroup>();
        CanvasGroup grupoAberto = cadeadoAberto.GetComponent<CanvasGroup>() ?? cadeadoAberto.AddComponent<CanvasGroup>();
        CanvasGroup grupoGenius = buttonContainer.GetComponent<CanvasGroup>() ?? buttonContainer.AddComponent<CanvasGroup>();

        // Setar alphas iniciais
        grupoFechado.alpha = 1f;
        grupoAberto.alpha = 0f;
        grupoGenius.alpha = 1f;

        grupoFechado.interactable = false;
        grupoFechado.blocksRaycasts = false;

        grupoAberto.interactable = false;
        grupoAberto.blocksRaycasts = false;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            grupoFechado.alpha = Mathf.Lerp(1f, 0f, t);
            grupoAberto.alpha = Mathf.Lerp(0f, 1f, t);
            grupoGenius.alpha = Mathf.Lerp(1f, 1f, t); // Se quiser fade do Genius também, troque isso por Lerp(1f, 0f, t)

            yield return null;
        }

        grupoFechado.alpha = 0f;
        grupoAberto.alpha = 1f;

        cadeadoFechado.SetActive(false);
    }



}
