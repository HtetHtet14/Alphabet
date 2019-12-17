using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestSuite_MainMenu 
{

	private SettingManager settingManager;
	private MCManager game;

    [Test]
	public void Tutorial_Test()
	{

		bool answer = false;
		game = new MCManager();

		int isOn_Tutorial = PlayerPrefs.GetInt("TUTORIAL_MODE");

		if (isOn_Tutorial == 1)

			//answer = game.isTutorialMode;
			game.isTutorialMode = true;

		//Assert.AreEqual(true, mCManager.isTutorialMode);
		Assert.AreEqual(true, game.isTutorialMode);
		//Assert.True(mCManager.isTutorialMode);
	}
}
