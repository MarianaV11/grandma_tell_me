using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class StartCutScene : MonoBehaviour
{
    [Header("Personagens")]
    public GameObject personagem1Prefab;
    public GameObject personagem2Prefab;
    [SerializeField] private string personagem1Name;
    [SerializeField] private string personagem2Name;
    [SerializeField] private Sprite personagem1Foto;
    [SerializeField] private Sprite personagem2Foto;

    [Header("Configuração da Cena")]
    public GameObject Puzzle;
    public float moveSpeed = 5f;
    public Vector2 posInicial1 = new Vector2(-1.5f, -0.5f);
    public Vector2 posInicial2 = new Vector2(-5.5f, 4);

    [SerializeField] private DialogueManager dialogueManager;

    private GameObject personagem1;
    private GranddaughterController granddaughterController;
    private GameObject personagem2;
    private CharacterMove mover1;
    private CharacterMove mover2;

    private void Start()
    {
        personagem1 = Instantiate(personagem1Prefab, posInicial1, Quaternion.identity);
        granddaughterController = personagem1.GetComponent<GranddaughterController>();
        granddaughterController.enabled = false;
        personagem2 = Instantiate(personagem2Prefab, posInicial2, Quaternion.identity);
        mover1 = new CharacterMove(personagem1, moveSpeed);
        mover2 = new CharacterMove(personagem2, moveSpeed);

        StartCoroutine(RodarCutscene());
    }

    private IEnumerator RodarCutscene()
    {
        yield return new WaitForSeconds(1f);
        /*
        yield return MostrarDialogo(personagem1Name, personagem1Foto, "Ahhh finalmente cheguei!!!");
        yield return MostrarDialogo(personagem1Name, personagem1Foto, "Essa casa tem um cheiro que me faz lembrar da infância... Canela e saudade.");
        */
        yield return MoverPersonagem(mover1, new Vector2(-1.5f, 2));
        // yield return MostrarDialogo(personagem2Name, personagem2Foto, "Ah Helena que bom que você chegou!");

        yield return MoverPersonagem(mover2, new Vector2(-1.5f, 4));
        yield return MoverPersonagem(mover2, new Vector2(-1.5f, 2.5f));
        
        /*
        yield return MostrarDialogos(new List<string>
        {
            "Ah, minha querida... \nEsta casa guarda muitas memórias.",
            "Algumas... estão começando a escapar de mim.",
            "Tome... \nEssa aqui é um retrato que há muito tempo foi tirado!",
            "nela aparecem pessoas muito importantes para mim\nE para você também, pequena..."
        }, personagem2Name, personagem2Foto);

        yield return IniciarPuzzle();

        yield return MostrarDialogos(new List<string>
        {
            "Que legal!!\nMas quem é essa pessoa com o rosto borrado?",
        }, personagem1Name, personagem1Foto);

        yield return MostrarDialogos(new List<string>
        {
            "Eu não me lembro...",
            "Mas ele era importante.",
            "Muito..."
        }, personagem2Name, personagem2Foto);

        yield return MostrarDialogo(personagem1Name, personagem1Foto, "Tudo bem vovó, eu vou te ajudar a descobrir\nquem é essa pessoa!");
        */
        yield return MoverPersonagem(mover2, new Vector2(-1.5f, 4.6f));
        yield return MoverPersonagem(mover2, new Vector2(0.5f,4.6f));
        Debug.Log("idle_down");
        mover2.setAnimation("idle_down");
    
        granddaughterController.enabled = true;
    }

    private IEnumerator MostrarDialogo(string nome, Sprite imagem, string texto)
    {
        dialogueManager.StartDialogue(new List<DialogueManager.DialogueLine>
        {
            new DialogueManager.DialogueLine { speakerName = nome, speakerImage = imagem, text = texto }
        });
        yield return EsperarDialogoTerminar();
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
        while (dialogueManager.IsFinished() || !Input.GetMouseButtonDown(0))
            yield return null;
    }

    private IEnumerator MoverPersonagem(CharacterMove mover, Vector2 destino)
    {
        bool chegou = false;
        while (!chegou)
        {
            chegou = mover.MoverPara(destino, Time.fixedDeltaTime);
            yield return null;
        }
    }

    private IEnumerator IniciarPuzzle()
    {
        var cenaAtual = SceneManager.GetActiveScene();
        if (cenaAtual.IsValid())
            SetColisoresDaCena(cenaAtual, false);

        Puzzle.SetActive(true);
        Puzzle.transform.position = personagem1.transform.position;

        bool puzzleFinalizado = false;
        var puzzleManager = Puzzle.GetComponentInChildren<GameManager>();
        puzzleManager.OnGameFinished += () => puzzleFinalizado = true;

        while (!puzzleFinalizado)
            yield return null;

        yield return new WaitForSeconds(1f);
        Puzzle.SetActive(false);
        
        if (cenaAtual.IsValid())
            SetColisoresDaCena(cenaAtual, true);

    }

    private void SetColisoresDaCena(Scene cena, bool ativo)
    {
        foreach (GameObject obj in cena.GetRootGameObjects())
        {
            foreach (var tilemapCollider in obj.GetComponentsInChildren<TilemapCollider2D>(true))
                tilemapCollider.enabled = ativo;

            foreach (var boxCollider in obj.GetComponentsInChildren<BoxCollider2D>(true))
                boxCollider.enabled = ativo;

            foreach (var script in obj.GetComponentsInChildren<MonoBehaviour>(true))
            {
                string nomeScript = script.GetType().Name;
                if (nomeScript.Contains("Move") || nomeScript.Contains("Controller"))
                    script.enabled = ativo;
            }
        }
    }
}
