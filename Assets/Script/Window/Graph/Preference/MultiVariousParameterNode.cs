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

	private MultiParameterSettingGroup mpsg;
	private string parameter;

	void Awake () {
		mpsg = this.GetComponentInParent<MultiParameterSettingGroup> ();
		int i = (this.transform.parent.GetComponentsInChildren<MultiVariousParameterNode> ().Length - 1) % ProjectData.ColorList.colors.Length;
		parameter = "0 :0,0,1,:50," + i + ",1,0,10,0,:";

		buttonImage = this.GetComponent<Image> ();
		image = this.GetComponentsInChildren<Image> ()[1];
		button = this.GetComponent<Button> ();
	}

	// Use this for initialization
	void Start () {
		button.onClick.AddListener (() => mpsg.Select (this));
		this.GetComponentInChildren<Text> ().text = "Graph " + number;
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
		//UpdateButtonText ();
		UpdateNodeColor ();
		mpsg.Select (this);
	}

	/*
	public void UpdateButtonText () {
		char[] separator = { ' ', ',' };
		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		string s = "";
		s += "GraphType:" + System.Enum.GetNames (typeof(GraphType)) [int.Parse (tmp [0])] + ",";
		s += "fish:" + (int.Parse(tmp [1]) + 1) + ",";
		s += "xType:" + DataBase.GetTags () [int.Parse (tmp [2])] + ",";
		s += "yType:" + DataBase.GetTags () [int.Parse (tmp [3])] + ",";
		s += "pointNum:" + tmp [4] + ",";
		s += "colorNum:" + tmp [5] + ",";
		s += "useColorGrad:" + (int.Parse (tmp [6]) > 0 ? true : false) + ",";
		s += "useSizeGrad:" + (int.Parse (tmp [7]) > 0 ? true : false) + ",";
		s += "plotSize:" + tmp [8] + ",";
		s += "useAutoSize:" + (int.Parse (tmp [9]) > 0 ? true : false) + ",";

		buttonText.text = s;
	}
	*/

	public void UpdateNodeColor () {
		char[] separator = { ':', ' ', ',' };
		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		image.color = ProjectData.ColorList.colors[int.Parse(tmp[5])];
	}

	public string GetViewText () {
		char[] separator = { ':', ' ', ',' };
		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		string s = "";
		s += "GraphType:" + System.Enum.GetNames (typeof(GraphType)) [int.Parse (tmp [0])] + "\n";
		s += "fish:" + (int.Parse(tmp [1]) + 1) + "\n";
		s += "xType:" + DataBase.GetTags () [int.Parse (tmp [2])] + "\n";
		s += "yType:" + DataBase.GetTags () [int.Parse (tmp [3])] + "\n";
		s += "pointNum:" + tmp [4] + "\n";
		s += "colorNum:" + tmp [5] + "\n";
		s += "useColorGrad:" + (int.Parse (tmp [6]) > 0 ? true : false) + "\n";
		s += "useSizeGrad:" + (int.Parse (tmp [7]) > 0 ? true : false) + "\n";
		s += "plotSize:" + tmp [8] + "\n";
		s += "useAutoSize:" + (int.Parse (tmp [9]) > 0 ? true : false) + "\n";

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
