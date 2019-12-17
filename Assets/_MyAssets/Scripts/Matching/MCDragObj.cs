using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MCDragObj : MonoBehaviour {

	public enum State
	{
		NOONE,
		RIGHT,
		WRONG
	}
	public State curState = State.NOONE;

	public Text ansText;

	private GameObject colliderObj;

	public GameObject clickedObj;

	private bool snapToQuestion;

	// private float nearestDist = 1000000f;

	private GameObject nearestQuestion; 

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 mPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);

		var dist = 0f;

		var nearestDist = 1000000f;

		foreach (var item in MCManager.Instance.qBoxs)
		{
			var itemPos = new Vector2(item.transform.position.x, item.transform.Find("AnsText").transform.position.y);

			dist = Vector2.Distance(transform.position, itemPos);

			if (dist < nearestDist)
			{
				nearestDist = dist;
				nearestQuestion = item;
			}
		}

		if (nearestQuestion != null)
		{
			Debug.Log("Nearest object is : " + nearestQuestion.name);
		}

		var nearestQDist = Vector2.Distance(MCManager.Instance.mainCam.ScreenToWorldPoint(mPos), 
			nearestQuestion.transform.Find("AnsText").transform.position);

		var snapPos = Vector2.zero;

		if (nearestQDist < 1.5f)
		{
			snapPos = nearestQuestion.transform.Find("AnsText").transform.position;
		}
		else
		{
			snapPos = MCManager.Instance.mainCam.ScreenToWorldPoint(mPos);
		}

		transform.position = Vector2.Lerp(transform.position, snapPos, Time.deltaTime * 30);

		if(Input.GetMouseButtonUp(0))
		{
			switch (curState)
			{
				case State.NOONE:
					// Destroy(gameObject);
					clickedObj.SetActive(true);
					break;

				case State.RIGHT:
					if(MCManager.Instance.isTutorialMode)
					{
						MCTutorial.Instance.ChangeAnimState();
					}
					colliderObj.GetComponent<MCQBox>().SetRightAnswer(true);
					break;

				case State.WRONG:
					//Handheld.Vibrate();
					clickedObj.SetActive(true);
					colliderObj.GetComponent<MCQBox>().SetRightAnswer(false);			
					break;

				default:
					break;
			}

			Destroy(gameObject);
		}
	}

	public void SetText (string _str)
	{
		ansText.text = _str;
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag != "QBox")
			return; 

		colliderObj = collider.gameObject; 

		if(collider.GetComponent<MCQBox>().rightAns == ansText.text)
		{
			curState = State.RIGHT;
		}
		else 
		{
			curState = State.WRONG;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		curState = State.NOONE;
	}


}
