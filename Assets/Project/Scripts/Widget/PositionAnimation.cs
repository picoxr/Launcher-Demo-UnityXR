using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PositionAnimation : MonoBehaviour {

	private Vector3 normalPosition;
	public float targetPositionZ = -50f;
	private float time = 0.2f;
	private Tween hoverTween;

	// Use this for initialization
	void Start () {
		normalPosition = this.transform.localPosition;
		PUIEventListener.Get(this.gameObject).onHover += OnHover;
		//PUIEventListener.Get(this.gameObject).onClick += OnClick;
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	private void OnHover(GameObject obj, bool isHover)
	{
		if (!this.enabled)
			return;
		hoverTween.Kill ();
		if (isHover) {
			hoverTween = this.transform.DOLocalMoveZ (targetPositionZ, time);
		} else {
			hoverTween = this.transform.DOLocalMoveZ (normalPosition.z, time);
		}
	}

	private void OnClick(GameObject obj)
	{
		if (!this.enabled)
			return;
		hoverTween.Kill ();
		this.transform.localPosition = normalPosition;
	}

	void OnDestroy()
	{
		PUIEventListener.Get(this.gameObject).onHover -= OnHover;
		//PUIEventListener.Get(this.gameObject).onClick -= OnClick;
	}

}
