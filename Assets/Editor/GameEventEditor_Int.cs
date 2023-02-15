using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent_Int))]
public class GameEventIntEditor : Editor
{
	int intToRaise;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GameEvent_Int gameEvent = (GameEvent_Int) target;

		intToRaise = EditorGUILayout.IntField("Select number to raise: ", intToRaise);
		if (GUILayout.Button("Raise")) { gameEvent.Raise(intToRaise); }		
		if (GUILayout.Button("Remove all listeners")) { gameEvent.ClearAllListeners(); }
	}
}

