using System;
using System.IO;

namespace SharpBrowser.Utils {
	internal static class FilePathUtils {

		public static bool CheckIfFilePath(this string path) {

			if (path.Length >= 3) {
				if (path[1] == ':') {
					if (path[2] == '\\') {
						if (Char.IsLetter(path[0])) {
							return true;
						}
					}
				}
			}
			return false;
		}

		public static bool CheckIfFilePath2(this string path) {

			if (path.Length >= 3) {
				if (path[1] == ':') {
					if (path[2] == '/') {
						if (Char.IsLetter(path[0])) {
							return true;
						}
					}
				}
			}
			return false;
		}

		public static bool SupportedInFilePath(this char c) {
			return !(c == '?' || c == '\'' || c == '\"' || c == '/' || c == '\\' || c == ';' || c == ':' || c == '&' || c == '*' || c == '<' || c == '>' || c == '|' || c == '{' || c == '}' || c == '[' || c == ']' || c == '(' || c == ')');
		}

		public static string ChangePathSlash(this string filePath, string slash) {
			if (slash == "\\") {
				if (filePath.Contains('/')) {
					return filePath.Replace("/", "\\");
				}
			}
			if (slash == "/") {
				if (filePath.Contains('\\')) {
					return filePath.Replace("\\", "/");
				}
			}
			return filePath;
		}
		public static string FileURLToPath(this string url) {
			return url.RemovePrefix("file:///").ChangePathSlash(@"\").DecodeURLForFilepath();
		}

		public static bool EnsureFolderExists(this string path) {
			if (!Directory.Exists(path)) {
				try {
					Directory.CreateDirectory(path);
					return true;
				}
				catch (Exception ex) {
				}
			}
			return false;
		}
	}
}
