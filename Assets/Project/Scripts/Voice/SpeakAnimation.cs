using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpeakAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void VolumeAnim(int volume)
	{
		this.transform.DOScale (1.0f + 0.003f * (float)volume, 0.2f).SetLoops (-1, LoopType.Yoyo);
	}

}
