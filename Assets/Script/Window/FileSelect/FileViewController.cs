using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileViewController : MonoBehaviour {

	[SerializeField]
	private GameObject content = null;
	[SerializeField]
	private GameObject nodeObj = null;
	[SerializeField]
	private FileInputFieldManager fifMan = null;
	[SerializeField]
	private InputField nameIf = null;
	[SerializeField]
	private Button addFolderButton = null;

	private string fileName;
	private bool flag = true;

	// Use this for initialization
	void Start () {
		addFolderButton.onClick.AddListener (() => AddFolder ());
	}
	
	// Update is called once per frame
	void Update () {
		if (flag) {
			UpdateView ();
			flag = false;
		}
	}


	// ScrollViewの表示を更新する
	void UpdateView() {
		// contentの中身を空にする
		foreach (Transform transform in content.transform)
			Destroy (transform.gameObject);

		// セパレータを設定
		char[] separator = { '/', '\\' };

		// もしフォルダ構造が変更されていたら
		if (!Directory.Exists (fifMan.GetPath ())) {
			fifMan.SetPath ("C:/Users");
		}

		// フォルダを読み込んでcontentに追加
		string[] folders = Directory.GetDirectories (fifMan.GetPath ());
		foreach (string name in folders) {
			string[] tmp = name.Split (separator);
			string s = tmp [tmp.Length - 1];
			GameObject obj = Instantiate (nodeObj, content.transform);
			FileNode fn = obj.GetComponent<FileNode> ();
			fn.Set (true, s);
			fn.fifMan = this.fifMan;
			fn.fvc = this;
			fn.nameIf = nameIf;
		}

		// ファイルを読み込んでcontentに追加
		string[] files;
		if (fifMan.key == ProjectData.FileKey.Read)
			files = Directory.GetFiles (fifMan.GetPath (), "*-fv.csv");
		else if (fifMan.key == ProjectData.FileKey.Formula)
			files = Directory.GetFiles (fifMan.GetPath (), "*.txt");
		else if (fifMan.key == ProjectData.FileKey.Image)
			files = Directory.GetFiles (fifMan.GetPath (), "*.png");
		else
			files = Directory.GetFiles (fifMan.GetPath (), "*.csv");
		foreach (string name in files) {
			string[] tmp = name.Split (separator);
			string s = tmp [tmp.Length - 1];
			GameObject obj = Instantiate (nodeObj, content.transform);
			FileNode fn = obj.GetComponent<FileNode> ();
			fn.Set (false, s);
			fn.fifMan = this.fifMan;
			fn.fvc = this;
			fn.nameIf = nameIf;
		}
	}

	public void CallForUpdateView() {
		flag = true;
	}

	public void AddFolder () {
		if (Directory.Exists (fifMan.GetPath () + "/" + fifMan.GetName ()))
			return;

		Directory.CreateDirectory (fifMan.GetPath () + "/" + fifMan.GetName ());
		UpdateView ();
	}
}
