using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent()]
public class Interactive : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
	[SerializeField] string m_toolTips;
	[SerializeField] bool m_localize;
    [SerializeField] float m_gap_y = -0.1f;
    [SerializeField] bool m_showDelta = false;

    const float k_tipsPopup = 0.04f;

	const float k_enabled = -1;
	const float k_disabled = -2;

	const float k_createTipsDelay = 0.2f;

	float m_hoverDuration;

	/// <summary> tips与head的距离. </summary>
	static float s_tipsDistance = 0;

	static Transform s_head;
	static GameObject s_tips;
    static Interactive s_current;

	static Text s_tipsText;
    static GameObject s_tipsDelta;
    static Canvas s_rootCanvas;
		
	public void SetToolTips(string toolTips, bool localize, float gap_y=-0.1f,bool showDelta=false)
	{
		m_toolTips = toolTips;
		m_localize = localize;
        m_gap_y = gap_y;
        m_showDelta = showDelta;
    }

	void Start()
	{
		if (s_tips == null)
		{
			Initialize();
		}

		m_hoverDuration = k_disabled;
	}

	void Update()
	{

        if (m_hoverDuration >= 0 && (m_hoverDuration += Time.deltaTime) >= k_createTipsDelay)
        {
            if (string.IsNullOrEmpty(m_toolTips))
            {
                HideTips();
                //Debug.LogError("Tips内容为空, 无法显示");
            }
            else
            {
                ShowTips();
            }
        }

        if (s_tips != null && s_tips.gameObject.activeSelf)
        {
            UpdateTipsOrientation();
        }
    }

	void OnDisable()
	{
		if (s_current == this)
		{
			HideTips();
		}
	}

	Vector3 CalculateTipsPosition()
	{
		float halfWidth = s_tips.GetComponent<HorizontalLayoutGroup>().preferredWidth / 2;
		Vector3 pos = PvrInputMoudle.CurrentRaycastResult.worldPosition;
		pos.x -= halfWidth * s_tips.GetComponent<RectTransform>().lossyScale.x;
		pos.y -= 0.07f;

		return s_head.position + (pos - s_head.position).normalized * s_tipsDistance;
	}

	Vector3 CalculateTipsPosition2()
	{
		GameObject gameObject = PvrInputMoudle.CurrentRaycastResult.gameObject;
		Vector3 pos = gameObject.transform.position;
        Debug.Log("ida----------------------------------posY:" + pos.y);
		pos.y -= (gameObject.GetComponent<RectTransform> ().sizeDelta.y * 0.005f / 2.0f + 0.02f) + m_gap_y;

		return pos;
	}

	public void ShowTips()
	{
		s_tipsText.text = m_localize ? Localization.Get(m_toolTips) : m_toolTips;
		s_tips.SetActive(true);

        s_tipsDelta.SetActive(m_showDelta);

        s_tips.GetComponent<HorizontalLayoutGroup>().CalculateLayoutInputHorizontal();

		Vector3 pos = CalculateTipsPosition2();
		s_tips.GetComponent<RectTransform>().position = pos;
		s_tips.transform.SetAsLastSibling();
		s_tips.transform.rotation = transform.rotation;

        if (m_showDelta) {
            s_tipsDelta.GetComponent<RectTransform>().position = pos;
            s_tipsDelta.transform.SetAsLastSibling();
            s_tipsDelta.transform.rotation = s_tips.transform.rotation;
        }

        m_hoverDuration = k_enabled;

		s_current = this;
	}

	public void HideTips()
	{
		s_tips.SetActive(false);
        s_tipsDelta.SetActive(false);
        m_hoverDuration = k_disabled;

		s_current = null;
	}

	void UpdateTipsOrientation()
	{
		s_tips.transform.rotation = Quaternion.LookRotation((s_tips.transform.position - s_head.position).normalized, transform.up);
        s_tipsDelta.transform.rotation = Quaternion.LookRotation((s_tipsDelta.transform.position - s_head.position).normalized, transform.up);

    }

	void Initialize()
	{
		s_tips = Instantiate(Resources.Load("ToolTips")) as GameObject;
		s_tips.SetActive(false);
        s_tipsDelta = Instantiate(Resources.Load("ToolTipsDelta")) as GameObject;
        s_tipsDelta.SetActive(false);
        s_tipsText = s_tips.transform.Find("Text").GetComponent<Text>();

		s_rootCanvas = FindTopmostCanvas();

		s_head = GameObject.Find("Head").transform;

		s_tipsDistance = (s_rootCanvas.transform.position - s_head.position).magnitude - k_tipsPopup;

		s_tips.transform.SetParent(s_rootCanvas.transform, false);
		s_tips.transform.localScale = Vector3.one;
		s_tips.transform.localPosition = Vector3.zero;
        s_tipsDelta.transform.SetParent(s_rootCanvas.transform, false);
        s_tipsDelta.transform.localScale = Vector3.one;
        s_tipsDelta.transform.localPosition = Vector3.zero;
    }

	Canvas FindTopmostCanvas()
	{
		Canvas[] parentCanvases = GetComponentsInParent<Canvas>();
		if (parentCanvases != null && parentCanvases.Length > 0)
		{
			return parentCanvases[parentCanvases.Length - 1];
		}

		return null;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		m_hoverDuration = 0;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		HideTips();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		m_hoverDuration = 0;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//HideTips();
	}
}
