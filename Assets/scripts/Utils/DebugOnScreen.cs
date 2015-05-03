using UnityEngine;
using System.Collections;

namespace Utils
{
	/// <summary>
	/// This adds a debug console to the screen. Can be handy when you don't want to setup an complete android IDE.
	/// </summary>
	public class DebugOnScreen : MonoBehaviour {

		public bool showDubbels = false;
		private bool show = false;
		private Rect outSide;
		private Rect inSide;
		private string[] texts = new string[COUNT];
		private int nr = 0; 

		private string lastLog;
		private string lastStackTrace;
		private LogType lastType;

		private string text;

		private const int COUNT = 15;
		// Use this for initialization
		void Start () {
			if(Debug.isDebugBuild || Application.isEditor){
				outSide = new Rect(0,0, Screen.width, Screen.height / 2);
				inSide = new Rect(0,0, Screen.width, 50);
				Application.RegisterLogCallback(HandleLog);
			}
		}

		private void HandleLog (string logString, string  stackTrace, LogType logType) { 
			if(showDubbels || 
			   !(logString.Equals(lastLog) && logType.Equals(lastType) )){ //&& stackTrace.Equals(lastStackTrace) )){
				int pos = nr++ % COUNT;
				texts[pos] = logType.ToString() + ": "+ logString;

				lastType = logType;
				lastStackTrace = stackTrace;
				lastLog = logString;

				text = "";
				for(pos = nr -1; pos > nr -(COUNT+1) && pos >= 0; pos--){
					text = texts[pos % COUNT] + "\n" + text;
				}
			}

		}
		
		void OnGUI(){
			if(Debug.isDebugBuild || Application.isEditor){
				GUI.skin.button.alignment = TextAnchor.UpperLeft;
				if(show){
					if(GUI.Button(outSide, text)){
						show = !show;
					}
				}else{
					if(GUI.Button(inSide, text)){
						show = !show;
					}
				}
			}
		}

		public static string GetGameObjectPath(GameObject obj)
		{
			string path = "/" + obj.name;
			while (obj.transform.parent != null)
			{
				obj = obj.transform.parent.gameObject;
				path = "/" + obj.name + path;
			}
			return path;
		}
	}
}