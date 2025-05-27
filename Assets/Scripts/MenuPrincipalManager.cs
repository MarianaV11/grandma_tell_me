using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalManager : MonoBehaviour
{
    [Header("Configurações de Cena e Painéis")]
    [SerializeField] private string nomeDolevelDeJogo = "Jogo";
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;

    [Header("Botões de Dificuldade")]
    public Button botaoFacil;
    public Button botaoMedio;
    public Button botaoDificil;

    private Color corSelecionado = new Color(0.4f, 0.6f, 1f); // azul claro
    private Color corNormal = Color.white;

    void Start()
    {
        ModoDificuldadeSelecionado.dificuldade = Dificuldade.Facil;
        AtualizarCoresBotoes(botaoFacil);
    }

    public void Jogar()
    {
        SceneManager.LoadScene(nomeDolevelDeJogo);
    }

    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

    public void SairJogo()
    {
        Debug.Log("Sair do jogo");
        Application.Quit();
    }

    public void SelecionarFacil()
    {
        ModoDificuldadeSelecionado.dificuldade = Dificuldade.Facil;
        AtualizarCoresBotoes(botaoFacil);
        Debug.Log("Modo Fácil selecionado");
    }

    public void SelecionarMedio()
    {
        ModoDificuldadeSelecionado.dificuldade = Dificuldade.Medio;
        AtualizarCoresBotoes(botaoMedio);
        Debug.Log("Modo Médio selecionado");
    }

    public void SelecionarDificil()
    {
        ModoDificuldadeSelecionado.dificuldade = Dificuldade.Dificil;
        AtualizarCoresBotoes(botaoDificil);
        Debug.Log("Modo Difícil selecionado");
    }

    private void AtualizarCoresBotoes(Button selecionado)
    {
        botaoFacil.image.color = corNormal;
        botaoMedio.image.color = corNormal;
        botaoDificil.image.color = corNormal;

        selecionado.image.color = corSelecionado;
    }
}
