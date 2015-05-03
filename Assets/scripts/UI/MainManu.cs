using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Utils;
using Linewars.Control;
using Linewars.GameLogic;

namespace Linewars.Ui
{
	/// <summary>
	/// This class handles the start menu.
	/// </summary>
	public class MainManu : MonoBehaviour {

		/// <summary>
		/// Reference to resume button.
		/// </summary>
		public Button resumeButton;
		/// <summary>
		/// Reference to the keyboard image and text;
		/// </summary>
		public GameObject keyboard;
		/// <summary>
		/// Reference to the alternative keyboard image and text;
		/// </summary>
		public GameObject altKeyboard;
		/// <summary>
		/// Reference to the touchscreen image and text;
		/// </summary>
		public GameObject smartphone;
		/// <summary>
		/// Reference to play text. 
		/// After Playing the first time it will chage to resart.
		/// </summary>
		public TextLangChange playText;

		/// <summary>
		/// Reference controls manager to switch the controls
		/// </summary>
		private ControlsManager controlsManager;
		/// <summary>
		/// Reference to the game manager. Interessting for game states.
		/// </summary>
		private GameManager gameManager;
		/// <summary>
		/// Array of all textBoxes to change them when the user decides to change the language.
		/// </summary>
		public TextLangChange[] textBoxes;

		/// <summary>
		/// Reference to multilanguage. This class handels loading languages etc.
		/// </summary>
		private Utils.MultiLang multiLang;

		/// <summary>
		/// Internal Counter fo remember the number of the current language. 
		/// </summary>
		private int langCounter = 0;

		void Start(){
			MainHelper mh = MainHelper.Instance;
			gameManager = mh.Get<GameManager>();
			controlsManager = mh.Get<ControlsManager>();
			multiLang = mh.Get<MultiLang>();
			resumeButton.interactable = false;

			//lookup all TextLangChange to find all Labels that sould change wenn User change language 
			textBoxes = UnityEngine.Object.FindObjectsOfType<TextLangChange>();
			langCounter = multiLang.GetCurrentLanaguageNr();
			ShowControls();
			UpdateLanguage();
		}

		void OnEnable() {
			// When the user Pauses the Game we changin Play to restart and enable resume Button.
			if(gameManager != null){
				resumeButton.interactable = gameManager.GetState() == GameManager.GameState.PAUSE;
				playText.startText = "Restart";
			}
			UpdateLanguage();
		}

		void OnApplicationPause(bool pauseStatus) {
			// If an event Pauses the game. For example User gets a phone call we also need to update
			// Resart Buttons.
			OnEnable();
		}

		/// <summary>
		/// Restart the game.
		/// </summary>
		public void Restart(){
			gameManager.SwitchState(GameManager.GameState.RESTART);
		}

		/// <summary>
		/// Resume the game.
		/// </summary>
		public void Resume(){
			gameManager.SwitchState(GameManager.GameState.RUN);
		}

		/// <summary>
		/// Cycle between controls.
		/// </summary>
		public void SwitchControls(){
			if(controlsManager.IsTouchControlled()){
				controlsManager.Activate(new KeyboardControlled());
			}else if(controlsManager.IsNormalKeyboard()){
				controlsManager.Activate(new KeyboardControlled2());
			}else{
				controlsManager.Activate(controlsManager.touchControlled);
			}
			ShowControls();
		}

		/// <summary>
		/// Enable and disable keyboard, altKeboard, touchscreen images dependent on the current configuration.
		/// </summary>
		public void ShowControls(){
			keyboard.SetActive(false);
			altKeyboard.SetActive(false);
			smartphone.SetActive(false);
			if(controlsManager.IsTouchControlled()){
				smartphone.SetActive(true);
			}else if(controlsManager.IsNormalKeyboard()){
				keyboard.SetActive(true);
			}else{
				altKeyboard.SetActive(true);
			}
		}

		/// <summary>
		/// Cycle between languages.
		/// </summary>
		public void ChangeLanguage(){
			langCounter = ++langCounter % multiLang.languagesNames.Length;
			multiLang.activeLang = multiLang.languagesNames[langCounter];
			Debug.Log("langCounter"+langCounter);
			UpdateLanguage();
		}


		/// <summary>
		/// Updates the language for all TextLangChange in this scene.
		/// </summary>
		public void UpdateLanguage(){
			foreach(TextLangChange tb in textBoxes){
				string s = multiLang.GetText(tb.Identifier());
				if(s == null || s.Equals(""))
					Debug.LogWarning("\""+tb.Identifier()+"\" can't find Text for \""+tb.name+"\".");
				tb.SetText(s);
			}
		}

		/// <summary>
		/// Exit the game
		/// </summary>
		public void Exit(){
			Application.Quit();
		}
	}
}



