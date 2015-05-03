using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Linewars.Data{
	[Serializable]
	public class Entry
	{

		public enum CharType{
			Red = 0, Green = 1, Blue = 2
		}
		
		///<summary>image of the entry</summary>
		[SerializeField]
		public Texture2D texture;
		///<summary>hp</summary>
		[SerializeField]
		public int heart;
		///<summary>attack Value</summary>
		[SerializeField]
		public int sword;
		///<summary>Defense Value</summary>
		[SerializeField]
		public int shield;
		///<summary>Character Type</summary>
		[SerializeField]
		public CharType characterType;
		/// <summary>Name</summary>
		[SerializeField]
		public string charName;
		
		public override string ToString ()
		{
			return string.Format ("[Entity] charName: {0}m heart: {1}, sword: {2}, shield: {3}, characterType {4}", charName, heart, sword, shield, characterType);
		}

		public virtual string ToCSV(){
			string s = "";
			#if UNITY_EDITOR
			s = string.Format ("Entity;{0};{1};{2};{3};{4};{5};", charName, heart, sword, shield, (int)characterType,((texture == null)?"":UnityEditor.AssetDatabase.GetAssetPath(texture)));
			#endif
			return s;
		}



		public virtual void FromCSV(string s){
			string[] parts = s.Split(';');
			if(parts.Length > 0){
				charName = parts[1];
				heart = int.Parse(parts[2]);
				sword = int.Parse(parts[3]);
				shield = int.Parse(parts[4]);
				characterType = (CharType)Enum.Parse(typeof (CharType), parts[5]);
				#if UNITY_EDITOR
				if(!parts[6].Equals("")){
					string re = "Resources/";
					if(parts[6].StartsWith(re)){
						int index = parts[6].LastIndexOf(".");
						if(index != -1){
							parts[6] = parts[6].Substring(0,index);
						}
						texture = Resources.Load(parts[6].Substring(re.Length,parts[6].Length - re.Length)) as Texture2D;
					}else{
						re = "Assets/";
						if(parts[6].StartsWith(re)){
							texture = UnityEditor.AssetDatabase.LoadAssetAtPath(parts[6], typeof(Texture2D)) as Texture2D;
						}
					}
				}
				#endif
			}
		}
	}
}

