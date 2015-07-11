using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;

namespace UITestIdGenerator.Mac
{
	public static class XmlExtensionsiOS
	{

		// TODO: Add searchBar
		// TODO: Clean up

		static nint counter;

		static nint sceneCounter;

		static StringBuilder stringBuilder;

		public static void AddXtcIdsToiOS (this string docUri)
		{
			var nib = XDocument.Load(docUri);

			stringBuilder = new StringBuilder ();

			stringBuilder.AppendLine("namespace Xamarin.UITest.Queries {\n\n\tpublic static class UIElements {");

			var scenes = nib.Root.getScenes();
			if (scenes?.Any() ?? false) {

				scenes.AddXtcIdsToStoryboardScenes();

			} else {

				nib.Root.AddXtcIdsToNib(docUri.DocumentName());
			}

			stringBuilder.AppendLine("\t}\n}");

			nib.Save(docUri);

			File.WriteAllText(docUri.FilePathForSibling("UIElements.cs"), stringBuilder.ToString());
		}


		#region AddXtcIdsToElement

		public static void AddXtcIdsToNib (this XElement nibRoot, string outputClassName)
		{
			stringBuilder.AppendLine(string.Format("\n\n\t\t#region {0}\n\n\t\tpublic static class {0} {{", outputClassName));

			nibRoot.AddXtcIdsToUIBarButtonItems();

			nibRoot.AddXtcIdsToUIButtons();

			nibRoot.AddXtcIdsToUITextFields();

			nibRoot.AddXtcIdsToUITextViews();

			nibRoot.AddXtcIdsToUILabels();

			stringBuilder.AppendLine("\t\t}\n\n\t\t#endregion");
		}

		public static void AddXtcIdsToStoryboardScenes (this IEnumerable<XElement> scenes)
		{
			sceneCounter = 0;

			foreach (var scene in scenes) {
				scene.AddXtcIdsToNib(scene.createClassNameForScene() ?? "OrphanedElements");
			}
		}


		public static void AddXtcIdsToUIBarButtonItems (this XElement root)
		{
			counter = 0;

			foreach (var barButtonItem in root.getBarButtonItems())
				barButtonItem.ensureAccessibilityLabel(barButtonItem.createXtcIdentifierForBarButtonItem());
		}


		public static void AddXtcIdsToUIButtons (this XElement root)
		{
			counter = 0;

			foreach (var button in root.getButtons())
				button.ensureAccessibilityIdentifier(button.createXtcIdentifierForButton());
		}


		public static void AddXtcIdsToUITextFields (this XElement root)
		{
			counter = 0;

			foreach (var textField in root.getTextFields())
				textField.ensureAccessibilityIdentifier(textField.createXtcIdentifierForTextField());
		}


		public static void AddXtcIdsToUITextViews (this XElement root)
		{
			counter = 0;

			foreach (var textView in root.getTextViews())
				textView.ensureAccessibilityIdentifier(textView.createXtcIdentifierForTextView());
		}


		public static void AddXtcIdsToUILabels (this XElement root)
		{
			counter = 0;

			foreach (var label in root.getLabels())
				label.ensureAccessibilityIdentifier(label.createXtcIdentifierForLabel());
		}

		#endregion


		#region getElement

		static IEnumerable<XElement> getBarButtonItems (this XElement root)
		{
			return root.Descendants("barButtonItem").Where(bbi => !bbi.isBarButtonItemSpace());
		}

		static bool isBarButtonItemSpace (this XElement barButtonItem)
		{
			var systemItem = barButtonItem.Attribute("systemItem")?.Value;
			return systemItem == "flexibleSpace" || systemItem == "fixedSpace";
		}

		static IEnumerable<XElement> getButtons (this XElement root) => root.Descendants("button");

		static IEnumerable<XElement> getTextFields (this XElement root) => root.Descendants("textField");

		static IEnumerable<XElement> getTextViews (this XElement root) => root.Descendants("textView");

		static IEnumerable<XElement> getLabels (this XElement root) => root.Descendants("label");

		static IEnumerable<XElement> getScenes (this XElement root) => root.Descendants("scene");

		#endregion


		#region createXtcIdentifierForElement

		static string createXtcIdentifierForBarButtonItem (this XElement element)
		{
			var name = element.Attribute("userLabel")?.Value ?? element.Attribute("systemItem")?.Value ?? element.Attribute("title")?.Value;

			if (string.IsNullOrWhiteSpace(name))
				name = element.HasDescendant("activityIndicatorView") ? "activity_indicator" : (++counter).ToString();

			return string.Format("barbutton_{0}", name.ToCleanLower());
			// TODO: write something to the console here to notifiy the user about lack of info
		}


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
			// TODO: write something to the console here to notifiy the user about lack of info
		}


		static string createXtcIdentifierForTextField (this XElement element)
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
			// TODO: write something to the console here to notifiy the user about lack of info
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
			// TODO: write something to the console here to notifiy the user about lack of info
		}


		static string createXtcIdentifierForLabel (this XElement element)
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

			return string.Format("label_{0}", name.ToCleanLower());
			// TODO: write something to the console here to tell them to add a userlabel
		}

		#endregion


		static string createClassNameForScene (this XElement scene)
		{
			var controllerTypes = new [] {
				"viewController",
				"navigationController",
				"tableViewController",
				"collectionViewController",
				"pageViewController"
			};

			var i = 0;

			var controllers = scene.Descendants(controllerTypes[i++]);

			while (!controllers.Any() && i < controllerTypes.Length) {
				controllers = scene.Descendants(controllerTypes[i++]);
			}

			var controller = controllers.FirstOrDefault();

			if (controller == null)
				return null;

			return controller.Attribute("customClass")?.Value ?? controller.Attribute("storyboardIdentifier")?.Value ??$"{controller.Name}{++sceneCounter}";
		}


		#region userDefinedRuntimeAttribute utils

		static void ensureAccessibilityLabel (this XElement element, string accessibilityLabelValue)
		{
			// first check in the <userDefinedRuntimeAttributes> node exists
			// if not create it and add it to the element
			var userDefinedRuntimeAttributes = element.getUserDefinedRuntimeAttributesNode();

			// next chech if any of the <userDefinedRuntimeAttribute> children has
			// a keyPath of "accessibilityLabel" if not, create a new one and add it
			if (!userDefinedRuntimeAttributes.hasAccessibilityLabel()) {
				userDefinedRuntimeAttributes.addUserDefinedRuntimeAttributeAccessibilityLabel(accessibilityLabelValue);
				stringBuilder.AppendLine(string.Format("\t\t\tpublic static string {0} = \"{0}\";", accessibilityLabelValue));
			}
		}


		static void ensureAccessibilityIdentifier (this XElement element, string accessibilityIdentifierValue)
		{
			// first check in the <userDefinedRuntimeAttributes> node exists
			// if not create it and add it to the element
			var userDefinedRuntimeAttributes = element.getUserDefinedRuntimeAttributesNode();

			// next chech if any of the <userDefinedRuntimeAttribute> children has
			// a keyPath of "accessibilityIdentifier" if not, create a new one and add it
			if (!userDefinedRuntimeAttributes.hasAccessibilityIdentifier()) {
				userDefinedRuntimeAttributes.addUserDefinedRuntimeAttributeAccessibilityIdentifier(accessibilityIdentifierValue);
				stringBuilder.AppendLine(string.Format("\t\t\tpublic static string {0} = \"{0}\";", accessibilityIdentifierValue));
			}
		}


		static bool hasAccessibilityLabel (this XElement userDefinedRuntimeAttributes)
		{
			return userDefinedRuntimeAttributes.Elements("userDefinedRuntimeAttribute")?.FirstOrDefault(a => a.keyPathIsAccessibilityLabel()) != null;
		}

		static bool hasAccessibilityIdentifier (this XElement userDefinedRuntimeAttributes)
		{
			return userDefinedRuntimeAttributes.Elements("userDefinedRuntimeAttribute")?.FirstOrDefault(a => a.keyPathIsAccessibilityIdentifier()) != null;
		}


		static void addUserDefinedRuntimeAttributeAccessibilityLabel (this XElement userDefinedRuntimeAttributes, string accessibilityLabelValue)
		{
			userDefinedRuntimeAttributes.Add(createUserDefinedRuntimeAttributeNode("accessibilityLabel", accessibilityLabelValue));
		}

		static void addUserDefinedRuntimeAttributeAccessibilityIdentifier (this XElement userDefinedRuntimeAttributes, string accessibilityIdentifierValue)
		{
			userDefinedRuntimeAttributes.Add(createUserDefinedRuntimeAttributeNode("accessibilityIdentifier", accessibilityIdentifierValue));
		}


		static XElement getUserDefinedRuntimeAttributesNode (this XElement element)
		{
			// first check in the <userDefinedRuntimeAttributes> node exists
			var userDefinedRuntimeAttributes = element.Element("userDefinedRuntimeAttributes");
			if (userDefinedRuntimeAttributes == null) {

				// if not create it and add it to the element
				userDefinedRuntimeAttributes = new XElement ("userDefinedRuntimeAttributes");
				element.Add(userDefinedRuntimeAttributes);
			}

			return userDefinedRuntimeAttributes;
		}


		// used for UIBarButtonItems
		static bool keyPathIsAccessibilityLabel (this XElement element)
		{
			return element.Attribute("keyPath")?.Value == "accessibilityLabel";
		}

		// used for UIView + subclasses
		static bool keyPathIsAccessibilityIdentifier (this XElement element)
		{
			return element.Attribute("keyPath")?.Value == "accessibilityIdentifier";
		}


		static XElement createUserDefinedRuntimeAttributeNode (string keyPath, string value)
		{
			var runtimeAttribute = new XElement ("userDefinedRuntimeAttribute");

			runtimeAttribute.SetAttributeValue("type", "string");
			runtimeAttribute.SetAttributeValue("keyPath", keyPath);
			runtimeAttribute.SetAttributeValue("value", value);

			return runtimeAttribute;
		}

		#endregion
	}
}