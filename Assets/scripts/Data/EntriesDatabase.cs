using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Linewars.Data{

	/// <summary>
	/// Entries database. This class helps to create EntriesDatabase to store 
	/// all necessary Entries in an asset.
	/// </summary>

	[Serializable]
	public class EntriesDatabase : ScriptableObject
	{
		[SerializeField]
		private List<Entry> _heroes;

		[SerializeField]
		private List<Entry> _enemies;

		public void OnEnable()
		{
			if (_heroes == null)
				_heroes = new List<Entry>();

			if (_enemies == null)
				_enemies = new List<Entry>();
		}

		public void AddHero (Entry spawnEntry)
		{
			_heroes.Add(spawnEntry);
		}

		public void AddEnemy (Entry spawnEntry)
		{
			_enemies.Add(spawnEntry);
		}


		public Entry GetRandomHero ()
		{
			return _heroes[Random.Range (0, _heroes.Count)];
		}

		public List<Entry> GetHeros()
		{
			return _heroes;
		}

		public List<Entry> GetEnemies()
		{
			return _enemies;
		}

		public string HeroesToCSV(){
			return ToCSV(_heroes);
		}

		public string EnemiesToCSV(){
			return ToCSV(_enemies);
		}

		public void HeroesFromCSV(string s){
			FromCSV(s, _heroes);
		}
		
		public void EnemiesFromCSV(string s){
			FromCSV(s, _enemies);
		}

		public static string ToCSV(List<Entry> list){
			string s = "";
			foreach(Entry outS in list){
				s += outS.ToCSV() + "\n";
			}
			return s;
		}

		public static void FromCSV(string s, List<Entry> list){
			string[] parts = s.Split('\n');
			list.Clear();
			foreach(string part in parts){
				if(!part.Trim().Equals("")){
					Entry e = new Entry();
					e.FromCSV(part);
					list.Add(e);
				}
			}
		}

		public override string ToString(){
			string s = "Heroes:\n";
			foreach(Entry e in _heroes){
				s += e.ToString()+"\n";
			}
			s += "Enemies:\n";
			foreach(Entry e in _enemies){
				s += e.ToString()+"\n";
			}
			return s;
		}
	}
}

