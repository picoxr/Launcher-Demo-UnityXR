using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(Interactive))]
public class InteractiveEditor : Editor
{
	List<string> mKeys;

	void OnEnable()
	{
		Dictionary<string, string[]> dict = Localization.dictionary;

		if (dict.Count > 0)
		{
			mKeys = new List<string>();

			foreach (KeyValuePair<string, string[]> pair in dict)
			{
				if (pair.Key == "KEY") continue;
				mKeys.Add(pair.Key);
			}
			mKeys.Sort(delegate (string left, string right) { return left.CompareTo(right); });
		}
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		GUILayout.Space(6f);
		PGUIEditorTools.SetLabelWidth(80f);
		var sp = PGUIEditorTools.DrawProperty("Localize", serializedObject, "m_localize");

		if (sp.boolValue)
		{
			// Key not found in the localization file -- draw it as a text field
			DrawLocalization();
		}
		else
		{
			PGUIEditorTools.DrawProperty("Message", serializedObject, "m_toolTips");
		}

        PGUIEditorTools.DrawProperty("GapY", serializedObject, "m_gap_y");
        PGUIEditorTools.DrawProperty("ShowDelta", serializedObject, "m_showDelta");

        serializedObject.ApplyModifiedProperties();
	}

	void DrawPreview(string myKey)
	{
		if (PGUIEditorTools.DrawHeader("Preview"))
		{
			PGUIEditorTools.BeginContents();

			string[] keys;
			string[] values;

			if (Localization.dictionary.TryGetValue("KEY", out keys) && Localization.dictionary.TryGetValue(myKey, out values))
			{
				for (int i = 0; i < keys.Length; ++i)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(keys[i], GUILayout.Width(70f));

					if (GUILayout.Button(values[i], "TextArea"/*"AS TextArea"*/, GUILayout.MinWidth(80f), GUILayout.MaxWidth(Screen.width - 110f)))
					{
						(target as UILocalize).value = values[i];
						GUIUtility.hotControl = 0;
						GUIUtility.keyboardControl = 0;
					}
					GUILayout.EndHorizontal();
				}
			}
			else
			{
				GUILayout.Label("No preview available");
			}

			PGUIEditorTools.EndContents();
		}
	}

	void DrawLocalization()
	{
		GUILayout.BeginHorizontal();
		SerializedProperty sp = PGUIEditorTools.DrawProperty("Key", serializedObject, "m_toolTips");
		string myKey = sp.stringValue;
		bool isPresent = (mKeys != null) && mKeys.Contains(myKey);

		GUI.color = isPresent ? Color.green : Color.red;
		GUILayout.BeginVertical(GUILayout.Width(22f));
		{
			GUILayout.Space(2f);
			GUILayout.Label(isPresent ? "\u2714" : "\u2718", "TL SelectionButtonNew", GUILayout.Height(20f));
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();

		GUI.color = Color.white;

		if (isPresent)
		{
			DrawPreview(myKey);
		}
		else if (mKeys != null && !string.IsNullOrEmpty(myKey))
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(80f);
			GUILayout.BeginVertical();
			GUI.backgroundColor = new Color(1f, 1f, 1f, 0.35f);

			int matches = 0;

			for (int i = 0, imax = mKeys.Count; i < imax; ++i)
			{
				if (mKeys[i].StartsWith(myKey, System.StringComparison.OrdinalIgnoreCase) || mKeys[i].Contains(myKey))
				{
#if UNITY_3_5
			 					if (GUILayout.Button(mKeys[i] + " \u25B2"))
#else
					if (GUILayout.Button(mKeys[i] + " \u25B2", "CN CountBadge"))
#endif
					{
						sp.stringValue = mKeys[i];
						GUIUtility.hotControl = 0;
						GUIUtility.keyboardControl = 0;
					}

					if (++matches == 8)
					{
						GUILayout.Label("...and more");
						break;
					}
				}
			}
			GUI.backgroundColor = Color.white;
			GUILayout.EndVertical();
			GUILayout.Space(22f);
			GUILayout.EndHorizontal();
		}
	}
}
