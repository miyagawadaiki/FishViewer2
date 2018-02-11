using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiVariousParameterNode : MonoBehaviour {

	//[SerializeField]
	//private Text buttonText = null;

	[SerializeField]
	private Color pressedColor = Color.white;

	public bool isSelected = false;
	public int number = 0;

	private Image buttonImage, image;
	private Button button;
	private Text text;

	private MultiParameterSettingGroup mpsg;
	private string parameter;
	private string graphName;

	void Awake () {
		mpsg = this.GetComponentInParent<MultiParameterSettingGroup> ();
		int i = (this.transform.parent.GetComponentsInChildren<MultiVariousParameterNode> ().Length - 1) % ProjectData.ColorList.colors.Length;
		parameter = "0 :0,0,1,0,:0,50," + i + ",1,1,10,0,:";

		buttonImage = this.GetComponent<Image> ();
		image = this.GetComponentsInChildren<Image> ()[1];
		button = this.GetComponent<Button> ();
		text = this.GetComponentInChildren<Text> ();
	}

	// Use this for initialization
	void Start () {
		button.onClick.AddListener (() => mpsg.Select (this));
		graphName = "Graph " + number;
		UpdateButtonText ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Select () {
		buttonImage.color = button.colors.pressedColor;
		isSelected = true;
	}

	public void Unselect () {
		buttonImage.color = button.colors.normalColor;
		isSelected = false;
	}

	/*
	public void RegisterMultiParameterSettingGroup(MultiParameterSettingGroup mpsg) {
		this.mpsg = mpsg;
	}
	*/

	public void RegisterParameter (string p) {
		if(!p.Equals(""))
			parameter = p;
		UpdateButtonText ();
		UpdateNodeColor ();
		mpsg.Select (this);
	}
		
	public void UpdateButtonText () {
		char[] separator = { ':', ' ', ',' };
		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		string s = "";
		s += System.Enum.GetNames (typeof(GraphType)) [int.Parse (tmp [0])] + ", ";
		s += "ID:" + (int.Parse(tmp [1]) + 1) + ", ";
		if (int.Parse(tmp[0]) != 1)
			s += "In:" + DataBase.GetShortTags () [int.Parse (tmp [2])] + ", ";
		s += "Out:" + DataBase.GetShortTags () [int.Parse (tmp [3])];

		text.text = graphName + ", " + s;
	}

	public void UpdateNodeColor () {
		char[] separator = { ':', ' ', ',' };
		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		image.color = ProjectData.ColorList.colors[int.Parse(tmp[7])];
	}

	public string GetViewText () {
		char[] separator = { ':', ' ', ',' };
		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		string s = "";
		s += " [GraphType]\n\t\t\t\t\t" + System.Enum.GetNames (typeof(GraphType)) [int.Parse (tmp [0])] + "\n";
		s += " [Source]\n";
		s += "\tFish:\t\t" + (int.Parse(tmp [1]) + 1) + "\n";
		if (int.Parse(tmp[0]) != 1)
			s += "\tInput:\t\t" + DataBase.GetTags () [int.Parse (tmp [2])] + "\n";
		else
			s += "\tInput:\t\tStep\n";
		s += "\tOutput:\t" + DataBase.GetTags () [int.Parse (tmp [3])] + "\n";
		if (int.Parse(tmp[5]) > 0)
			s += "\tDirection\t" + DataBase.GetTags () [int.Parse (tmp [4])] + "\n";
		s += " [Pointer]\n";
		s += "\tType:\t" + tmp [5] + "\n";
		s += "\tNumber:\t" + tmp [6] + "\n";
		s += "\tColor:\t\t" + ProjectData.ColorList.names[int.Parse(tmp [7])] + "\n";
		s += "\tUse color grad:\n\t\t\t\t\t" + (int.Parse (tmp [8]) > 0 ? "Yes" : "No") + "\n";
		s += "\tUse size grad:\n\t\t\t\t\t" + (int.Parse (tmp [9]) > 0 ? "Yes" : "No") + "\n";
		s += "\tSize:\t\t" + tmp [10] + "\n";
		s += "\tUse auto size:\n\t\t\t\t\t" + (int.Parse (tmp [11]) > 0 ? "Yes" : "No") + "\n";

		return s;
	}

	public string GetParameterText() {
		return parameter;
	}

	public void OpenSettingWindow() {
		MyWindowManager mwm = this.GetComponentInParent<MyWindowManager> ();
		mwm.AddWindow ("MultiVariousParameterSetting");
		mwm.GetLastWindowController ().gameObject.GetComponentInChildren<MultiVariousParameterSettingContent> ().RegisterMultiVariousParameterNode (this);
	}
}
