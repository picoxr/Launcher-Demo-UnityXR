using UnityEngine;
namespace OneP.InfinityScrollView
{
	public static class RectTransformExtensions {
	    /// <summary>
	    /// Set the scale to 1,1,1
	    /// </summary>
	    public static void SetDefaultScale(this RectTransform trans) {
	        trans.localScale = new Vector3(1, 1, 1);
	    }

	    /// <summary>
	    /// Set the point in which both anchors and the pivot should be placed. This makes it very easy to set positions and scales, but it destroys autoscaling
	    /// </summary>
	    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec) {
	        trans.pivot = aVec;
	        trans.anchorMin = aVec;
	        trans.anchorMax = aVec;
	    }

	    /// <summary>
	    /// Get the current size of the RectTransform as a Vector2
	    /// </summary>
	    public static Vector2 GetSize(this RectTransform trans) {
	        return trans.rect.size;
	    }

	    public static float GetWidth(this RectTransform trans) {
	        return trans.rect.width;
	    }
	    public static float GetHeight(this RectTransform trans) {
	        return trans.rect.height;
	    }

	    /// <summary>
	    /// Set the position of the RectTransform within it's parent's coordinates. Depending on the position of the pivot, the RectTransform actual position will differ.
	    /// </summary>
	    public static void SetLocalPosition(this RectTransform trans, Vector2 newPos) {
	        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
	    }

	    public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos) {
	        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
	    }
	    public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos) {
	        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
	    }
	    public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos) {
	        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
	    }
	    public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos) {
	        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
	    }

	    public static void SetSizeDelta(this RectTransform trans, Vector2 newSize) {
	        Vector2 oldSize = trans.rect.size;
	        Vector2 deltaSize = newSize - oldSize;
	        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
	        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
	    }
	    public static void SetWidth(this RectTransform trans, float newSize) {
	        SetSizeDelta(trans, new Vector2(newSize, trans.rect.size.y));
	    }
	    public static void SetHeight(this RectTransform trans, float newSize) {
	        SetSizeDelta(trans, new Vector2(trans.rect.size.x, newSize));
	    }
	}
}