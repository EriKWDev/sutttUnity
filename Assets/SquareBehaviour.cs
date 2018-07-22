using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBehaviour : Photon.MonoBehaviour {

	GameManager gameManager;
	public string id = "";

	public Color newColor;

	SpriteRenderer spriteRenderer;

	public void Awake() {
		id = this.name;
	}

	void Start() {
		gameManager = GameManager.gameManager;
		spriteRenderer = GetComponent<SpriteRenderer>();
		newColor = spriteRenderer.color;
	}

	private void Update()
	{
		spriteRenderer.color = Color.Lerp(spriteRenderer.color, newColor, Time.deltaTime * 8f);
	}

	public void ChangeColor (Color color) {
		newColor = color;
	}
}
