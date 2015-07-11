using System;
using System.Text.RegularExpressions;
using System.IO;
using Foundation;

namespace UITestIdGenerator.Mac
{
	public static class StringExtensions
	{
		public static string ToCleanLower (this string dirtyString)
		{
			var cleanString = Regex.Replace(dirtyString, "[^a-zA-Z0-9_]+", "_");
			return Regex.Replace(cleanString, "__*", "_").Trim('_').ToLower();
		}

		public static bool HasNibExtension (this string path)
		{
			var ext = Path.GetExtension(path).ToLower();
			return ext == ".storyboard" || ext == ".xib";
		}

		public static bool HasAndroidExtension (this string path)
		{
			var ext = Path.GetExtension(path).ToLower();
			return ext == ".axml";
		}

		public static string DocumentName (this string docUri)
		{
			return Path.GetFileNameWithoutExtension(docUri);
		}

		public static string FilePathForSibling (this string docUri, string sibling)
		{
			return docUri.Replace(Path.GetFileName(docUri), sibling);
		}

		public static NSUrl ToFileUrl (this IntPtr handle)
		{
			return NSUrl.FromFilename(NSString.FromHandle(handle));
		}
	}
}