using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;


namespace TestColor
{
	public class MyDialogViewController : DialogViewController
	{
		public MyDialogViewController() : base(new RootElement(""))
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			///View.BackgroundColor = UIColor.Brown;
			//TableView.BackgroundColor = UIColor.Brown;

			Root.Add(new Section("Color Test"){new StringElement("Color for BigBob85 Test")});
			
			//this.TableView.BackgroundColor = UIColor.Clear;
			//this.TableView.SeparatorColor = UIColor.Clear;
			this.TableView.BackgroundView = null;


			//this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			this.View.BackgroundColor = UIColor.Brown;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			//View.BackgroundColor = UIColor.Brown;
			//TableView.BackgroundColor = UIColor.Brown;


		}
	}
}

