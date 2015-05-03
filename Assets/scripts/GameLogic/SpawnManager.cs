using UnityEngine;
using System.Collections.Generic;
using Linewars.Data;

namespace Linewars.GameLogic
{
	/// <summary>
	/// Spawn manager. Takes care spawning heros and bad guys.
	/// </summary>
	public class SpawnManager : MonoBehaviour, IRestartable, IInformEnable {

		/// <summary>
		/// The hero points are indirect added to currentHeroPoints every second.
		/// </summary>
		public AnimationCurve heroPoints = AnimationCurve.Linear(0,1,1000,10);
		/// <summary>
		/// The enemy points are directly used
		/// </summary>
		public AnimationCurve enemyPoints = AnimationCurve.Linear(0,1,1000,25);
		/// <summary>
		/// If bigger as heroPointsCosts the SpawnManger can spawn new Hero if less than maxHeroCount
		/// </summary>
		private int currentHeroPoints = 0;
		public int maxHeroCount = 10;
		public int heroPointsCosts = 10;

		/// <summary>
		/// List of Heros that are not collected by the player
		/// </summary>
		private List<GameObject> watingHeros = new List<GameObject>();
		public GameObject heroPrefab;
		public GameObject enemyPrefab;
		public EntriesDatabase entriesData;

		public Transform root;

		public Color blueColor = Color.blue;
		public Color redColor = Color.red;
		public Color greenColor = Color.green;

		/// <summary>
		/// Map of enemies by Key Cost and value bad guy entry.
		/// </summary>
		public Dictionary<int, List<Entry>> enemys = new Dictionary<int, List<Entry>>();

		public Rect spawnArea = new Rect(3f,3.5f,35, 24);
		public int zpos = -1;


		public  float updateInterval = 1;
		private float timeleft;
		private float gameStart;

		private int lowest = int.MaxValue;
		private int highest = int.MinValue;

		private Player player;
		private GameManager gameManager;

		public void Awake(){
			Utils.MainHelper.Instance.Register(this);
		}

		public void Start(){
			Utils.MainHelper mh = Utils.MainHelper.Instance;
			gameManager = mh.Get<GameManager>();
			player = mh.Get<Player>();
			SetupEnemys();
		}

		public void OnCollectHero(GameObject hero){
			watingHeros.Remove(hero);
		}

		/// <summary>
		/// Setups the enemys. Sort the badguy and calc ther value
		/// then put them in goups.
		/// </summary>
		public void SetupEnemys(){
			List<Entry> orginalList = entriesData.GetEnemies();
			foreach(Entry enemy in orginalList){
				int value = enemy.heart + enemy.shield + enemy.sword;
				if(value > 0){
					if(highest < value)
						highest = value;
					if(lowest > value)
						lowest = value;
					if(enemys.ContainsKey(value)){
						enemys[value].Add(enemy);
					}else{
						List<Entry> l = new List<Entry>();
						l.Add(enemy);
						enemys.Add(value,l);
					}
				}
			}
			string print = "Enemys Count ("+lowest+" - "+highest+")\n";
			foreach(int key in enemys.Keys){
				print += key+": "+ enemys[key].Count + "\n";
			}
			Debug.Log (print);
		}

		public int HerosInGame(){
			return player.HeroCount() + watingHeros.Count;
		}

		public void Restart(){
			gameStart = Time.time;
			currentHeroPoints = 0;
			for(int i = 0; i < root.transform.childCount; i++){
				GameObject.Destroy(root.transform.GetChild(i).gameObject);
			}
		}
		
		void OnEnable(){
		}
		
		public void OnEnable(bool enable){
		}

		void Update(){
			if(TickTime()){
				timeleft -= Time.deltaTime;
				if( timeleft <= 0.0 ){
					timeleft = updateInterval;
					OnUpdateInterval();
				}
			}
		}

		private bool TickTime(){
			return gameManager.GetState() == GameManager.GameState.RUN && !player.IsInFight();
		}

		/// <summary>
		/// This method is called every updateInterval * Second and spawns Heroes and enemies
		/// </summary>
		void OnUpdateInterval(){
			float pastTime = Time.time - gameStart;
			currentHeroPoints += (int)heroPoints.Evaluate(pastTime);

			if(currentHeroPoints >= heroPointsCosts && HerosInGame() <= maxHeroCount){
				Vector3 pos = GetSpawnPos();
				Hero h = SpawnHero(pos);
				watingHeros.Add(h.gameObject);
				currentHeroPoints -= heroPointsCosts;
			}
			int enemyPoins = (int)this.enemyPoints.Evaluate(pastTime);
			Vector3 enPoint = GetSpawnPos();
			SpawnEnemy(enPoint, enemyPoins);
		}

		/// <summary>
		/// Generates a random spawn position.
		/// </summary>
		/// <returns>The spawn position.</returns>
		private Vector3 GetSpawnPos(){
			float x = Random.Range(0,(int)spawnArea.width) + spawnArea.x;
			float y = Random.Range(0,(int)spawnArea.height) + spawnArea.y;
			Vector3 pos = new Vector3(x,y, zpos);
			//TODO test for space
			return pos;
		}


		/// <summary>
		/// Spawn and set up a enemy unit.
		/// </summary>
		/// <returns>The enemy.</returns>
		/// <param name="pos">Position.</param>
		/// <param name="points">Points to spare</param>
		private Enemy SpawnEnemy(Vector3 pos, int points){
			if(points >= lowest && points <= highest){
				List<Entry> es = enemys[points];
				Entry e = es[Random.Range(0, es.Count)];
				GameObject go = (GameObject)GameObject.Instantiate(enemyPrefab);
				Enemy en = go.AddComponent<Enemy>();
				en.SetEntry(e);
				go.transform.position = pos;
				go.transform.parent = root;
				SpriteRenderer sp = go.GetComponent<SpriteRenderer>();
				sp.sprite = Sprite.Create(e.texture, new Rect(0,0, e.texture.width, e.texture.height), new Vector2(0.5f,0.5f));
				go.name = "E_"+e.charName;
				Transform aura =  go.transform.FindChild("AuraNew Sprite");
				aura.gameObject.SetActive(true);
				Material l = aura.gameObject.GetComponent<Renderer>().material;
				switch(e.characterType){
				case Entry.CharType.Blue: l.color = blueColor; break;
				case Entry.CharType.Red: l.color = redColor; break;
				case Entry.CharType.Green: l.color = greenColor; break;
				}
				return en;
			}
			return null;
		}

		/// <summary>
		/// Spawn and setup a hero.
		/// </summary>
		/// <returns>The hero.</returns>
		/// <param name="pos">Position.</param>
		private Hero SpawnHero(Vector3 pos){
			Entry e = entriesData.GetRandomHero();
			GameObject go = (GameObject)GameObject.Instantiate(heroPrefab);
			go.GetComponent<CharacterController>().enabled = false;
			Hero h = go.AddComponent<Hero>();
			h.SetEntry(e);
			go.transform.position = pos;
			go.transform.parent = root;
			SpriteRenderer sp = go.GetComponent<SpriteRenderer>();
			sp.sprite = Sprite.Create(e.texture, new Rect(0,0, e.texture.width, e.texture.height), new Vector2(0.5f,0.5f));
			go.name = "H_"+e.charName;
			CollectHero ch = go.GetComponent<CollectHero>();
			ch.onCollectListener += OnCollectHero;
			Transform aura =  go.transform.FindChild("AuraNew Sprite");
			Material l = aura.gameObject.GetComponent<Renderer>().material;
			switch(e.characterType){
			case Entry.CharType.Blue: l.color = blueColor; break;
			case Entry.CharType.Red: l.color = redColor; break;
			case Entry.CharType.Green: l.color = greenColor; break;
			}
			return h;
		}

		public Hero GetStartHeroEntry(){

			return SpawnHero(transform.position);
		}
	}
}


