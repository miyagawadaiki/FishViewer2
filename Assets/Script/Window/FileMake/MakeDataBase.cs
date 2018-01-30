using System.IO;
using System.Collections;
using System.Collections.Generic;

public class MakeDataBase {

	private static float[,,] data;
	private static List<string> tags;

	public MakeDataBase () {
		Init ();
	}

	public static void Init () {
		tags = new List<string> ();




	}

	public static void InitDataBase () {
		string fileName = ProjectData.FileName.GetName (ProjectData.FileKey.Read);
		StreamReader sr = new StreamReader (ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Read), System.Text.Encoding.GetEncoding("UTF-8"));

		int count = 0;
		for (int i = 0;; i++) {
			string s = sr.ReadLine ();
			if (s == null)
				break;
			count++;
		}

		for (int i = 0; i < count; i++) {

		}
	}

	public static void InitFormulas () {

	}

	public static void Add (string name) {
		tags.Add (name);
	}

	public static int Search (string name) {
		for (int i = 0; i < tags.Count; i++) {
			if (tags [i].Equals (name))
				return i;
		}

		return -1;
	}
}
