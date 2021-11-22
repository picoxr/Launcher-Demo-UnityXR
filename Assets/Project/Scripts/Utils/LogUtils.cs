using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogUtils : MonoBehaviour
{

	private static string tag = "PUI---Launcher---";

	public static void Log (object message)
	{
		if (Constant.IS_DEBUG) {
			Debug.Log (tag + message);
		}
	}

	public static void LogError (object message)
	{
		if (Constant.IS_DEBUG) {
			Debug.LogError (tag + message);
		}
	}

	public static void LogWarning (object message)
	{
		if (Constant.IS_DEBUG) {
			Debug.LogWarning (tag + message);
		}
	}

}
