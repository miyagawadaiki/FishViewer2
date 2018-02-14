using System.Collections;
using System.Collections.Generic;

namespace ProjectData {
	public enum FileKey {
		Read,
		Input,
		Output,
		Formula,
		Image,
	}

	public class FileName {

		private static string[,] names = new string[2,System.Enum.GetNames(typeof(FileKey)).Length];
		private static char[] separator = { '/', '\\' };

		public static void Set(FileKey key, string path, string name) {
			names [0, (int)key] = path;
			names [1, (int)key] = name;
			UnityEngine.PlayerPrefs.SetString (System.Enum.GetName(typeof(FileKey), key), path + "/" + name);
		}

		public static string GetPath(FileKey key) {
			if (names [0, (int)key] == null) {
				string line = UnityEngine.PlayerPrefs.GetString (System.Enum.GetName (typeof(FileKey), key), "C:/Users/aaa.csv");
				string[] tmp = line.Split (separator);
				names [0, (int)key] = tmp [0];
				for (int i = 1; i < tmp.Length - 1; i++)
					names [0, (int)key] += "/" + tmp [i];
			}
			return names[0, (int)key];
		}

		public static string GetName(FileKey key) {
			if (names [1, (int)key] == null) {
				string line = UnityEngine.PlayerPrefs.GetString (System.Enum.GetName (typeof(FileKey), key), "C:/Users/aaa.csv");
				string[] tmp = line.Split (separator);
				names [1, (int)key] = tmp [tmp.Length - 1];
			}
			return names [1, (int)key];
		}

		public static string GetNameWithPath(FileKey key) {
			return GetPath (key) + "/" + GetName (key);
		}
	}

	public class ColorList {

		public static UnityEngine.Color[] colors = {
			new UnityEngine.Color(1f, 0f, 0f),						// red
			new UnityEngine.Color(1f, 96f / 255f, 0f),				// orange
			new UnityEngine.Color(0f, 163f / 255, 40f / 255f),		// green
			new UnityEngine.Color(0f, 32f / 255f, 1f),				// blue
			new UnityEngine.Color(149f / 255f, 0f, 183f / 255f),	// purple
			new UnityEngine.Color(1f, 0f, 146f / 255f),				// pink
			new UnityEngine.Color(1f, 0.9f, 0.08f),					// yellow
			new UnityEngine.Color(0f, 1f, 0f),						// yello-green
			new UnityEngine.Color(0f, 224f / 255f, 1f),				// light-blue
			new UnityEngine.Color(28f / 255f, 0f, 88f / 255f),		// deap-blue
		};

		public static string[] names = {
			"Red",
			"Orange",
			"Yellow",
			"Light Green",
			"Green",
			"Light Blue",
			"Blue",
			"Deep Blue",
			"Purple",
			"Pink",
		};

		public static int Count() {
			return colors.Length;
		}

		public static UnityEngine.Color GetColor(int i) {
			return colors [i % colors.Length];
		}

	}

	public class DefaultData {
		public static DataType[] dataTypes = {
			new DataType ("PositionX", ""), // 0
			new DataType ("PositionY", ""), // 1
			new DataType ("VelocityX", "!PositionX,0 !PositionX,-1 -   @dt /"), // 2
			new DataType ("VelocityY", "!PositionY,0 !PositionY,-1 -   @dt /"), // 3
			new DataType ("AccelerationX", "!VelocityX,0 !VelocityX,-1 -   @dt /"), // 4
			new DataType ("AccelerationY", "!VelocityY,0 !VelocityY,-1 -   @dt /"), // 5
			new DataType ("Distance", "!PositionX,0 !PositionY,0 Mag"), // 6
			new DataType ("Speed", "!VelocityX,0 !VelocityY,0 Mag"), // 7
			new DataType ("Accelaration", "!Speed,0 !Speed,-1 -  @dt /"), // 8
			new DataType ("AbsAcceleration", "!AccelerationX,0 !AccelerationY,0 Mag"), // 9
			new DataType ("PositionAngle", "1 0 !PositionX,0 !PositionY,0 CalcAngle"), // 10
			new DataType ("AugumentAngle", "!VelocityX,-1 !VelocityY,-1 !VelocityX,0 !VelocityY,0 CalcAngle"), // 11
			new DataType ("AbsAugAngle", "!AugumentAngle,0 Abs"),	// 12
		};

		public static string[] defaultGraphNames = {
			"PolarPos",
			"Speed",
			"V-A",
			//"V-Angle",
		};

		public static string[] defaultGraphTexts = {
			System.Enum.GetName (typeof(GraphContentType), GraphContentType.MultiEven) + "Graph/2 :a:0,6,10,0,:0,50,0,1,1,10,0,:/400,400",
			System.Enum.GetName (typeof(GraphContentType), GraphContentType.MultiEven) + "Graph/1 :a:0,0,7,0,:0,200,0,0,0,10,1,:/400,250",
			System.Enum.GetName (typeof(GraphContentType), GraphContentType.MultiEven) + "Graph/0 :a:0,7,8,0,:0,50,0,1,1,10,0,:/350,250",
			//System.Enum.GetName (typeof(GraphContentType), GraphContentType.Single) + "Graph/0 :0,7,11,:0,50,0,1,1,10,0,:",
		};

		public static List<string> shortcutTexts = new List<string> ();

		public static int Search (string s) {
			for (int i = 0; i < dataTypes.Length; i++) {
				if (dataTypes [i].Equals (s))
					return i;
			}

			return -1;
		}
	}
}