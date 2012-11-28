using System;
using MonoTouch.Foundation;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace ProxyTest
{
	public class DataDownloader
	{
		public event EventHandler<AsyncDownloadComplete_EventArgs> AsyncDownloadComplete = null;
		public event EventHandler<AsyncDownloadProgressChanged_EventArgs> AsyncDownloadProgressChanged = null;
		public event EventHandler<AsyncDownloadFailed_EventArgs> AsyncDownloadFailed = null;


		private string _url = string.Empty;
		public string URL
		{
			get { return _url; }
		}

		private StringBuilder _stringBuilder = new StringBuilder();
		public String Text
		{
			get { return _stringBuilder.ToString(); }
		}
		
		private Exception _exception = null;
		public Exception Exception
		{
			get { return _exception; }
		}
		
		private string _post = String.Empty;
		public string POST
		{
			get { return _post; }
			set { _post = value; }
		}
		
		private double _timeout = 120;
		public double Timeout
		{
			get { return _timeout; }
			set { _timeout = value; }
		}

		public DataDownloader(string url)
		{
			_exception = null;
			_url = url;
		}
		
		public void AsyncDownload()
		{
			Download(true, String.Empty);
		}

		public void AsyndDownloadToFile(string path)
		{
			Download(true, path);
		}
		
		public void SyncDownload()
		{
			Download(false, String.Empty);
		}

		public void SyndDownloadToFile(string path)
		{
			Download(false, path);
		}

		private void Download(bool isAsync, string path)
		{
			_stringBuilder.Clear();
			
			NSMutableUrlRequest request = null;
			NSError error = null;
			
			try
			{
#if DEBUG				
				DateTime start = DateTime.Now;
#endif
				request = new NSMutableUrlRequest(new NSUrl(_url), NSUrlRequestCachePolicy.ReloadIgnoringLocalAndRemoteCacheData, _timeout);
				
				if (_post != String.Empty)
				{
					request.HttpMethod = "POST";
					request["Content-Length"] = _post.Length.ToString();;
					request["Content-Type"] = "application/x-www-form-urlencoded";
					request.Body = _post;
				}
				
				if (isAsync)
				{
					DataDownloader_NSUrlConnectionDelegate connectionDelegate = new DataDownloader_NSUrlConnectionDelegate();

					connectionDelegate.AsyncDownloadComplete += delegate(object sender, AsyncDownloadComplete_EventArgs e) {
						if (path != String.Empty)
						{
							((DataDownloader_NSUrlConnectionDelegate)sender).WriteResultToFile(path);
						}

						if (AsyncDownloadComplete != null)
						{
							AsyncDownloadComplete(this, e);
						}
					};




					if (AsyncDownloadProgressChanged != null)
					{
						connectionDelegate.AsyncDownloadProgressChanged += delegate(object sender, AsyncDownloadProgressChanged_EventArgs e) {
							AsyncDownloadProgressChanged(this, e);
						};
					}

					if (AsyncDownloadFailed != null)
					{
						connectionDelegate.AsyncDownloadFailed += delegate(object sender, AsyncDownloadFailed_EventArgs e) {
							AsyncDownloadFailed(this, e);
						};
					}

					NSUrlConnection connection = new NSUrlConnection(request, connectionDelegate, true);
				}
				else
				{
					
					NSUrlResponse response = null;


					NSData output = NSUrlConnection.SendSynchronousRequest(request, out response, out error);
					
					if (output == null)
					{
						if (response == null)
						{
							// Could not connect to server.
						}
						else
						{
							// do something with response, error maybe?
						}
					}

					if (path != String.Empty)
					{
						File.WriteAllText(path, output.ToString());
					}

					_stringBuilder = new StringBuilder(output.ToString());
					Console.WriteLine(_stringBuilder.ToString());
				}
#if DEBUG
				Console.WriteLine("NSData.FromUrl: " + (DateTime.Now - start).TotalMilliseconds);
				start = DateTime.Now;
#endif
			}
			catch (Exception err)
			{
				_exception = err;
				Console.WriteLine("StreamLoading Error: " + err.Message);
			}
			finally
			{

			}
		}
		
		
		public class DataDownloader_NSUrlConnectionDelegate : NSUrlConnectionDelegate
		{
			long bytesReceived = 0;
			long expectedBytes = 0;
			float progress = 0;
			byte [] result;

			public event EventHandler<AsyncDownloadComplete_EventArgs> AsyncDownloadComplete = null;
			public event EventHandler<AsyncDownloadProgressChanged_EventArgs> AsyncDownloadProgressChanged = null;			
			public event EventHandler<AsyncDownloadFailed_EventArgs> AsyncDownloadFailed = null;

			public DataDownloader_NSUrlConnectionDelegate()
			{
				result = new byte [0];
			}
			
			public override void ReceivedResponse(NSUrlConnection connection, NSUrlResponse response)
			{
				expectedBytes = response.ExpectedContentLength;
			}

			public void WriteResultToFile(string path)
			{
				File.WriteAllBytes(path, result);
			}
			
			public override void ReceivedData(NSUrlConnection connection, NSData data)
			{
				byte [] nb = new byte [result.Length + data.Length];
				result.CopyTo(nb, 0);
				Marshal.Copy(data.Bytes, nb, result.Length, (int) data.Length);
				result = nb;				
				
				uint receivedLen = data.Length;
				bytesReceived = (bytesReceived + receivedLen);

				//progress = ((bytesReceived/(float)expectedBytes)*100)/100;
				progress = (bytesReceived/(float)expectedBytes);

				if (AsyncDownloadProgressChanged != null)
				{
					AsyncDownloadProgressChanged(this, new AsyncDownloadProgressChanged_EventArgs(progress));
				}
			}
			
			public override void FinishedLoading(NSUrlConnection connection)
			{
				if (AsyncDownloadComplete != null)
				{
					AsyncDownloadComplete(this, new AsyncDownloadComplete_EventArgs());
				}
			}
			
			public override void FailedWithError(NSUrlConnection connection, NSError error)
			{
				if (AsyncDownloadFailed != null)
				{
					AsyncDownloadFailed(this, new AsyncDownloadFailed_EventArgs(error));
				}
			}
		}


		public class AsyncDownloadComplete_EventArgs : EventArgs
		{
			
		}
		
		public class AsyncDownloadFailed_EventArgs : EventArgs
		{
			public NSError Error { get; internal set; }
			
			public AsyncDownloadFailed_EventArgs(NSError error)
			{
				this.Error = error;
			}
		}
		
		public class AsyncDownloadProgressChanged_EventArgs : EventArgs
		{
			public float Progress { get; internal set; }
			
			public AsyncDownloadProgressChanged_EventArgs(float progress)
			{
				this.Progress = progress;
			}
		}
		

		
		
	}
}

