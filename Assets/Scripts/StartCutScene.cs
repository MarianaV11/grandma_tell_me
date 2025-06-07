using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class StartCutScene : MonoBehaviour
{
    public GameObject personagem1Prefab;
    public GameObject personagem2Prefab;

    public Vector2 posInicial1 = new Vector2(-1.5f, -0.5f);
    public Vector2 posInicial2 = new Vector2(-5.5f, 4);

    public Vector2 destino1 = new Vector2(-1.5f, 2);
    public Vector2 destino2 = new Vector2(-1.5f, 2.5f);

    public float moveSpeed = 5f;

    private GameObject personagem1;
    private GameObject personagem2;

    [SerializeField] private string personagem1Name;
    [SerializeField] private string personagem2Name;

    private CharacterMove mover1;
    private CharacterMove mover2;

    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private Sprite personagem1Foto;
    [SerializeField] private Sprite personagem2Foto;

    private void Start()
    {
        personagem1 = Instantiate(personagem1Prefab, posInicial1, Quaternion.identity);
        personagem2 = Instantiate(personagem2Prefab, posInicial2, Quaternion.identity);

        mover1 = new CharacterMove(personagem1, moveSpeed);
        mover2 = new CharacterMove(personagem2, moveSpeed);

        SceneManager.sceneUnloaded += OnSceneUnloaded;

        StartCoroutine(ControlarCutscene());
    }

    private IEnumerator EsperarDialogoTerminar()
    {
        while (dialogueManager.IsFinished() || !Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
    }

    private IEnumerator ControlarCutscene()
    {
        yield return new WaitForSeconds(1f);

        var falas = new List<DialogueManager.DialogueLine>
        {
            new DialogueManager.DialogueLine
            {
                speakerName = personagem1Name,
                speakerImage = personagem1Foto,
                text = "Ahhh finalmente cheguei!!!"
            }
        };
        dialogueManager.StartDialogue(falas);
        yield return EsperarDialogoTerminar();

        falas = new List<DialogueManager.DialogueLine>
        {
            new DialogueManager.DialogueLine
            {
                speakerName = personagem1Name,
                speakerImage = personagem1Foto,
                text = "Essa casa tem um cheiro que me faz lembrar da infância... Canela e saudade."
            }
        };
        dialogueManager.StartDialogue(falas);

        bool p1chegou1 = false;
        while (!p1chegou1)
        {
            p1chegou1 = mover1.MoverPara(destino1, Time.fixedDeltaTime);
            yield return null;
        }

        yield return EsperarDialogoTerminar();

        falas = new List<DialogueManager.DialogueLine>
        {
            new DialogueManager.DialogueLine
            {
                speakerName = personagem2Name,
                speakerImage = personagem2Foto,
                text = "Ah Helena que bom que você chegou!"
            }
        };
        dialogueManager.StartDialogue(falas);
        yield return EsperarDialogoTerminar();

        bool p2chegou1 = false;
        while (!p2chegou1)
        {
            p2chegou1 = mover2.MoverPara(new Vector2(-1.5f, 4), Time.fixedDeltaTime);
            yield return null;
        }

        bool p2chegou2 = false;
        while (!p2chegou2)
        {
            p2chegou2 = mover2.MoverPara(destino2, Time.fixedDeltaTime);
            yield return null;
        }

        falas = new List<DialogueManager.DialogueLine>
        {
            new DialogueManager.DialogueLine
            {
                speakerName = personagem2Name,
                speakerImage = personagem2Foto,
                text = "Ah, minha querida... \nEsta casa guarda muitas memórias."
            },
            new DialogueManager.DialogueLine
            {
                speakerName = personagem2Name,
                speakerImage = personagem2Foto,
                text = "Algumas... estão começando a escapar de mim."
            },
            new DialogueManager.DialogueLine
            {
                speakerName = personagem2Name,
                speakerImage = personagem2Foto,
                text = "Tome... \nEssa aqui é um retrato que há muito tempo foi tirado, nela aparecem pessoas muito importantes para mim\nE para você também, pequena..."
            }
        };
        dialogueManager.StartDialogue(falas);
        yield return EsperarDialogoTerminar();

        // Armazena a cena atual antes de carregar a nova
        Scene cenaAnterior = SceneManager.GetActiveScene();

        // Carrega a nova cena aditivamente
        SceneManager.LoadScene("Scenes/PuzzleTransition", LoadSceneMode.Additive);
        yield return null;

        // Define nova cena como ativa
        Scene cenaNova = SceneManager.GetSceneByName("PuzzleTransition");
        SceneManager.SetActiveScene(cenaNova);
        yield return null;

        // Posiciona GameHolder, se necessário
        GameObject gameHolder = GameObject.Find("GameHolder");
        if (gameHolder != null)
        {
            gameHolder.transform.position = new Vector3(0f, 0f, -1f);
        }

        // Desativa colisores da cena anterior
        if (cenaAnterior.IsValid())
        {
            SetColisoresDaCena(cenaAnterior, false);
        }
        yield return null;
        Debug.Log("oiiiiiii");
    }


    // Desativa ou reativa coliders e scripts interativos
    private void SetColisoresDaCena(Scene cena, bool ativo)
    {
        foreach (GameObject obj in cena.GetRootGameObjects())
        {
            foreach (var tilemapCollider in obj.GetComponentsInChildren<TilemapCollider2D>(true))
            {
                tilemapCollider.enabled = ativo;
            }

            foreach (var boxCollider in obj.GetComponentsInChildren<BoxCollider2D>(true))
            {
                boxCollider.enabled = ativo;
            }

            foreach (var script in obj.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (script.GetType().Name.Contains("Move") || script.GetType().Name.Contains("Controller"))
                {
                    script.enabled = ativo;
                }
            }
        }
    }

    // Detecta descarregamento da cena PuzzleTransition para reativar
    private void OnSceneUnloaded(Scene unloadedScene)
    {
        if (unloadedScene.name == "PuzzleTransition")
        {
            Scene cenaAnterior = SceneManager.GetSceneByName("startTransition_");
            if (cenaAnterior.IsValid())
            {
                SetColisoresDaCena(cenaAnterior, true);
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void IniciarDialogoPosPuzzle()
    {
        dialogueManager.dialoguePanel.SetActive(true);
        StartCoroutine(DialogoDepoisDoPuzzle());
    }

    private IEnumerator DialogoDepoisDoPuzzle()
    {
        yield return new WaitForSeconds(0.5f);

        var falasPosPuzzle = new List<DialogueManager.DialogueLine>
        {
            new DialogueManager.DialogueLine
            {
                speakerName = personagem1Name,
                speakerImage = personagem1Foto,
                text = "E esse aqui? Por que o rosto está borrado?"
            },
            new DialogueManager.DialogueLine
            {
                speakerName = personagem2Name,
                speakerImage = personagem2Foto,
                text = "Eu não lembro... Mas ele era importante. Muito"
            }
        };

        dialogueManager.StartDialogue(falasPosPuzzle);
        yield return EsperarDialogoTerminar();
    }
}



