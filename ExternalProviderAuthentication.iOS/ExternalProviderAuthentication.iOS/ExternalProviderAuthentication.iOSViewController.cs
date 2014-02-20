using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace ExternalProviderAuthentication.iOS
{
	public partial class ExternalProviderAuthentication_iOSViewController : UIViewController
	{
		private readonly AuthenticationServices _services = new AuthenticationServices("http://externalproviderdemo.azurewebsites.net/");
		private ExternalLoginViewModel _selectedProvider = null;

		public ExternalProviderAuthentication_iOSViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			List<ExternalLoginViewModel> externalLoginProviders = new List<ExternalLoginViewModel>(await _services.GetExternalLoginProviders ());
			InvokeOnMainThread (() => {
				float y = 50;
				int index = 0;
				foreach(ExternalLoginViewModel model in externalLoginProviders)
				{
					int localIndex = index;
					UIButton button = UIButton.FromType(UIButtonType.RoundedRect);
					button.SetTitle(model.Name, UIControlState.Normal);
					button.Tag = index;
					RectangleF frame = button.Frame;
					frame.Y = y;
					frame.Width = View.Frame.Width;
					frame.Height = 27;
					button.Frame = frame;
					View.AddSubview(button);
					button.TouchUpInside += (object sender, EventArgs e) => {
						_selectedProvider = externalLoginProviders [localIndex];
						PerformSegue ("presentBrowser", this);
					};

					index++;
					y += frame.Height + 7;
				}
			});
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
			if (segue.Identifier == "presentBrowser") {
				BrowserLoginViewController controller = (BrowserLoginViewController)segue.DestinationViewController;
				controller.ExternalLoginProvider = _selectedProvider;
				controller.Services = _services;
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

