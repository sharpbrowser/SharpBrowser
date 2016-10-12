using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;

namespace SharpBrowser {
	internal static class Utils {

		public static bool IsFocussed(TextBox tb) {
			return tb.Focused;
		}

		public static void AddHotKey(Form form, Action function, Keys key, bool ctrl = false, bool shift = false, bool alt = false) {
			form.KeyPreview = true;
			form.KeyDown += delegate(object sender, KeyEventArgs e) {
				if (e.IsHotkey(key, ctrl, shift, alt)) {
					function();
				}
			};
		}

		public static bool IsFullySelected(TextBox tb) {
			return tb.SelectionLength == tb.TextLength;
		}
		public static bool HasSelection(TextBox tb) {
			return tb.SelectionLength > 0;
		}


		public static bool CheckIfFilePath(this string path) {

			if (path[1] == ':') {
				if (path[2] == '\\') {
					if (Char.IsLetter(path[0])) {
						return true;
					}
				}
			}
			return false;
		}

		public static bool CheckIfFilePath2(this string path) {

			if (path[1] == ':') {
				if (path[2] == '/') {
					if (Char.IsLetter(path[0])) {
						return true;
					}
				}
			}
			return false;
		}

		public static string GetAfter(this string text, string find, int startAt = 0, bool returnAll = false, bool forward = true) {
			if (text == null) { return returnAll ? text : ""; }
			int idx;
			if (!forward) {
				idx = text.LastIndexOf(find, text.Length - startAt, StringComparison.Ordinal);
			} else {
				idx = text.IndexOf(find, startAt, StringComparison.Ordinal);
			}
			if (idx == -1) { return returnAll ? text : ""; }
			idx += find.Length;
			return text.Substring(idx);
		}

		public static string GetAfterLast(this string text, string find, bool returnAll = false) {
			int idx = text.LastIndexOf(find, StringComparison.Ordinal);
			if (idx == -1) {
				return returnAll ? text : "";
			}
			idx += find.Length;
			return text.Substring(idx);
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

		public static bool FileNotExists(this string path) {
			return !File.Exists(path);
		}


		public static string ConvertToString(this object o) {
			if (o is string) {
				return o as string;
			}
			return null;
		}
		public static bool CheckIfValid(this string text, bool trimAndCheck = false) {
			return text != null && text.Length > 0;
		}

		public static void InvokeOnParent(this Control control, MethodInvoker method) {
			Control parent = control.Parent == null ? control : control.Parent;
			if (parent.IsHandleCreated) {
				parent.Invoke(method);
				return;
			}
		}

		public static bool IsHotkey(this KeyEventArgs eventData, Keys key, bool ctrl = false, bool shift = false, bool alt = false) {
			return eventData.KeyCode == key && eventData.Control == ctrl && eventData.Shift == shift && eventData.Alt == alt;
		}

		public static CefState ToCefState(this bool value) {
			return value ? CefState.Enabled : CefState.Disabled;
		}
		public static bool IsBitmaskOn(this int num, int bitmask) {
			return (num & bitmask) != 0;
		}

		public static bool BeginsWith(this string str, string beginsWith, bool caseSensitive = true) {
			if (beginsWith.Length > str.Length) {
				return false;
			}
			if (beginsWith.Length == str.Length) {
				return String.Equals(beginsWith, str, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
			}
			return str.LastIndexOf(beginsWith, beginsWith.Length - 1, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) == 0;
		}
		public static string RemovePrefix(this string str, string prefix, bool caseSensitive = true) {
			if (str.Length >= prefix.Length && str.BeginsWith(prefix, caseSensitive)) {
				return str.Substring(prefix.Length);
			}
			return str;
		}


	}
}
