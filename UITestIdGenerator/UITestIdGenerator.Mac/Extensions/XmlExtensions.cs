using System.Xml.Linq;
using System.Linq;

namespace UITestIdGenerator.Mac
{
	public static class XmlExtensions
	{
		public static bool HasDescendant (this XElement element, string name)
		{
			return element.Descendants(name)?.Any() ?? false;
		}
	}
}