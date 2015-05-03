using UnityEngine;
using UnityEditor;

namespace Linewars.Data{
	[CustomEditor(typeof(EntriesDatabase))]
	public class EntriesDatabaseEditor : Editor
	{
		private void ListGUI(SerializedProperty prop, string title){
			EditorGUILayout.LabelField(title);
			EditorGUI.BeginChangeCheck ();
			foreach (SerializedProperty element in prop)
			{
				EditorGUILayout.PropertyField (element);
			}
			if (GUILayout.Button("Add "+title))
			{
				prop.arraySize++;
			}
			
			if (GUILayout.Button("Remove "+title))
			{
				prop.arraySize--;
			}
			if (GUILayout.Button("export "+title+".csv"))
			{
				Export((EntriesDatabase)target, title);
			}
			if (GUILayout.Button("import "+title+".csv"))
			{
				Import((EntriesDatabase)target, title);
			}
		}

		public override void OnInspectorGUI ()
		{
			var entries = serializedObject.FindProperty("_heroes");
			ListGUI(entries, "Hero");
			var enemies = serializedObject.FindProperty("_enemies");
			ListGUI(enemies, "Enemy");
			EditorGUILayout.LabelField("Extra");
			if (GUILayout.Button("Debug"))
			{
				Debug.Log(target.ToString());
			}


			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();
		}

		private void Export(EntriesDatabase ed, string title){
			var path = EditorUtility.SaveFilePanel("Export "+title+" to CSV",
			                                       "",
			                                       title+".csv",
			                                       "csv");
			if(path.Length != 0) {
				if(title.Equals("Hero"))
					System.IO.File.WriteAllText(path, ed.HeroesToCSV());
				else
					System.IO.File.WriteAllText(path, ed.EnemiesToCSV());

			}
		}

		private void Import(EntriesDatabase ed, string title){
			var path = EditorUtility.OpenFilePanel("Import "+title+" to CSV",
			                                       "",
			                                       "csv");
			if(path.Length != 0) {
				string readed = System.IO.File.ReadAllText(path);
				if(title.Equals("Hero"))
					ed.HeroesFromCSV(readed);
				else
					ed.EnemiesFromCSV(readed);
			}
			Repaint();
		}
	}
}
