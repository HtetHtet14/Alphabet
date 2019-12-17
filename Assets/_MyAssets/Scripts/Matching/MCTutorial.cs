using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTutorial : MonoBehaviour {
	public static MCTutorial Instance = null;

	public GameObject finger;

	private Animator anim;
	private int index = 1;

	private void Awake()
	{
		Instance = this; 
	}

	// Use this for initialization
	void Start ()
	{
		anim = finger.GetComponent<Animator>();

		anim.Play("Stage1");

		finger.SetActive(false);
	}

	// Change animation state 
	public void ChangeAnimState ()
	{
		index++;
		anim.Play("Stage" + index.ToString());

		if(index == 4)
		{
			TutorialEnd();
		}
	}

	// Call when tutorial end
	public void TutorialEnd ()
	{
		finger.SetActive(false);

		PlayerPrefs.SetInt("TUTORIAL_MODE", 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
