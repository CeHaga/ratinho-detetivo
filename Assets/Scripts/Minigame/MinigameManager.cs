using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;

    [Range(1, 6)]
    [SerializeField] private int width;

    [Range(1, 3)] 
    [SerializeField] private int height;

    [SerializeField] private GameObject[] cards;
    [SerializeField] private bool[] cardValue;
    [SerializeField] private CardController[] cardControllers;

    private int actualRow;
    private int diariesFound;

	public static MinigameManager Instance {
		get; private set;
	}
	public bool hasStarted {
		get; private set;
	}
	
	private void Awake() {
		if (Instance != null && Instance != this) { 
			Destroy(this.gameObject);
		} else {
			Instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}

    // Start is called before the first frame update
    void Start()
    {
        actualRow = 0;
        diariesFound = 0;

        // Set one random card true per row
        cardValue = new bool[height * width];
        for (int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                cardValue[i * width + j] = false;
            }
            int randomColumn = Random.Range(0, width);
            cardValue[i * width + randomColumn] = true;
        }
    }

    // Update is called once per frame 
    void Update()
    {
        // Check if all rows were clicked
        if (actualRow >= height)
        {
            if(diariesFound == actualRow)
            {
                Debug.Log("You win!");
            }
            else
            {
                Debug.Log("You lose!");
            }
        }
    }

    public void ClickCard(int row, int column)
    {
        Debug.Log("Clicked card: " + row + ", " + column);
        if (row == actualRow)
        {
            cardControllers[row * width + column].FlipCard(cardValue[row * width + column]);
            actualRow++;
        }
    }

    [EditorCools.Button]
    public void CreateCards()
    {
        Debug.Log("Creating cards");

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
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                cards[i * width + j] = Instantiate(cardPrefab, transform);
                cards[i * width + j].transform.position = new Vector3((j + 1) * widthOffset - cameraWidth / 2, -(i + 1) * heightOffset + cameraHeight / 2, 0);
                cardControllers[i * width + j] = cards[i * width + j].GetComponent<CardController>();
                cardControllers[i * width + j].Init(i, j);
            }
        }
        // Debug if cardcontrollers is null
        Debug.Log("Card controllers is null: " + (cardControllers == null));
 
        EditorUtility.SetDirty(this);
        var scene = gameObject.scene;
        EditorSceneManager.MarkSceneDirty(scene);
    }

    IEnumerator Destroy(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }
}
