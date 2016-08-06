using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Line : MonoBehaviour {

	SpriteRenderer sprite;
	public Color activeColor;
	public Color defaultColor1;
	public Color defaultColor2;
	internal bool colorVariant = false;
	internal Vector2 coordinates;


	void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();;
	}

	public void highlight()
	{
		sprite.color = activeColor;
	}

	public void lowlight()
	{
		if (colorVariant)
			sprite.color = defaultColor2;
		else
			sprite.color = defaultColor1;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
