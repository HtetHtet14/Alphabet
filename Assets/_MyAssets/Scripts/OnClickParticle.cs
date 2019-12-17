using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickParticle : MonoBehaviour {

	public GameObject clickPart;
	public Camera mainCam;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			ShowOnClickParticle();
		}
	}


	// Show particle when mouse click on the screen 
	public void ShowOnClickParticle()
	{
		Vector3 mousePos = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3));

		GameObject part = Instantiate(clickPart, mainCam.transform, false);
		part.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 3.0f);
	}
}
