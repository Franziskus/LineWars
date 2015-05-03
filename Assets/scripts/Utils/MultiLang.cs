using UnityEngine;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// This class handles internationalization. 
	/// </summary>
	public class MultiLang : MonoBehaviour {

		public string defaultLanguage = "English";

		private IDictionary<string, string> currentDic;
		private IDictionary<string, IDictionary<string, string>> languages;
		private string currentLang;
		public string[] languagesNames;


		public string activeLang{
			get{
				return currentLang;
			}
			set{
				if(IsKnownLanguage(value)){
					currentLang = value;
					currentDic = languages[currentLang];
				}
			}
		}


		void Awake(){
			currentLang = defaultLanguage;
		}

		void Start () {
			Debug.Log("LoadMultiLang");
			Load();
			string systemLang = Application.systemLanguage.ToString();
			if(systemLang != null && IsKnownLanguage(systemLang))
				currentLang = systemLang;
			currentDic = languages[currentLang];

		}

		public int GetCurrentLanaguageNr(){
			for(int i = 0; i < languagesNames.Length; i++){
				if(currentLang.Equals(languagesNames[i]))
				   return i;
			}
			return -1;
		}

		private bool IsKnownLanguage(string name){
			if(name == null)
				return false;
			return languages.ContainsKey(name);
		}

		public bool Load(){
			string path = null;
			if(Application.isWebPlayer || Application.isMobilePlatform){
				path = "lang";
				Debug.Log("load Mobile or Web");
				languages = XmlHelper.LoadXMLFolderOnline(path);
			}else{
				Debug.Log("load Else");
				path = XmlHelper.GetApplicationPath() + "/Resources/lang";
				languages = XmlHelper.LoadXMLFolder(path);
			}
			if(languages == null){
				Debug.LogWarning("Can't find Languages for \""+path+"\"");
				languages = new Dictionary<string,IDictionary<string,string>>();
				return false;
			}else{
				string foundLangs = "Founded Languages: ";
				string[] temp = new string[languages.Count];
				int i = 0;
				foreach(string key in languages.Keys){
					temp[i++] = key;
					foundLangs += key+" ";
				}
				languagesNames = temp;
				Debug.Log(foundLangs);
				return true;
			}
		}

		/// <summary>
		/// Gets the text in the current language by entering the key.
		/// </summary>
		/// <returns>The text.</returns>
		/// <param name="orginal">Orginal.</param>
		public string GetText(string orginal){
			return GetText(orginal, new string[0]);
		}

		public string GetText(string orginal, System.Object value){
			return GetText(orginal, new string[]{value.ToString()});
		}

		private string GetText(string orginal, params  System.Object[] values){
			string back = orginal;
			if(currentDic != null && currentDic.ContainsKey(orginal)){
				back = currentDic[orginal];
			}else{
				Debug.LogWarning("Can't find Entry for \""+orginal+"\".");
			}
			if(values.Length > 0)
				back = string.Format(back, values);
			return back;
		}
	}
}

