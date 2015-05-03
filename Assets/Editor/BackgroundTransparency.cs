using UnityEngine;
using UnityEditor;  
using System.IO;
using System.Threading;
using System.Collections.Generic;

public class BackgroundTransparency : EditorWindow {

	protected Texture2D[] source; 
	protected string path = "";
	protected Color color = Color.black;
	protected Texture2D currentTexture;
	protected Texture2D orginalPreviewTexture;
	 

	[MenuItem("Custom/BackgroundTransparencyEditor")]  
	static void Init()  
	{ 
		BackgroundTransparency window = (BackgroundTransparency)EditorWindow.GetWindow(typeof(BackgroundTransparency));
		window.Reset();
		window.Show(); 
	}

	/// <summary>
	/// Reset current values.
	/// </summary>
	public void Reset(){
		path = Application.dataPath;
		source = null;
		currentTexture = null;
		orginalPreviewTexture = null;
	}


	protected void OnGUI() {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();
		GUILayout.Label ("One Texture:");
		GUILayout.Label ("Folder Path:");
		GUILayout.Label ("Background Color:");

		if(orginalPreviewTexture != null){
			GUILayout.Label("Orginal image alfa cleaned:");
			GUILayout.Label(orginalPreviewTexture);
		}else{
			GUILayout.Label("");
			GUILayout.Label("");
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical();
		Texture2D old = (source == null || source.Length < 1)?null:source[0];
		Texture2D temp = (Texture2D)EditorGUILayout.ObjectField(old, typeof(Texture2D), false);
		if(temp != null && temp != old){
			source = new Texture2D[]{temp};
			path = Application.dataPath;
			orginalPreviewTexture = AlfaCleand(temp);
		}


		if(GUILayout.Button(path)){
			string newPath = OpenDialog(path);
			if(newPath != null && newPath.Length > 0){
				path = newPath;
				source = GetAllTexturesInFolder(path);
				if(source.Length > 0){
					orginalPreviewTexture = AlfaCleand(source[0]);
				}
			}
		}
		color = EditorGUILayout.ColorField("", color);

		if(currentTexture != null){
			GUILayout.Label("Last image output:");
			GUILayout.Label(currentTexture);
		}else{
			GUILayout.Label("");
			GUILayout.Label("");
		}

		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();


		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Reset")) {
			Reset();
		}
		if(GUILayout.Button("make Transparent")) {
			MakeTransparent();
		}
		EditorGUILayout.EndHorizontal();
	}

	/// <summary>
	/// Gets the absolute of an asset.
	/// </summary>
	/// <returns>The absolute asset path.</returns>
	/// <param name="o">An unity asset</param>
	public static string GetAbsoluteAssetPath(Object o){
		string oPath = AssetDatabase.GetAssetPath( o );
		string fullPath = Application.dataPath.Substring(0,Application.dataPath.IndexOf("Assets")) + oPath;
		return fullPath;
	}

	/// <summary>
	/// Sourcing through a folder and returns only the Textures.
	/// </summary>
	/// <returns>The all textures in folder.</returns>
	/// <param name="path">Absolute path of the Folder.</param>
	protected Texture2D[] GetAllTexturesInFolder(string path){
		List<Texture2D> back = new List<Texture2D>();
		string[] files = Directory.GetFiles(path);
		for(int i = 0; i < files.Length; i++){
			string file = files[i].Substring(Application.dataPath.Substring(0,Application.dataPath.IndexOf("Assets")).Length);
			Texture2D currentT = AssetDatabase.LoadAssetAtPath(file, typeof(Texture2D)) as Texture2D;
			if(currentT != null){
				currentT.name = file;
				back.Add(currentT);
			}
			EditorUtility.DisplayProgressBar("Load folder...", "", ((float)i)/ files.Length);
		}
		EditorUtility.ClearProgressBar();
		return back.ToArray();
	}

	/// <summary>
	/// Returns a texture where alpha is 100% at every pixel.
	/// </summary>
	/// <returns>Texture with alfa at 100%</returns>
	/// <param name="source">Source texture.</param>
	protected Texture2D AlfaCleand(Texture2D source){
		string fullPath = GetAbsoluteAssetPath(source);
		byte[] fileBytes = File.ReadAllBytes(fullPath);
		Texture2D target = new Texture2D(source.width, source.height, source.format, false);
		target.LoadImage(fileBytes);
		target.Apply();

		Color32[] pixels =  target.GetPixels32();
		for(int p = 0; p < pixels.Length; p++)
		{ 
			pixels[p].a = 255;;
		}
		target.SetPixels32(pixels);
		target.Apply();
		return target;
	}

	/// <summary>
	/// This method interates through the source array and replace the selected color with transparent color.
	/// </summary>
	protected void MakeTransparent(){
		if(source != null){
			try 
			{
				for(int i = 0; i < source.Length; i++){
					if(EditorUtility.DisplayCancelableProgressBar("Saving ...", 
					                                              i + " of " + source.Length + "("+source[i].name+")", 
					                                              ((float)i)/ source.Length)){
						EditorUtility.ClearProgressBar();
						return;
					}
					
					Texture2D text = MakeTransparent(source[i]);
					currentTexture = text;
				}
			}
			catch(System.NullReferenceException e){
				Debug.LogError("NullReferenceException "+e.StackTrace);
			}
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
		}
	}

	void OnInspectorUpdate() {
		Repaint();
	}

	/// <summary>
	/// Replace selected color with transparent color for this texture.
	/// Also store the texture.
	/// </summary>
	/// <returns>The transparent texture.</returns>
	/// <param name="source">Source Texture2D instace.</param>
	protected Texture2D MakeTransparent(Texture2D source){
		string fullPath = GetAbsoluteAssetPath(source);
		byte[] fileBytes = File.ReadAllBytes(fullPath);

		Texture2D target = new Texture2D(source.width, source.height, TextureFormat.ARGB32, false);
		target.LoadImage(fileBytes);
		target.Apply();

		Color[] pixels =  target.GetPixels();
		for(int p = 0; p < pixels.Length; p++)
		{ 
			Color found = pixels[p];
			found.a = 1;
			if(found.Equals(color)){
				pixels[p] =  new Color32(0,0,0,0);
			}
		}

		target.SetPixels(pixels);
		target.Apply();

		byte[] bytes = target.EncodeToPNG();
		File.WriteAllBytes(fullPath, bytes);
		return target;
	}

	/// <summary>
	/// Opens a FolderDialog. Returns the absolute path.
	/// </summary>
	/// <param name="oldPath">current Path</param>
	/// <returns>Folder path</returns>
	protected string OpenDialog(string oldPath){
		string path = EditorUtility.OpenFolderPanel("Texture Folder", oldPath, "");
		return path;
	}
}
