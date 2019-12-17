using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimateSoundScript : MonoBehaviour
{
	public AnimationInfo idleInfo; 

	public AnimationInfo[] animationInfos;

	private Animation anim;

	private AudioSource audioSource;

	private int index = 0; 

	// 
	IEnumerator Start ()
	{
		anim = GetComponent<Animation>();

		audioSource = GetComponentInParent<AudioSource>();

		anim.Play();

		foreach (var item in animationInfos)
		{
			anim.AddClip(item.animClip, item.animClip.name);
		}

		// Add Idle animation 
		anim.AddClip(idleInfo.animClip, idleInfo.animClip.name);

		if(animationInfos.Length > 0)
		{
			index = animationInfos.Length - 1;
		}

		yield return new WaitForEndOfFrame();

		if(SceneManager.GetActiveScene().name == "LPLocalScene")
		{
			PlayAnimSound();
		}	
	}

	// Play the animation 
	public void PlayAnimSound ()
	{
		anim = GetComponent<Animation>();

		audioSource = GetComponentInParent<AudioSource>();

		audioSource.volume = PlayerPrefs.GetFloat("SOUND_VOLUME", 0.5F);

		// If there have no infos animation, just play Idle and return
		if(animationInfos.Length == 0)
		{
			PlayIdle();

			return; 
		}

		if( index >= animationInfos.Length)
		{
			index = 0;
		}

		StopCoroutine(ChangeAnimation(animationInfos[index]));

		anim.Play(animationInfos[index].animClip.name);

		audioSource.clip = animationInfos[index].audioClip;

		audioSource.loop = true; 

		audioSource.Play();

		StartCoroutine(ChangeAnimation(animationInfos[index]));

		index++;
	}

	// Change animation 
	IEnumerator ChangeAnimation (AnimationInfo info)
	{
		yield return new WaitForSeconds(info.playTime == 0 ? info.audioClip.length : info.playTime);

		audioSource.Stop();

		PlayIdle();
	}

	// Play Idle animation 
	public void PlayIdle ()
	{
		anim.Play(idleInfo.animClip.name);

		if (idleInfo.audioClip != null)
		{
			audioSource.loop = true;

			audioSource.clip = idleInfo.audioClip;

			audioSource.Play();
		}
	}
}

[System.Serializable]
public class AnimationInfo
{
	public string name;

	public AnimationClip animClip;

	public float playTime;

	public AudioClip audioClip;

	public bool isLoop;
}
