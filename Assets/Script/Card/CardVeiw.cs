using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardVeiw : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private CardManager CardManager;
    
    private Card _card;
    public Card Card { get { return _card; } 
        set
        {
            _card = value;
            if(_card != null)
            {
                explan.text = _card.Explanation;
                _name.text = _card.Name;
                GetComponent<Image>().sprite = CardManager.Sprites[(int)_card.Sprite];
            }
        } 
    }

    //����
    [SerializeField]
    private Text explan;
    [SerializeField]
    private Text _name;

    public void Init(CardManager c)
    {
        CardManager = c;
    }

    public void Select()
    {
        CardManager.Selected(_card);
        SoundManager.instance.SFXCreate(SoundManager.Clips.CardClip, 1, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        explan.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        explan.gameObject.SetActive(false);
    }
}