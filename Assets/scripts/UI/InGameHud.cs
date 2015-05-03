using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Utils;
using Linewars.GameLogic;

namespace Linewars.Ui
{
	/// <summary>
	/// Hud for ingame. For HP of the current Hero and score.
	/// </summary>
	public class InGameHud : MonoBehaviour, IRestartable {


		public Image[] hps;
		public int points;
		public Text pointsText;
		private GameManager gameManager;


		public void Awake(){
			MainHelper.Instance.Register(this);
		}

		public void Start(){
			SetHP(0);
			MainHelper mh = MainHelper.Instance;
			gameManager = mh.Get<GameManager>();
			gameManager.Add(this, GameManager.GameState.RESTART);
		}

		public void Restart(){
			if(gameManager.GetState() != GameManager.GameState.END){
				SetPoints(0);
			}
		}

		/// <summary>
		/// Update the showen HP
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetHP(int value){
			value = Mathf.Max(0, value);
			for(int i = 0; i < value; i++){
				hps[i].enabled = true;
			}
			for(int i = value; i < hps.Length; i++){
				hps[i].enabled = false;
			}
		}

		/// <summary>
		/// Adds points to score.
		/// </summary>
		/// <param name="value">Value.</param>
		public void AddPoints(int value){
			SetPoints(points + value);
		}


		/// <summary>
		/// Sets the score.
		/// </summary>
		/// <param name="value">Value.</param>
		private void SetPoints(int value){
			points = value;
			pointsText.text = value.ToString();
		}
	}
}


