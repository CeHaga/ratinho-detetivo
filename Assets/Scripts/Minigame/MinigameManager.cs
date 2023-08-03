using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;

public class MinigameManager : MonoBehaviour
{
	[Header("Cards")]
	[SerializeField] private GameObject cardPrefab;

	[Range(1, 6)]
	[SerializeField] private int width;

	[Range(1, 3)]
	[SerializeField] private int height;

	[Header("Tries")]
	[SerializeField] private TMP_Text triesText;
	[SerializeField] private float delayBetweenTries;
	private bool isWaiting;

	[SerializeField] private int totalTries;
	private int currentTry;

	[SerializeField] private GameObject[] cards;
	[SerializeField] private bool[] cardValue;
	[SerializeField] private CardController[] cardControllers;

	private string mainGameScene;
	private int actualRow;

	// Start is called before the first frame update
	void Start()
	{
		actualRow = 0;
		currentTry = 0;
		isWaiting = false;

		mainGameScene = MinigameResult.mainGameScene;

		triesText.text = "" + (totalTries - currentTry);

		CreateCards();
		SetCardsValues();
	}

	public void ClickCard(int row, int column)
	{
		if (isWaiting) return;
		if (row != actualRow) return;

		StartCoroutine(ChangeCards(row, column));
	}

	public IEnumerator ChangeCards(int row, int column)
	{
		isWaiting = true;
		actualRow++;

		int index = row * width + column;
		cardControllers[index].FlipCard(cardValue[index]);

		bool isCorrect = cardValue[index];

		if (isCorrect)
		{
			Debug.Log("Correct");
			isWaiting = false;

			if (actualRow >= height)
			{
				Debug.Log("You Win");

				yield return new WaitForSeconds(delayBetweenTries);

				GameOver(true);
			}
			yield break;
		}
		Debug.Log("Incorrect");

		yield return new WaitForSeconds(delayBetweenTries);

		ResetCards();

		currentTry++;
		triesText.text = "" + (totalTries - currentTry);
		Debug.Log("Remaining tries: " + (totalTries - currentTry));
		if (currentTry >= totalTries)
		{
			Debug.Log("Game Over");

			// Flip each card with true value
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int cardIndex = i * width + j;
					if (cardValue[cardIndex])
					{
						cardControllers[cardIndex].FlipCard(true);
					}
				}
			}

			yield return new WaitForSeconds(delayBetweenTries);

			GameOver(false);
		}

		isWaiting = false;
	}

	public void ResetCards()
	{
		actualRow = 0;
		foreach (CardController card in cardControllers)
		{
			card.ResetCard();
		}
	}

	public IEnumerator WaitNextTry()
	{
		isWaiting = true;
		yield return new WaitForSeconds(delayBetweenTries);
		isWaiting = false;
	}

	public void GameOver(bool win)
	{
		// Go back to main game
		MinigameResult.happened = true;
		MinigameResult.win = win;
		SceneManager.LoadScene(mainGameScene);
	}

	public void CreateCards()
	{
		float cameraHeight = Camera.main.orthographicSize * 2;
		float cameraWidth = cameraHeight * Camera.main.aspect;

		float widthOffset = cameraWidth / (width + 1);
		float heightOffset = cameraHeight / (height + 1);

		// Destroy each card children
		if (transform.childCount > 0)
		{
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				StartCoroutine(Destroy(transform.GetChild(i).gameObject));
			}
		}

		// Create new cards
		cards = new GameObject[height * width];
		cardControllers = new CardController[height * width];
		UnityAction<int, int> OnCardClick = new UnityAction<int, int>(ClickCard);
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				cards[i * width + j] = Instantiate(cardPrefab, transform);
				cards[i * width + j].transform.position = new Vector3((j + 1) * widthOffset - cameraWidth / 2, -(i + 1) * heightOffset + cameraHeight / 2, 0);
				cardControllers[i * width + j] = cards[i * width + j].GetComponent<CardController>();
				cardControllers[i * width + j].Init(i, j, OnCardClick);
			}
		}

		// EditorUtility.SetDirty(this);
		// var scene = gameObject.scene;
		// EditorSceneManager.MarkSceneDirty(scene);
	}

	public void SetCardsValues()
	{
		cardValue = new bool[height * width];
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				cardValue[i * width + j] = false;
			}
			int randomColumn = Random.Range(0, width);
			cardValue[i * width + randomColumn] = true;
		}
	}

	IEnumerator Destroy(GameObject go)
	{
		yield return new WaitForEndOfFrame();
		DestroyImmediate(go);
	}
}
