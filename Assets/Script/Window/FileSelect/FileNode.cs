using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileNode : MonoBehaviour {

	[SerializeField]
	private GameObject file = null;
	[SerializeField]
	private GameObject folder = null;
	[SerializeField]
	private Text text = null;

	[System.NonSerialized]
	public bool isFolder = false;
	[System.NonSerialized]
	public FileInputFieldManager fifMan;
	[System.NonSerialized]
	public FileViewController fvc;
	[System.NonSerialized]
	public InputField nameIf;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// FileNodeにフォルダかどうかと名前を与える
	public void Set(bool b, string name) {
		isFolder = b;
		text.text = name;

		// アイコンを切り替える
		if (isFolder) {
			file.SetActive (false);
			folder.SetActive (true);
		} else {
			file.SetActive (true);
			folder.SetActive (false);
		}
	}


	// 名前を返す
	public string GetName() {
		return text.text;
	}


	// 押されたときの処理
	public void Excute() {
		if (isFolder) {
			fifMan.GoNext (GetName ());
			fvc.CallForUpdateView ();
		} else {
			nameIf.text = GetName ();
		}
	}
}
