using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Linewars.Data{
	[CustomPropertyDrawer(typeof(Entry))]
	public class EntryDrawer : PropertyDrawer
	{
		//private bool faltout = false;


		public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
		{
			var textureT = prop.FindPropertyRelative("texture");
			Texture2D texture = textureT.objectReferenceValue as Texture2D;
			var heart = prop.FindPropertyRelative("heart");
			var sword = prop.FindPropertyRelative("sword");
			var shield = prop.FindPropertyRelative("shield");
			var characterType = prop.FindPropertyRelative("characterType");
			var charName = prop.FindPropertyRelative("charName");



			float step = 50;
			if(pos.width > 50)
				step = (pos.width - 25) / 4;
			EditorGUI.BeginChangeCheck ();
			prop.isExpanded  = EditorGUI.Foldout(pos, prop.isExpanded, "Entity: "+charName.stringValue);


			if(prop.isExpanded){
				Texture2D temp;
				//int heart;
				//int sword;
				//int shield;
				//Entity.CharType characterType;
				EditorGUILayout.BeginHorizontal();
				{
					temp = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(64), GUILayout.MinWidth(64), GUILayout.MaxWidth(64),GUILayout.Height(64), GUILayout.MinHeight(64), GUILayout.MaxHeight(64));
					
					EditorGUILayout.BeginVertical();
					{
						EditorGUILayout.LabelField("Name: ");
						//string charName = EditorGUILayout.TextField( e.charName);
						EditorGUILayout.PropertyField(charName,GUIContent.none);
						EditorGUILayout.BeginHorizontal();
						{
							EditorGUILayout.BeginVertical();
							{
								EditorGUILayout.LabelField("Heart:", GUILayout.MaxWidth(step));
								EditorGUILayout.PropertyField(heart,GUIContent.none, GUILayout.MaxWidth(step));
							}
							EditorGUILayout.EndVertical();
							EditorGUILayout.BeginVertical();
							{
								EditorGUILayout.LabelField("Sword:", GUILayout.MaxWidth(step));
								EditorGUILayout.PropertyField(sword,GUIContent.none, GUILayout.MaxWidth(step));
							}
							EditorGUILayout.EndVertical();
							EditorGUILayout.BeginVertical();
							{
								EditorGUILayout.LabelField("Shield:", GUILayout.MaxWidth(step));
								EditorGUILayout.PropertyField(shield,GUIContent.none, GUILayout.MaxWidth(step));
							}
							EditorGUILayout.EndVertical();
							EditorGUILayout.BeginVertical();
							{
								EditorGUILayout.LabelField("Type:", GUILayout.MaxWidth(step));
								EditorGUILayout.PropertyField(characterType,GUIContent.none, GUILayout.MaxWidth(step));
							}
							EditorGUILayout.EndVertical();
						}
						EditorGUILayout.EndHorizontal();

						if (EditorGUI.EndChangeCheck ()){
							prop.FindPropertyRelative("texture").objectReferenceValue = temp;
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndHorizontal();
			}

			/*
			// Draw health
			EditorGUI.IntSlider(
				new Rect(pos.x, pos.y, pos.width - kColorWidth, pos.height),
				health, kHealthMin, kHealthMax, label);

			// Draw color
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			EditorGUI.PropertyField(
				new Rect(pos.width - kColorWidth, pos.y, kColorWidth, pos.height),
				color, GUIContent.none);
			EditorGUI.indentLevel = indent;
			*/
		}
	}
}