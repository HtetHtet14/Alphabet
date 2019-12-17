using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollButtonScript : MonoBehaviour {

	public int id;
	public GameObject rightImg; 

	private void Awake()
	{
		rightImg.SetActive(false);
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	// When the player click on the button 
	public void OnClickButton ()
	{
		LPLocalManager.instance.OnClickQuestion(id);
	}

	// Show right and wrong image
	public void SetRight(int ans)
	{
		if (ans == 1)
			rightImg.SetActive(true);
		else
			rightImg.SetActive(false);
	}

	// Set answer string 
	public void SetString (string str)
	{

	}
}
