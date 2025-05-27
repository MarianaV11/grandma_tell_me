using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    [SerializeField] private String personagem1Name;
    [SerializeField] private String personagem2Name;

    private CharacterMove mover1;
    private CharacterMove mover2;
    
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private Sprite personagem1Foto;
    [SerializeField] private Sprite personagem2Foto;

    private void Start()
    {
        // Instancia os personagens
        personagem1 = Instantiate(personagem1Prefab, posInicial1, Quaternion.identity);
        personagem2 = Instantiate(personagem2Prefab, posInicial2, Quaternion.identity);

        // Cria controladores
        mover1 = new CharacterMove(personagem1, moveSpeed);
        mover2 = new CharacterMove(personagem2, moveSpeed);

        // Inicia a cutscene
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
        
        // Fala 1 - Helena parada
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
        
        // Fala 2 - Helena comenta sobre a casa
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
        
        // Helena começa a andar
        bool p1chegou1 = false;
        while (!p1chegou1)
        {
            p1chegou1 = mover1.MoverPara(destino1, Time.fixedDeltaTime);
            yield return null;
        }

        yield return EsperarDialogoTerminar();
        
        // Fala 3 - Avó Elisa responde
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
        
        // Elisa anda em duas etapas
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
        
        
        // Fala 3 - Avó Elisa responde
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
                text = "Tome... \nEssa aqui é um retrato que a muito tempo foi tirado, nela aparece pessoas muito importante para mim \nE para você também pequena..."
            },
        };
        dialogueManager.StartDialogue(falas);
        yield return EsperarDialogoTerminar();
        
        SceneManager.LoadScene("Scenes/Puzzle");

        Debug.Log("Cutscene finalizada!");
    }

}