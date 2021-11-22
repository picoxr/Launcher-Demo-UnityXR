using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HomeAnimation : MonoBehaviour {

	private Vector3 normalPosition;
//	public float targetPositionZ = -40f;
	public float targetScale = 1.2f;
	private float time = 0.3f;
	public RectTransform imageRectTran;

	private Tweener imageTweener;
	private bool isHover;
	// Use this for initialization
	void Start () {
		normalPosition = this.transform.localPosition;
		PUIEventListener.Get(this.gameObject).onHover += OnHover;
		PUIEventListener.Get(this.gameObject).onClick += OnClick;
//		PvrInputMoudle.FindInputModule().onRefreshCursor += RefreshCursor;
	}

	// Update is called once per frame
//	void Update () {
//
//	}

//	void RefreshCursor(UnityEngine.EventSystems.RaycastResult raycastResult)
//	{
//		if (raycastResult.gameObject == null || raycastResult.gameObject != this.gameObject)
//			return;
//		if (!isHover)
//			return;
//		Vector3 localPos = transform.InverseTransformPoint (raycastResult.worldPosition);
//		float xOffset = (float)(imageRectTran.rect.width * 0.1 / 2);
//		float yOffset = (float)(imageRectTran.rect.height * 0.1 / 2);
//		Vector3 tempPos = new Vector3(Mathf.Clamp (localPos.x, -xOffset, xOffset), Mathf.Clamp (localPos.y, -yOffset, yOffset), 0);
//		imageTweener = this.imageRectTran.DOLocalMove (tempPos, time);
//	}

	private void OnHover(GameObject obj, bool isHover)
	{
		if (!this.enabled)
			return;
		this.isHover = isHover;
		if (isHover) {
//			this.transform.DOLocalMoveZ (targetPositionZ, time);
			this.imageRectTran.DOScale (targetScale, time);
			this.imageRectTran.DOLocalMoveZ (80f, time);
		} else {
//			this.transform.DOLocalMoveZ (normalPosition.z, time);
			this.imageRectTran.DOScale (Vector3.one, time);
			this.imageRectTran.DOLocalMoveZ (0f, time);
//			this.imageRectTran.DOLocalMove (Vector3.zero, time);
		}
	}

	private void OnClick(GameObject obj)
	{
		if (!this.enabled)
			return;
//		imageTweener.Kill ();
		OnHover (obj, false);
	}

	void OnDestroy()
	{

	}

}
