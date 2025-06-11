using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
public class ManagerFlorest : MonoBehaviour
{
    [Header("Personagem Principal")]
    public GameObject personagem1Prefab;
    [SerializeField] private string personagem1Name;
    [SerializeField] private Sprite personagem1Foto;

    [Header("Configuração da Cena da Floresta")]
    public GameObject MemoryGame;
    public float moveSpeed = 10f;
    public Vector2 posInicial1 = new Vector2(-1.5f, -0.5f);

    [SerializeField] private DialogueManager dialogueManager;

    private GameObject personagem1;
    private GranddaughterController granddaughterController;
    private CharacterMove mover1;
    
    void Start()
    {
        personagem1 = Instantiate(personagem1Prefab, posInicial1, Quaternion.identity);
        granddaughterController = personagem1.GetComponent<GranddaughterController>();
        granddaughterController.enabled = true;
        mover1 = new CharacterMove(personagem1, moveSpeed);
        
    }

    void Update()
    {
        
    }
}
