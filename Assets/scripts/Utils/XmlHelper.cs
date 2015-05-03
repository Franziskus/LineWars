using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Utils
{

/// <summary>
/// This class reads xml files from disk or resource folder.
/// </summary>
public class XmlHelper : MonoBehaviour {

	/// <summary>
	/// This Class is one of the ugliest workarounds I´ve ever seen.
	/// Because IDictionary can't be written via XmlSerializer because it would be written via
	/// Dat­a­Con­tract­Se­ri­al­izer but this is not implemented in Unity/Mono.  We have to use this 
	/// List<DataItem> thing.
	/// </summary>
	public class DataItem
	{
   		public string Key;
 		public string Value;
    	
		public DataItem(){
		}
		
		public DataItem(string key, string value)
   		{
      		Key = key;
      		Value = value;
		}	
	}

	public const string FLOAT_STRING_REDEX = @"\-?\d+\.?\d*E?-?\d*";

	public static string GetApplicationPath(){
		string path = Application.dataPath;
		if (Application.platform == RuntimePlatform.OSXPlayer) {
			path += "/../../";
		}
		else if (Application.platform == RuntimePlatform.WindowsPlayer) {
			path += "/../";
		}
		return path;
	}

	public static string AddSlashToEnd(string folderPath){
		if(!folderPath.EndsWith("\\") && !folderPath.EndsWith("/")){
			folderPath = folderPath + "/";
		}
		return folderPath;
	}

	
	
	public static IDictionary<string, string> LoadXMLOnline(string path){
		Debug.Log("Try to load \""+path+"\".");
		UnityEngine.Object ele = Resources.Load(path);
		if(ele is TextAsset && ((TextAsset)ele).text.StartsWith("<?xml")){
			IDictionary<string,string> method = LoadXMLFromString(((TextAsset)ele).text);
			return method;
		} 
		return null;
	}

	public static IDictionary<string, IDictionary<string,string>> LoadXMLFolderOnline(string folderPath){
		IDictionary<string, IDictionary<string,string>> back = new Dictionary<string,IDictionary<string,string>>();
		UnityEngine.Object[] eles =  Resources.LoadAll(folderPath);
		foreach(UnityEngine.Object ele in eles){
				Debug.Log("ele "+ele.name);
			if(ele is TextAsset && ((TextAsset)ele).text.StartsWith("<?xml")){
				IDictionary<string,string> method = LoadXMLFromString(((TextAsset)ele).text);
				back.Add(ele.name, method);
			}
		}
		return back;
	}

	public static IDictionary<string, IDictionary<string,string>> LoadXMLFolder(string folderPath){
		string[] files = Directory.GetFiles(folderPath);
		IDictionary<string, IDictionary<string,string>> back = new Dictionary<string,IDictionary<string,string>>();
		foreach(string file in files){

			if(File.Exists(file) && (file.EndsWith(".xml") || file.EndsWith(".XML"))){
				string fname = Path.GetFileNameWithoutExtension(file);
				back.Add(fname, LoadXML(file));
			}
		}
		return back;
	}

	public static IDictionary<string,string> LoadXMLFromString(string text)
	{
		Dictionary<string, string> myDictionary = new Dictionary<string, string>();
		XmlSerializer xs = new XmlSerializer(typeof(List<DataItem>));
		TextReader tr = new StringReader(text);
		List<DataItem> templist = (List<DataItem>)xs.Deserialize(tr);
			
		foreach (DataItem di in templist)
		{
			myDictionary.Add(di.Key, di.Value);
		}
		tr.Close();
		return myDictionary;
	}

	public static IDictionary<string,string> LoadXML(string fullpath)
	{
   		Dictionary<string, string> myDictionary = new Dictionary<string, string>();
 		if(File.Exists(fullpath)){
			XmlSerializer xs = new XmlSerializer(typeof(List<DataItem>));
			TextReader tr = new StreamReader(fullpath); 
			List<DataItem> templist = (List<DataItem>)xs.Deserialize(tr);
 
  			foreach (DataItem di in templist)
   			{
       			myDictionary.Add(di.Key, di.Value);
   			}
			tr.Close();
		}else{
			Debug.LogWarning("File "+fullpath +" not found!");
		}
		return myDictionary;
	}
	
	public static string convertFloat(float v){
		return v.ToString();
	}
		
	public static float convertFloat(String v){
		Regex rx = new Regex(FLOAT_STRING_REDEX);
		Match m = rx.Match(v);
		if(m.Success){
			return ParseFloat(v);
		}else{
			Debug.LogWarning("Can't Parse Float \""+v+"\"");
			return 0;
		}

	}

	public static string convertVector2(Vector2 v){
		return "("+v.x+","+v.y+")";
	}
	
	public static Vector2 convertVector2(String v){
		Regex rx = new Regex("\\(("+FLOAT_STRING_REDEX+"),("+FLOAT_STRING_REDEX+")\\)");
		Match m = rx.Match(v);
			return new Vector2(ParseFloat(m.Groups[1].ToString()),ParseFloat(m.Groups[2].ToString()));
	}

	public static string convertVector3(Vector3 v){
		return "("+v.x+","+v.y+","+v.z+")";
	}

	public static string convertQuaternion(Quaternion v){
		return "("+v.x+","+v.y+","+v.z+","+v.w+")";
	}
	
	public static bool ParseBool(string text){
			return bool.Parse(text);
	}

	public static bool convertBool(string boolean){
			return ParseBool(boolean);
	}

	public static string convertBool(bool boolean){
			return boolean.ToString();
	}

	public static float ParseFloat(string text, string extra = ""){
			try{
				return float.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
			}catch(FormatException e){
				Debug.LogError("Can't parse \""+text+"\" ("+extra+")"+e.ToString());
				throw e;
			}
	}

	public static Quaternion convertQuaternion(String v){
			Regex rx = new Regex("\\(("+FLOAT_STRING_REDEX+"),("+FLOAT_STRING_REDEX+"),("+FLOAT_STRING_REDEX+"),("+FLOAT_STRING_REDEX+")\\)");
		Match m = rx.Match(v);
		Quaternion back = Quaternion.identity;
		try{
				back = new Quaternion(ParseFloat(m.Groups[1].ToString(), "Group 1"),ParseFloat(m.Groups[2].ToString(), "Group 2"),ParseFloat(m.Groups[3].ToString(), "Group 3"),ParseFloat(m.Groups[4].ToString(), "Group 4"));
		}catch(FormatException e){
				Debug.LogError("Can't convert \""+v+"\" x:\""+m.Groups[1].ToString()+"\"? y:\""+m.Groups[2].ToString()+"\"? z:\""+m.Groups[3].ToString()+"\"?"+"\"? w:\""+m.Groups[4].ToString()+"\"? \n ");		
		}
		return back;
	}

	public static string convertQuaternionArray(Quaternion[] arr){
		string s = "";
		foreach(Quaternion v in arr){
			s += "["+convertQuaternion(v)+"]";
		}
		return s;
	}

	public static bool[] convertBoolArray(string arr){
			List<bool> l = new List<bool>();
			Regex rx = new Regex(@"[^\]|\[]+");
			Match m = rx.Match(arr);
			while(m.Success){
				l.Add(ParseBool(m.Value));
				m = m.NextMatch();
			}
			bool[] back = new bool[l.Count];
			l.CopyTo(back,0);
			return back;
	}

	public static string convertBool2Array(bool[] arr){
		string s = "";
		foreach(bool v in arr){
			s += "["+convertBool(v)+"]";
		}
		return s;
	}

	public static Quaternion[] convertQuaternionArray(string arr){
		List<Quaternion> l = new List<Quaternion>();
		Regex rx = new Regex(@"[^\]|\[]+");
		Match m = rx.Match(arr);
		while(m.Success){
			l.Add(convertQuaternion(m.Value));
			m = m.NextMatch();
		}
		Quaternion[] back = new Quaternion[l.Count];
		l.CopyTo(back,0);
		return back;
	}

	public static Vector3 convertVector3(String v){
		Regex rx = new Regex(@"\((\-?\d+\.?\d*),(\-?\d+\.?\d*),(\-?\d+\.?\d*)\)");
		Match m = rx.Match(v);
		Vector3 back = new Vector3(0f,0f,0f);
		try{
			back = new Vector3(ParseFloat(m.Groups[1].ToString()),ParseFloat(m.Groups[2].ToString()),ParseFloat(m.Groups[3].ToString()));
		}catch(FormatException e){
			Debug.LogError("Can't convert \""+v+"\" x:\""+m.Groups[1].ToString()+"\"? y:\""+m.Groups[2].ToString()+"\"? z:\""+m.Groups[3].ToString()+"\"?");
		}
		return back;
	}
	
	public static string convertVector2Array(Vector2[] arr){
		string s = "";
		foreach(Vector2 v in arr){
			s += "["+convertVector2(v)+"]";
		}
		return s;
	}
	
	public static Vector2[] convertVector2Array(string arr){
		List<Vector2> l = new List<Vector2>();
		Regex rx = new Regex(@"[^\]|\[]+");
		Match m = rx.Match(arr);
		while(m.Success){
			l.Add(convertVector2(m.Value));
			m = m.NextMatch();
		}
		Vector2[] back = new Vector2[l.Count];
		l.CopyTo(back,0);
		return back;
	}

	public static string convertStringArray(string[] arr){
		string s = "";
		foreach(string v in arr){
			s += "["+v+"]";
		}
		return s;
	}

	public static string[] convertStringArray(string arr){
		List<string> l = new List<string>();
		Regex rx = new Regex(@"[^\]|\[]+");
		Match m = rx.Match(arr);
		while(m.Success){
			l.Add(m.Value);
			m = m.NextMatch();
		}
		string[] back = new string[l.Count];
		l.CopyTo(back,0);
		return back;
	}


	public static string convertVector3Array(Vector3[] arr){
		string s = "";
		foreach(Vector3 v in arr){
			s += "["+convertVector3(v)+"]";
		}
		return s;
	}
			
	public static Vector3[] convertVector3Array(string arr){
		List<Vector3> l = new List<Vector3>();
		Regex rx = new Regex(@"[^\]|\[]+");
		Match m = rx.Match(arr);
		while(m.Success){
			l.Add(convertVector3(m.Value));
			m = m.NextMatch();
		}
		Vector3[] back = new Vector3[l.Count];
		l.CopyTo(back,0);
		return back;
	}
	
	
}
}

