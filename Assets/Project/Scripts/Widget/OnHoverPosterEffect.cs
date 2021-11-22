using UnityEngine;
using System.Collections;

public class OnHoverPosterEffect : MonoBehaviour
{
	public GameObject rotationRoot;
	public GameObject m_box;
	public float value = 6;
	private float x1;
	private float x2;
	private float y1;
	private float y2;
	private float baseRX;
	private float baseRY;
	private float RX;
	private float RY;
	private Vector2 old_wh;
	private Vector2 new_wh;

	private RectTransform m_box_rect;
	private Vector2 m_box_sizeDelta;

	// Use this for initialization
	void Start ()
	{

		m_box_rect = m_box.GetComponent<RectTransform> ();
		m_box_sizeDelta = m_box_rect.sizeDelta;

		x1 = m_box.transform.position.x - m_box_sizeDelta.x * 1 / 6;
		x2 = m_box.transform.position.x + m_box_sizeDelta.x * 1 / 6;
		y1 = m_box.transform.position.y + m_box_sizeDelta.y * 1 / 6;
		y2 = m_box.transform.position.y - m_box_sizeDelta.y * 1 / 6;

		baseRX = value / (m_box_sizeDelta.x * 1 / 3) / 70;
		baseRY = value / (m_box_sizeDelta.y * 1 / 3) / 70;

		old_wh = new Vector2 (m_box_sizeDelta.x, m_box_sizeDelta.y);
		new_wh = new Vector2 (m_box_sizeDelta.x + 10, m_box_sizeDelta.y + 15);
	}

	// Update is called once per frame
	void Update ()
	{
		if (null == rotationRoot) {
			return;
		}
		
		if (null != PvrInputMoudle.CurrentRaycastResult.gameObject && PvrInputMoudle.CurrentRaycastResult.gameObject == m_box.gameObject) {
			
			Vector3 point = transform.InverseTransformPoint (PvrInputMoudle.CurrentRaycastResult.worldPosition);
			m_box_sizeDelta = new_wh;

			if (point.x <= m_box.transform.position.x) {
				RX = baseRX * (m_box.transform.position.x - point.x);
			} else if (point.x > m_box.transform.position.x) {
				RX = baseRX * (m_box.transform.position.x - point.x);
			} else {
				RX = 0;
			}

			if (point.y <= m_box.transform.position.y) {
				RY = baseRY * (point.y - m_box.transform.position.y);
			} else if (point.y > m_box.transform.position.y) {
				RY = baseRY * (point.y - m_box.transform.position.y);
			} else {
				RY = 0;
			}

			DoRotation (new Vector3 (RY, RX, 0.0f));
		} else {
			m_box_sizeDelta = old_wh;
			DoRotation (Vector3.zero);
		}
	}

	private void DoRotation (Vector3 value)
	{
		rotationRoot.transform.localRotation = Quaternion.Lerp (rotationRoot.transform.localRotation, Quaternion.EulerAngles (value), Time.deltaTime * 7.0f);
	}
}
