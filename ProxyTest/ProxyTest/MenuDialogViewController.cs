using System;
using MonoTouch.Dialog;
using MonoTouch.CoreFoundation;
using MonoTouch.Foundation;
using System.Text;
using System.IO;
using System.Net;
using MonoTouch.UIKit;
using MonoTouch.Security;

namespace ProxyTest
{
	public class MenuDialogViewController : DialogViewController
	{
		private static string url = @"http://spamwars.com/dl/spamwars_subjects_log.txt";
		
		
		
		public MenuDialogViewController() : base(new RootElement(""))
		{
			CFProxySettings proxy0 = CFNetwork.GetSystemProxySettings();
			IWebProxy proxy1 = WebRequest.GetSystemWebProxy();
			IWebProxy proxy2 = HttpWebRequest.DefaultWebProxy;
			IWebProxy proxy3 = HttpWebRequest.GetSystemWebProxy();
			IWebProxy proxy4 = CFNetwork.GetDefaultProxy();
			
			
			RootElement root = new RootElement("");
			root.UnevenRows = true;
			
			Section section = new Section();
			
			CFProxySettings proxySettings = CFNetwork.GetSystemProxySettings();
			
			section.Add(new MultilineElement("HTTPEnable", proxySettings.HTTPEnable.ToString()));
			section.Add(new MultilineElement("HTTPPort", proxySettings.HTTPPort.ToString()));
			section.Add(new MultilineElement("HTTPProxy", (proxySettings.HTTPProxy == null) ? "null" : proxySettings.HTTPProxy.ToString()));
			section.Add(new MultilineElement("ProxyAutoConfigEnable", proxySettings.ProxyAutoConfigEnable.ToString()));
			section.Add(new MultilineElement("ProxyAutoConfigJavaScript", (proxySettings.ProxyAutoConfigJavaScript == null) ? "null" : proxySettings.ProxyAutoConfigJavaScript.ToString()));
			section.Add(new MultilineElement("ProxyAutoConfigURLString", (proxySettings.ProxyAutoConfigURLString == null) ? "null" : proxySettings.ProxyAutoConfigURLString.ToString()));
			root.Add(section);
			
			foreach (NSString key in proxySettings.Dictionary.Keys)
			{
				section = new Section(key.ToString());
				section.Add(new MultilineElement(proxySettings.Dictionary[key].ToString()));
				root.Add(section);
			}		
			
			StyledStringElement sse;
			
			section = new Section();
			sse = new StyledStringElement("HttpWebRequest");
			sse.Tapped += delegate() {
				Do_HttpWebRequest();
			};
			section.Add(sse);
			root.Add(section);
			
			
			
			section = new Section();
			sse = new StyledStringElement("HttpWebRequest DefaultWebProxy()");
			sse.Tapped += delegate() {
				Do_HttpWebRequest_DefaultWebProxy();				
			};
			section.Add(sse);
			root.Add(section);
			
			
			
			section = new Section();
			sse = new StyledStringElement("WebClient");
			sse.Tapped += delegate() {
				Do_WebClient();
			};
			section.Add(sse);
			root.Add(section);
			
			
			section = new Section();
			sse = new StyledStringElement("NSData.FromUrl");
			sse.Tapped += delegate() {
				Do_NSData_FromUrl();				
			};
			section.Add(sse);
			root.Add(section);
			
			
			section = new Section();
			sse = new StyledStringElement("NSUrlConnection");
			sse.Tapped += delegate() {
				Do_NSUrlConnection();				
			};
			section.Add(sse);
			root.Add(section);
			
			
			section = new Section();
			sse = new StyledStringElement("DataDownloader");
			sse.Tapped += delegate() {
				Do_DataDownloader();				
			};
			section.Add(sse);
			root.Add(section);
			
			Root = root;
		}
		
		
		private void Do_HttpWebRequest()
		{
			Stream remoteStream  = null;
			WebResponse response = null;
			UIAlertView alertView = null;
			
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				WebRequest request = WebRequest.Create(url);
				response = request.GetResponse();
				remoteStream = response.GetResponseStream();
				
				// Allocate a 1k buffer
				byte[] buffer = new byte[1024];
				int bytesRead;
				int bytesProcessed = 0;
				do
				{
					bytesRead = remoteStream.Read (buffer, 0, buffer.Length);
					stringBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
					
					// Write the data to the local file
					//localStream.Write (buffer, 0, bytesRead);
					
					// Increment total bytes processed
					bytesProcessed += bytesRead;
				} while (bytesRead > 0);
				
				alertView = new UIAlertView("Success", stringBuilder.ToString(), null, "Ok", null);
			}
			catch(Exception e)
			{
				alertView = new UIAlertView("Error", e.Message, null, "Ok", null);
				Console.WriteLine(e.Message);
			}
			finally
			{
				if (response != null)
					response.Close();
				
				if (remoteStream != null)
					remoteStream.Close();
			}			
			
			alertView.Show();
		}
		
		private void Do_HttpWebRequest_DefaultWebProxy()
		{
			
			Stream remoteStream  = null;
			WebResponse response = null;
			UIAlertView alertView = null;
			StringBuilder stringBuilder = new StringBuilder();
			
			try
			{
				WebRequest request = WebRequest.Create(url);
				//request.Proxy = WebRequest.DefaultWebProxy;
				request.Proxy.Credentials = WebRequest.DefaultWebProxy.Credentials;
				response = request.GetResponse();
				remoteStream = response.GetResponseStream();
				
				
				// Allocate a 1k buffer
				byte[] buffer = new byte[1024];
				int bytesProcessed = 0;
				int bytesRead;
				do
				{
					bytesRead = remoteStream.Read (buffer, 0, buffer.Length);
					stringBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
					
					// Write the data to the local file
					//localStream.Write (buffer, 0, bytesRead);
					
					// Increment total bytes processed
					bytesProcessed += bytesRead;
				} while (bytesRead > 0);
				
				alertView = new UIAlertView("Success", stringBuilder.ToString(), null, "Ok", null);
			}
			catch(Exception e)
			{
				alertView = new UIAlertView("Error", e.Message, null, "Ok", null);
				Console.WriteLine(e.Message);
			}
			finally
			{
				if (response != null)
					response.Close();
				
				if (remoteStream != null)
					remoteStream.Close();
			}		
			
			alertView.Show();
		}
		
		private void Do_WebClient()
		{			
			UIAlertView alertView = null;
			try
			{
				WebClient webClient = new WebClient();
				/*
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e) {
					InvokeOnMainThread( delegate {
						if (e.Error == null)
						{
							alertView = new UIAlertView("Success", e.Result, null, "Ok", null);
						}
						else
						{
							alertView = new UIAlertView("Error", e.Error.Message, null, "Ok", null);
						}
						alertView.Show();
					});
				};
				*/
				string result = webClient.DownloadString(url);
				alertView = new UIAlertView("Success", result, null, "Ok", null);
				alertView.Show();
				
			}
			catch(Exception e)
			{
				alertView = new UIAlertView("Error", e.Message, null, "Ok", null);
				alertView.Show();
				Console.WriteLine(e.Message);
			}
		}
		
		private void Do_NSData_FromUrl()
		{
			
			UIAlertView alertView = null;
			try
			{
				NSData data = NSData.FromUrl(new NSUrl(url));
				
				if (data == null)
				{
					alertView = new UIAlertView("Error", "NSData.FromUrl returned null", null, "Ok", null);
				}
				else
				{
					NSString dataString = NSString.FromData(data, NSStringEncoding.ASCIIStringEncoding);
					
					if (dataString == null)
					{
						alertView = new UIAlertView("Error", "NSString.FromData returned null", null, "Ok", null);
					}
					else
					{
						alertView = new UIAlertView("Success", dataString.ToString(), null, "Ok", null);
					}
				}				
			}
			catch(Exception e)
			{
				if (alertView == null)
					alertView = new UIAlertView("Error", e.Message, null, "Ok", null);
				
				Console.WriteLine(e.Message);
			}			
			
			alertView.Show();
		}
		
		private void Do_NSUrlConnection()			
		{
			//NSUrlConnection urlConnection;
			//string localFilename = Path.GetTempPath() + Path.GetFileName(url);
			
			UIAlertView alertView = null;
			try
			{
				NSUrlResponse response = null;
				NSError error = null;
				NSUrlRequest urlRequest = new NSUrlRequest(new NSUrl(url), NSUrlRequestCachePolicy.ReloadIgnoringLocalAndRemoteCacheData, 120);
				NSData data = NSUrlConnection.SendSynchronousRequest(urlRequest, out response, out error);
				
				if (data == null)
				{
					alertView = new UIAlertView("Error", "NSUrlConnection.SendSynchronousRequest returned null\n" + error.LocalizedDescription, null, "Ok", null);
				}
				else
				{
					NSString dataString = NSString.FromData(data, NSStringEncoding.ASCIIStringEncoding);
					
					if (dataString == null)
					{
						alertView = new UIAlertView("Error", "NSString.FromData returned null", null, "Ok", null);
					}
					else
					{
						alertView = new UIAlertView("Success", dataString.ToString(), null, "Ok", null);
					}
				}				
			}
			catch(Exception e)
			{
				if (alertView == null)
					alertView = new UIAlertView("Error", e.Message, null, "Ok", null);
				
				Console.WriteLine(e.Message);
			}	
			
			alertView.Show();
			
			
			//ProxyTest_NSUrlConnectionDelegate connectionDelegate = new ProxyTest_NSUrlConnectionDelegate();
			//urlConnection = new NSUrlConnection(DownloadRequest, connectionDelegate, true);			
		}
		
		private void Do_DataDownloader()
		{
			UIAlertView alertView = null;
			
			DataDownloader dataDownloader = new DataDownloader(url);
			dataDownloader.SyncDownload();
			
			if (dataDownloader.Exception == null)
			{
				alertView = new UIAlertView("Success", dataDownloader.Text, null, "Ok", null);
			}
			else
			{
				alertView = new UIAlertView("Error", dataDownloader.Exception.Message, null, "Ok", null);
			}
			
			alertView.Show();
		}
	}
}

