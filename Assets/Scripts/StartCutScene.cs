using System.Collections;
using UnityEngine;

public class StartCutScene : MonoBehaviour
{
    public GameObject personagem1Prefab;
    public GameObject personagem2Prefab;

    public Vector2 posInicial1 = new Vector2(-1.5f, -0.5f);
    public Vector2 posInicial2 = new Vector2(-5.5f, 4);

    public Vector2 destino1 = new Vector2(-1.5f, 2);
    public Vector2 destino2 = new Vector2(-1.5f, 2.5f);

    public float moveSpeed = 3f;

    private GameObject personagem1;
    private GameObject personagem2;

    private CharacterMove mover1;
    private CharacterMove mover2;

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

    private IEnumerator ControlarCutscene()
    {
        bool p1chegou1 = false; bool p2chegou1 = true; bool p2chegou2 = true;

        while (!p1chegou1)
        {
            p1chegou1 = mover1.MoverPara(destino1, Time.deltaTime);
            if (p1chegou1) p2chegou1 = false;
            yield return null;
        }
        
        while (!p2chegou1)
        {
            p2chegou1 = mover2.MoverPara(new Vector2(-1.5f, 4), Time.deltaTime);
            if (p2chegou1) p2chegou2 = false;
            yield return null;
        }
        
        
        while (!p2chegou2)
        {
            p2chegou2 = mover2.MoverPara(destino2, Time.deltaTime);
            yield return null;
        }

        // Cutscene finalizada
        Debug.Log("Cutscene finalizada!");
    }
}