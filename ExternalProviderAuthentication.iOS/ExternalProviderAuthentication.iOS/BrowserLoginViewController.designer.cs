// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace ExternalProviderAuthentication.iOS
{
	[Register ("BrowserLoginViewController")]
	partial class BrowserLoginViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIWebView _webBrowser { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_webBrowser != null) {
				_webBrowser.Dispose ();
				_webBrowser = null;
			}
		}
	}
}
