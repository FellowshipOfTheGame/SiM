using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public bool Volume
    {
        set
        {
            instance.GetComponent<AudioSource>().volume = (value ? 1f : 0f); 
        }
    }

    [Header("Levels")]
    public Collection levels;
    public static int level;
    
	private BoardGenerator boardGenerator = null;
    public Color? currentColor = null;
	public float currentTime;

	//Mouse Variables
	private bool isMouseBeingDragged = false;
	private Color? mouseEfetiveColor = null;
	private Line highlightLine;
	private Line highlightColumn;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
		boardGenerator = GetComponent<BoardGenerator> ();
    }

    void Update()
    {
        if (level != -1)
        {
            LoadBoard();
            level = -1;
        }
        currentTime += Time.deltaTime;
			
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (mousePos, Vector2.zero);

		if(highlightLine != null){
			highlightLine.lowlight();
			highlightLine = null;
		}
		if(highlightColumn != null){
			highlightColumn.lowlight();
			highlightColumn = null;
		}
			

		if (hit)
		{
			Pixel script = hit.collider.gameObject.GetComponent<Pixel> ();
			if (script) {
				float x = script.coordinates.x;
				float y = script.coordinates.y;					

				GameObject[] lines = GameObject.FindGameObjectsWithTag ("Lines");
				foreach(GameObject obj in lines){
					Line line = obj.GetComponent<Line> ();
					if (line != null) {
						if (line.coordinates.x == x && highlightColumn == null){
							line.highlight();
							highlightColumn = line;
						}
						if (line.coordinates.y == y && highlightLine == null){
							line.highlight();
							highlightLine = line;
						}
					}
				}
			}
		}

		if (Input.GetMouseButton (0) || Input.GetMouseButton (1) ) {
			//isMouseClicked = true;

			if (hit) 
			{
				Pixel script = hit.collider.gameObject.GetComponent<Pixel> ();
				if (script) {
					if (!isMouseBeingDragged)
						mouseEfetiveColor = script.actualColor;
					isMouseBeingDragged = true;
					if(mouseEfetiveColor.Equals(script.actualColor)){
						if (Input.GetMouseButton (0) && !Input.GetMouseButton (1))
							script.OnSelect (currentColor);
						else if (Input.GetMouseButton (1) && !Input.GetMouseButton (0))
							script.OnSelect (null);
					}
				}

				ColorChooser colorC = hit.collider.gameObject.GetComponent<ColorChooser> ();
				if (colorC) 
				{
					GameObject colorPicker = GameObject.Find ("Color Picker");
					foreach (Transform child in colorPicker.transform) {
						ColorChooser chooser = child.GetComponent<ColorChooser> ();
						if (chooser) {
							if (chooser.selected)
								chooser.StartCoroutine ("shrinkSize");
						}
					}
					currentColor = colorC.actualColor;
					colorC.StartCoroutine ("growSize");
				}
			}
		}
		else if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
		{
			isMouseBeingDragged = false;
			mouseEfetiveColor = null;
		}
    }
		
	public bool verifyWining()
	{
		bool hasWon = true;
		GameObject[] pixels = GameObject.FindGameObjectsWithTag ("Pixel");
		foreach (GameObject cell in pixels) 
		{
			Pixel pixel = cell.GetComponent<Pixel>();

			if (pixel != null && !pixel.IsColorRight ()) {
				hasWon = false;
				break;
			}
		}
        print(hasWon);
		if(hasWon) 
		{
			foreach (GameObject cell in pixels) {
				Pixel pixel = cell.GetComponent<Pixel>();

				if (pixel) {
					pixel.StopAllCoroutines ();
					pixel.StartCoroutine ("fill");
				}
			}
		}

		return hasWon;
	}


    public static GameManager GetInstance()
    {
        return instance;
    }

	public void LoadBoard(){
		currentTime = 0f;
		if (boardGenerator == null)
			boardGenerator = GetComponent<BoardGenerator> ();
        boardGenerator.LoadBoard(level);
	}

}
