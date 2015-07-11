using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace UITestIdGenerator.Mac
{
	public static class XmlExtensionsAndroid
	{
		static nint counter;

		public static void AddXtcIdsToAndroid (this string documentUrl)
		{
			
		}

		public static void AddXtcIdsToAndroid (this XElement layoutRoot)
		{
			// layoutRoot.AddXtcIdsToBarButtonItems();

			layoutRoot.AddXtcIdsToAndroidButtons();

			layoutRoot.AddXtcIdsToAndroidEditTexts();

			layoutRoot.AddXtcIdsToAndroidTextViews();

//			layoutRoot.AddXtcIdsToLabels();
		}


		#region AddXtcIdsToElement

		//		public static void AddXtcIdsToBarButtonItems (this XElement layoutRoot)
		//		{
		//			counter = 0;
		//
		//			foreach (var barButtonItem in layoutRoot.getBarButtonItems())
		//				barButtonItem.ensureAccessibilityLabel(barButtonItem.createXtcIdentifierForBarButtonItem());
		//		}


		public static void AddXtcIdsToAndroidButtons (this XElement layoutRoot)
		{
			counter = 0;

			foreach (var button in layoutRoot.getButtons())
				button.ensureContentDescription(button.createXtcIdentifierForButton());
		}


		public static void AddXtcIdsToAndroidEditTexts (this XElement layoutRoot)
		{
			counter = 0;

			foreach (var editText in layoutRoot.getEditTexts())
				editText.ensureContentDescription(editText.createXtcIdentifierForEditText());
		}


		public static void AddXtcIdsToAndroidTextViews (this XElement layoutRoot)
		{
			counter = 0;

			foreach (var textView in layoutRoot.getTextViews())
				textView.ensureContentDescription(textView.createXtcIdentifierForTextView());
		}


		//		public static void AddXtcIdsToLabels (this XElement layoutRoot)
		//		{
		//			counter = 0;
		//
		//			foreach (var label in layoutRoot.getLabels())
		//				label.ensureContentDescription(label.createXtcIdentifierForLabel());
		//		}

		#endregion


		#region getElement

		//		static IEnumerable<XElement> getBarButtonItems (this XElement root)
		//		{
		//			return root.Descendants("barButtonItem").Where(bbi => !bbi.isBarButtonItemSpace());
		//		}
		//
		//		static bool isBarButtonItemSpace (this XElement barButtonItem)
		//		{
		//			var systemItem = barButtonItem.Attribute("systemItem")?.Value;
		//			return systemItem == "flexibleSpace" || systemItem == "fixedSpace";
		//		}

		static IEnumerable<XElement> getButtons (this XElement root) => root.Descendants("Button");

		static IEnumerable<XElement> getEditTexts (this XElement root) => root.Descendants("EditText");

		static IEnumerable<XElement> getTextViews (this XElement root) => root.Descendants("TextView");

		// static IEnumerable<XElement> getLabels (this XElement root) => root.Descendants("Label");


		#endregion


		#region createXtcIdentifierForElement

		//		static string createXtcIdentifierForBarButtonItem (this XElement element)
		//		{
		//			var name = element.Attribute("userLabel")?.Value ?? element.Attribute("systemItem")?.Value ?? element.Attribute("title")?.Value;
		//
		//			if (string.IsNullOrWhiteSpace(name))
		//				name = element.HasDescendant("activityIndicatorView") ? "activity_indicator" : ++counter.ToString();
		//
		//			return string.Format("barbutton_{0}", name.ToCleanLower());
		//			// TODO: write something to the console here to tell them to add a userlabel
		//			// TODO: move the string cleaning elsewhere
		//		}


		static string createXtcIdentifierForButton (this XElement element)
		{
			var name = element.Attribute("userLabel")?.Value;

			if (string.IsNullOrWhiteSpace(name)) {

				var title = element.Element("state")?.Attribute("title")?.Value;

				if (!string.IsNullOrWhiteSpace(title)) {

					if (title.Length > 20) title = title.Substring(0, 20);

					name = title;
				} else {

					name = (++counter).ToString();
				}
			}

			return string.Format("button_{0}", name.ToCleanLower());
			// TODO: write something to the console here to tell them to add a userlabel
			// TODO: move the string cleaning elsewhere
		}


		static string createXtcIdentifierForEditText (this XElement element)
		{
			var name = element.Attribute("userLabel")?.Value;

			if (string.IsNullOrWhiteSpace(name)) {

				var title = element.Attribute("title")?.Value;

				if (!string.IsNullOrWhiteSpace(title)) {

					if (title.Length > 20) title = title.Substring(0, 20);

					name = title;
				} else {

					name = (++counter).ToString();
				}
			}

			return string.Format("textfield_{0}", name.ToCleanLower());
			// TODO: write something to the console here to tell them to add a userlabel
			// TODO: move the string cleaning elsewhere
		}


		static string createXtcIdentifierForTextView (this XElement element)
		{
			var name = element.Attribute("userLabel")?.Value;

			if (string.IsNullOrWhiteSpace(name)) {

				var stringElement = element.Element("string");

				var title = stringElement?.Attribute("key")?.Value == "text" ? stringElement.Value : null;

				if (!string.IsNullOrWhiteSpace(title)) {

					if (title.Length > 20) title = title.Substring(0, 20);

					name = title;
				} else {

					name = (++counter).ToString();
				}
			}

			return string.Format("textview_{0}", name.ToCleanLower());
			// TODO: write something to the console here to tell them to add a userlabel
			// TODO: move the string cleaning elsewhere
		}


		//		static string createXtcIdentifierForLabel (this XElement element)
		//		{
		//			var name = element.Attribute("userLabel")?.Value;
		//
		//			if (string.IsNullOrWhiteSpace(name)) {
		//
		//				var stringElement = element.Element("string");
		//
		//				var title = stringElement?.Attribute("key")?.Value == "text" ? stringElement.Value : null;
		//
		//				if (!string.IsNullOrWhiteSpace(title)) {
		//
		//					if (title.Length > 20) title = title.Substring(0, 20);
		//
		//					name = title;
		//				} else {
		//
		//					name = ++counter.ToString();
		//				}
		//			}
		//
		//			return string.Format("label_{0}", name.ToCleanLower());
		//			// TODO: write something to the console here to tell them to add a userlabel
		//			// TODO: move the string cleaning elsewhere
		//		}

		#endregion


		#region userDefinedRuntimeAttribute utils


		static void ensureContentDescription (this XElement element, string contentDescriptionValue)
		{
			var contentDescriptionAttribute = element.Attribute("android:contentDescription");
			if (contentDescriptionAttribute == null) {

				contentDescriptionAttribute = new XAttribute ("android:contentDescription", contentDescriptionValue);
				element.Add(contentDescriptionAttribute);

				Console.WriteLine("Added android:contentDescription to {0} : {1}", element.Name, contentDescriptionValue);
			}
		}
		//
		//
		//		static bool hasContentDescription (this XElement element)
		//		{
		//			return element.Attribute("android:contentDescription")?.FirstOrDefault(a => a.keyPathIsContentDescription()) != null;
		//		}
		//
		//
		//		static void addAndroidContentDescription (this XElement userDefinedRuntimeAttributes, string contentDescriptionValue)
		//		{
		//			userDefinedRuntimeAttributes.Add(createUserDefinedRuntimeAttributeNode("contentDescription", contentDescriptionValue));
		//		}
		//
		//
		//		static XElement getUserDefinedRuntimeAttributesNode (this XElement element)
		//		{
		//			// first check in the <userDefinedRuntimeAttributes> node exists
		//			var userDefinedRuntimeAttributes = element.Element("userDefinedRuntimeAttributes");
		//			if (userDefinedRuntimeAttributes == null) {
		//
		//				// if not create it and add it to the element
		//				userDefinedRuntimeAttributes = new XElement ("userDefinedRuntimeAttributes");
		//				element.Add(userDefinedRuntimeAttributes);
		//			}
		//
		//			return userDefinedRuntimeAttributes;
		//		}
		//
		//
		//		// used for UIView
		//		static bool keyPathIsContentDescription (this XElement element)
		//		{
		//			return element.Attribute("keyPath")?.Value == "contentDescription";
		//		}
		//
		//
		//		static XElement createUserDefinedRuntimeAttributeNode (string keyPath, string value)
		//		{
		//			var runtimeAttribute = new XElement ("userDefinedRuntimeAttribute");
		//
		//			runtimeAttribute.SetAttributeValue("type", "string");
		//			runtimeAttribute.SetAttributeValue("keyPath", keyPath);
		//			runtimeAttribute.SetAttributeValue("value", value);
		//
		//			return runtimeAttribute;
		//		}

		#endregion
	}
}