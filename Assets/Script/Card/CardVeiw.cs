using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardVeiw : MonoBehaviour
{
    private CardManager CardManager;
    public Card card;


    public void Init(CardManager c)
    {
        CardManager = c;
    }

    public void Select()
    {
        CardManager.Selected(card);
    }
}