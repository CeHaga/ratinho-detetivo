using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] private int row;
	[SerializeField] private int column;

	[SerializeField] private OnCardClickEvent OnCardClick;

	public void Init(int row, int column, UnityAction<int, int> OnCardClick)
	{
		this.row = row;
		this.column = column;
		this.OnCardClick.AddListener(OnCardClick);
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

	public void ResetCard()
	{
		GetComponent<SpriteRenderer>().color = Color.white;
	}
}
