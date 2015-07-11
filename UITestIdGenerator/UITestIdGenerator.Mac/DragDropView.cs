using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using System.IO;

namespace UITestIdGenerator.Mac
{
	public partial class DragDropView : NSView
	{
		#region Constructors

		// Called when created from unmanaged code
		public DragDropView (IntPtr handle) : base(handle)
		{
			Initialize();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public DragDropView (NSCoder coder) : base(coder)
		{
			Initialize();
		}

		// Shared initialization code
		void Initialize ()
		{
			RegisterForDraggedTypes(new []{ "NSFilenamesPboardType" });
		}

		#endregion

		public override NSDragOperation DraggingEntered (NSDraggingInfo sender)
		{
			return DraggingUpdated(sender);
		}


		public override NSDragOperation DraggingUpdated (NSDraggingInfo sender)
		{
			NSObject obj = sender.DraggingSource;

			if (obj != null && obj.Equals(this)) {
				return NSDragOperation.Move;
			}

			return NSDragOperation.Copy;
		}


		public override bool PerformDragOperation (NSDraggingInfo sender)
		{
			NSPasteboard pb = sender.DraggingPasteboard;
			NSArray data = null;

			if (pb.Types.Contains(NSPasteboard.NSFilenamesType)) {
				data = pb.GetPropertyListForType(NSPasteboard.NSFilenamesType) as NSArray;
			}

			if (data != null) {

				for (nuint i = 0; i < data.Count; i++) {

					var url = data.ValueAt(i).ToFileUrl();

					if (url.IsFileUrl && File.Exists(url.Path)) {

						if (url.Path.HasNibExtension()) {

							url.Path.AddXtcIdsToiOS();

						} else if (url.Path.HasAndroidExtension()) {

							url.Path.AddXtcIdsToAndroid();

						} else {

							throw new NotSupportedException ("File extension must be .storyboard, .xib, or .axml");
						}
					}
				}
			}

			return true;
		}
	}
}
