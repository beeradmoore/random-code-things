using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using System.Threading;

namespace NumberCruncher
{
	public class NumberCruncherViewController : UIViewController
	{
		UIButton startButton = null;
		UITextView textView = null;
		int totalNumbers = 0;
		public NumberCruncherViewController()
		{
			View.BackgroundColor = UIColor.White;

			startButton = UIButton.FromType(UIButtonType.RoundedRect);
			startButton.SetTitle("Start", UIControlState.Normal);
			startButton.SetTitle("Processing", UIControlState.Disabled);
			startButton.SetTitleColor(UIColor.LightGray, UIControlState.Disabled);
			startButton.TouchDown += DoProcessing;
			startButton.Frame = new RectangleF(10, 10, 300, 26);
			View.AddSubview(startButton);

			 
			textView = new UITextView(new RectangleF(10, startButton.Frame.Bottom + 10, 300, UIScreen.MainScreen.ApplicationFrame.Height - startButton.Frame.Bottom - 20));
			textView.Font = UIFont.FromName("Courier", 16f);
			textView.Editable = false;
			textView.TextAlignment = UITextAlignment.Left;
			View.AddSubview(textView);
		}

		private void ClearText()
		{
			InvokeOnMainThread(delegate {
				textView.Text = String.Empty;
			});
		}

		private void ConcatText(string text)
		{
			InvokeOnMainThread(delegate {
				textView.Text += text + "\n";
				textView.ScrollRangeToVisible(new NSRange(textView.Text.Length, 0));
			});
		}

		private void DoProcessing(object sender, EventArgs e)
		{
			startButton.Enabled = false;
			DateTime start = DateTime.Now;
			Random rand = new Random();

			++totalNumbers;

			ConcatText("Total numbers: " + totalNumbers);

			double [] selectedNumbers = new double[totalNumbers];
			for (int i = 0; i < selectedNumbers.Length; ++i)
			{
				selectedNumbers[i] = rand.Next(1, 100);
			}

			ConcatText("Numbers: " + String.Join(", ", selectedNumbers));


			NCNode ncNode = new NCNode(0, selectedNumbers, null);

			ncNode.Process();
				
			ConcatText("Total Nodes: " + ncNode.TotalNodes);
			ConcatText("Total Time: " + (DateTime.Now - start).TotalMilliseconds.ToString());
			ConcatText("\n\n");
			startButton.Enabled = true;
		}
	}
}

