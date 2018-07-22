using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidSquarebehaviour : Photon.MonoBehaviour {

	GameManager gameManager;
	public string id;
	public bool isLocked = false;
	Color newColor;
	SpriteRenderer spriteRenderer;

	void Start() {
		gameManager = GameManager.gameManager;
		spriteRenderer = GetComponent<SpriteRenderer>();
		newColor = spriteRenderer.color;
		id = name;
		gameManager.midClicks.Add(id, 0);

		int i = 0;
		foreach (SquareBehaviour square in transform.GetComponentsInChildren<SquareBehaviour> ()) {
			square.name = id + i;
			square.id = square.name;
			gameManager.board.Add(square.id, 2);
			i++;
		}
	}

	private void Update()
	{
		spriteRenderer.color = Color.Lerp(spriteRenderer.color, newColor, Time.deltaTime * 8f);
	}

	public void Lock (Color color) {
		isLocked = true;
		ChangeColor(color);
	}

	public void ChangeColor(Color color)
	{
		newColor = color;
	}
}
