using UnityEngine;
using UnityEditor;

namespace Linewars.Data{

	public class CreateEntriesDatabaseAsset
	{
		[MenuItem("Custom/CreateEntriesDatabaseAsset")]
		static void CreateAsset()
		{ 
			var db = ScriptableObject.CreateInstance<EntriesDatabase> ();
			AssetDatabase.CreateAsset (db, "Assets/data/EntriesDatabase.asset");
		}
	}
}