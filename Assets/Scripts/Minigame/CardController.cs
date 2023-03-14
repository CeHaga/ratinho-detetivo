using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int row;
    [SerializeField] private int column;

    [SerializeField] private OnCardClickEvent OnCardClick;

    public void Init(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnCardClick.Invoke(row, column);
    }

    public void FlipCard(bool value)
    {
        if (value)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
