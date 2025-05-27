using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Componentes visuais")]
    [SerializeField] private Image iconImage; // Imagem da carta

    [Header("Sprites")]
    public Sprite hiddenIconSprite; // Ícone oculto
    public Sprite iconSprite;       // Ícone revelado

    [Header("Estado da carta")]
    public bool isSelected;         // Se a carta está selecionada ou não

    [HideInInspector]
    public CardsController controller; // Referência ao controlador do jogo

    // Evento de clique da carta
    public void OnCardClick()
    {
        controller.SetSelected(this);
    }

    // Define o sprite da carta (chamada na criação)
    public void SetIconSprite(Sprite sp)
    {
        iconSprite = sp;
    }

    // Mostra a face da carta
    public void Show()
    {
        iconImage.sprite = iconSprite;
        isSelected = true;
    }

    // Esconde a face da carta
    public void Hide()
    {
        iconImage.sprite = hiddenIconSprite;
        isSelected = false;
    }
}
