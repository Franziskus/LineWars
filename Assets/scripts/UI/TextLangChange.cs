using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace Linewars.Ui
{
	/// <summary>
	/// Internationalization script for Ui.Text labels.
	/// </summary>
	public class TextLangChange : MonoBehaviour, IChangeLanguage {


		private Text text;
		[HideInInspector]
		public string startText;

		void Awake () {
			text = GetComponent<Text>();
			startText = text.text;
		}
		
		public string Identifier(){
			return startText;
		}
		
		public void SetText(string text){
				this.text.text = text;
		}
	}
}
