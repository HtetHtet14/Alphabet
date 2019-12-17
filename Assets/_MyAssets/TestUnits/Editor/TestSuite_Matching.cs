using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class TestSuite_Matching 
{
	private MCManager mCManager;
	private GameObject gameObject;
	
	[Test]
	public void Is_Playing_Tutorial()
	{
		mCManager = GameObject.Find("MCManager").GetComponent<MCManager>();

		bool isON = mCManager.isTutorialMode;

		//if(isON == true)
		//{
		//	Assert.AreEqual(1, PlayerPrefs.GetInt("TUTORIAL_MODE"));
		//}

		if(PlayerPrefs.GetInt("TUTORIAL_MODE") == 1)
		{
			Assert.AreEqual(true, isON);
		}
		
	}
	
}
