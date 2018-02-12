using System.IO;
using System.Collections;
using System.Collections.Generic;

public class DataBase {

	public static string fileName;
	private static string[] tags;
	private static string[] shortTags;
	private static float[,,] data;
	private static float[] max;
	private static float[] min;
	public static int step, fish, tag;
	public static float dt;
	private static char[] separator = { ',' };
	public static List<string> shortcutList;

	public static SimulationManager simuMan;

	private DataBase() {
		
	}

	public static void SetDataBase() {
		fileName = ProjectData.FileName.GetName (ProjectData.FileKey.Read);
		StreamReader sr = new StreamReader (ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Read), System.Text.Encoding.GetEncoding("UTF-8"));

		string[] tmp = sr.ReadLine ().Split (separator);
		step = int.Parse(tmp[0]);
		fish = int.Parse (tmp [1]);
		dt = float.Parse (tmp [2]);

		tmp = sr.ReadLine ().Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		tag = tmp.Length / fish;
		tags = new string[tag];
		shortTags = new string[tag];
		data = new float[step, fish, tag];
		max = new float[tag];
		min = new float[tag];

		for (int i = 0; i < tag; i++) {
			tags [i] = tmp [i];
			shortTags [i] = GetShortString (tags [i]);
			max [i] = -10000f;
			min [i] = 10000f;

			if (tags [i].IndexOf ("Norm") > 0) {
				max [i] = 1f;
				min [i] = 0f;
			}

			if (tags [i].IndexOf ("Angle") > 0) {
				max [i] = UnityEngine.Mathf.PI;
				min [i] = -UnityEngine.Mathf.PI;
			}
		}

		for (int i = 0; i < step; i++) {
			SetData (i, sr.ReadLine ());
		}

		for (int i = 0; i < tag; i++) {
			if (min [i] > 0f)
				min [i] = 0f;
			if (max [i] < 0f)
				max [i] = 0f;
		}

		shortcutList = new List<string> ();
		string shortcut;
		while ((shortcut = sr.ReadLine ()) != null) {
			shortcutList.Add (shortcut);
		}

		simuMan.Init ();
	}

	private static void SetData(int step, int fish, int tag, float value) {
		data [step, fish, tag] = value;
	}

	private static void SetData(int step, string line) {
		string[] tmp = line.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < fish; i++) {
			for (int j = 0; j < tag; j++) {
				float value = float.Parse (tmp [i * tag + j]);
				if (float.IsNaN (value))
					value = 0f;
				SetData (step, i, j, value);
				if (max [j] < value)
					max [j] = value;
				if (min [j] > value)
					min [j] = value;
			}
		}
	}

	public static float GetData(int step, int fish, int tag) {
		return data [step, fish, tag];
	}

	public static string[] GetTags() {
		return tags;
	}

	public static string[] GetShortTags() {
		return shortTags;
	}

	public static float GetMax(int tag) {
		return max [tag];
	}

	public static float GetMin(int tag) {
		return min [tag];
	}

	public static string GetShortString (string text) {
		string ret = "";

		int cnt = 0;
		for (int i = 0; i < text.Length; i++) {
			if (char.IsUpper (text, i)) {
				ret += text.Substring (i, 1);
				cnt = 2;
			} else if (cnt > 0) {
				ret += text.Substring (i, 1);
				cnt--;
			}
		}

		if (ret.Length > 7)
			ret = ret.Substring (0, 7);

		return ret;
	}
}
