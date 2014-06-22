using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Nfc;
using System.Text;

namespace nfcActivator
{
	[Activity (Label = "nfcActivator", MainLauncher = true)]
	public class MainActivity : Activity
	{
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

					var textView = (TextView)this.FindViewById (Resource.Id.txtNfcData);
					textView.Text = GetNdefMessageContents (rawMsgs);

					SetRingerState (Android.Media.RingerMode.Vibrate);
				}
			}
		}

		void SetRingerState (Android.Media.RingerMode newState)
		{
			var audioService = (Android.Media.AudioManager)this.GetSystemService (Context.AudioService);
			audioService.RingerMode = newState;
		}

		static string GetNdefMessageContents (IParcelable[] msgs)
		{
			var displayText = new StringBuilder ();
			foreach (NdefMessage msg in msgs) {
				var records = msg.GetRecords ();
				for (int i = 0; i < records.Length; i++) {
					var bytes = records [i].GetPayload ();
					displayText.AppendFormat (" + Record {0}: {1}", i.ToString (), System.Text.Encoding.ASCII.GetString (bytes));
				}
			}

			return displayText.ToString ();
		}
	}
}


