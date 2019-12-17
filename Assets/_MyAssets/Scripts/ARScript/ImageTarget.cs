using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ImageTarget : MonoBehaviour, ITrackableEventHandler
{
	private GameObject showObject;

	private TrackableBehaviour trackableBehaviour;

	private bool isTargetFound = false;

	private void Awake()
	{
		showObject = transform.GetChild(0).gameObject;
	}

	// Use this for initialization
	void Start()
	{
		trackableBehaviour = GetComponent<TrackableBehaviour>();

		if (trackableBehaviour != null)
		{
			trackableBehaviour.RegisterTrackableEventHandler(this);
		}	
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED ||
			newStatus == TrackableBehaviour.Status.TRACKED)
		{
			// Found new target.
			isTargetFound = true; 

			Controller.Instance.FoundNewTarget(showObject);
			showObject.GetComponent<ObjectScript>().OnFoundTarget();
		}
		else
		{
			if (isTargetFound)
			{
				Controller.Instance.ResetCamera();

				// Target Lost.
				showObject.GetComponent<ObjectScript>().OnLostTarget();				
			}
			else
			{
				// Disable the object for first time. 
				showObject.GetComponent<ObjectScript>().OnDisableObject();
			}
		}
	}


}
