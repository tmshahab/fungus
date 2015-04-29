using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Rotorz.ReorderableList;

namespace Fungus
{

	[CustomEditor(typeof(Localization))]
	public class LocalizationEditor : Editor 
	{
		protected SerializedProperty activeLanguageProp;
		protected SerializedProperty localizationFileProp;

		protected virtual void OnEnable()
		{
			activeLanguageProp = serializedObject.FindProperty("activeLanguage");
			localizationFileProp = serializedObject.FindProperty("localizationFile");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			Localization localization = target as Localization;

			EditorGUILayout.PropertyField(activeLanguageProp);
			EditorGUILayout.PropertyField(localizationFileProp);

			GUILayout.Space(10);

			EditorGUILayout.HelpBox("Exports a localization csv file to disk. You should save this file in your project assets and then set the Localization File property above to use it.", MessageType.Info);

			if (GUILayout.Button(new GUIContent("Export Localization File")))
			{
				ExportLocalizationFile(localization);
			}

			GUILayout.Space(10);

			EditorGUILayout.HelpBox("Exports all standard text in the scene to a text file for easy editing in a text editor. Use the Import option to read the standard text back into the scene.", MessageType.Info);

			if (GUILayout.Button(new GUIContent("Export Standard Text")))
			{
				ExportStandardText(localization);
			}

			if (GUILayout.Button(new GUIContent("Import Standard Text")))
			{
				ImportStandardText(localization);
			}

			serializedObject.ApplyModifiedProperties();
		}

		public virtual void ExportLocalizationFile(Localization localization)
		{
			string path = EditorUtility.SaveFilePanel("Export Localization File", "Assets/",
			                                          "localization.csv", "");
			if (path.Length == 0) 
			{
				return;
			}

			string csvData = localization.GetCSVData();			
			File.WriteAllText(path, csvData);
			AssetDatabase.Refresh();

			ShowNotification(localization);
		}

		public virtual void ExportStandardText(Localization localization)
		{
			string path = EditorUtility.SaveFilePanel("Export Standard Text", "Assets/", "standard.txt", "");
			if (path.Length == 0) 
			{
				return;
			}
			
			string textData = localization.GetStandardText();			
			File.WriteAllText(path, textData);
			AssetDatabase.Refresh();

			ShowNotification(localization);
		}
		
		public virtual void ImportStandardText(Localization localization)
		{
			string path = EditorUtility.OpenFilePanel("Import Standard Text", "Assets/", "txt");
			if (path.Length == 0) 
			{
				return;
			}

			string textData = File.ReadAllText(path);
			localization.SetStandardText(textData);

			ShowNotification(localization);
		}

		protected virtual void ShowNotification(Localization localization)
		{
			FlowchartWindow.ShowNotification(localization.notificationText);
			localization.notificationText = "";
		}
	}

}
