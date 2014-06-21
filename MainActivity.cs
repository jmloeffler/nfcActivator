using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Nfc;

namespace nfcActivator
{
	[Activity (Label = "nfcActivator", MainLauncher = true)]
	public class MainActivity : Activity
	{
		//int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			var adapter = Android.Nfc.NfcAdapter.DefaultAdapter;

			//if we're resuming because an ndef record was discovered, get the ndef payloads
			//and write them to the display
			if (Android.Nfc.NfcAdapter.ActionNdefDiscovered.Equals(Intent.Action)) {
				var rawMsgs = Intent.GetParcelableArrayExtra(Android.Nfc.NfcAdapter.ExtraNdefMessages);
				if (rawMsgs != null) {
					var msgs = new Android.Nfc.NdefMessage[rawMsgs.Length];
					for (int i = 0; i < rawMsgs.Length; i++) {
						msgs[i] = (Android.Nfc.NdefMessage) rawMsgs[i];
					}

					var textView = (TextView)this.FindViewById (Resource.Id.txtNfcData);
					textView.Text = string.Empty;

					foreach (var msg in msgs) {
						foreach (var record in msg.GetRecords()) {
							var bytes = record.GetPayload ();
							textView.Text += System.Text.Encoding.ASCII.GetString (bytes);
						}
					}
				}
			}


		}

	}
}


