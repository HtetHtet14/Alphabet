using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudMover : MonoBehaviour {

	public Sprite[] sprites;

	private bool isMove;

	private float sprWidth; 

	private void OnEnable()
	{
		isMove = true; 
	}

	// Use this for initialization
	void Start ()
	{
		GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];

		sprWidth = GetComponent<Image>().sprite.border.x;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isMove)
		{
			transform.Translate(Random.Range(5, 20) * Time.deltaTime, 0, 0);

			if (transform.localPosition.x > Screen.width + sprWidth + 50)
			{
				transform.localPosition = new Vector2(-Screen.width - sprWidth - 50, transform.localPosition.y);

				GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];
			}
		}
	}
}
