using System;
using MonoTouch.Foundation;
using System.Runtime.InteropServices;
using System.IO;

namespace ProxyTest
{
	public class ProxyTest_NSUrlConnectionDelegate : NSUrlConnectionDelegate
	{
		long bytesReceived = 0;
		float percentComplete = 0;
		long expectedBytes = 0;
		float progress = 0;
		byte [] result;

		public ProxyTest_NSUrlConnectionDelegate()
		{
			result = new byte [0];
		}

		public override void ReceivedResponse(NSUrlConnection connection, NSUrlResponse response)
		{
			expectedBytes = response.ExpectedContentLength;
		}

		public override void ReceivedData(NSUrlConnection connection, NSData data)
		{

			byte [] nb = new byte [result.Length + data.Length];
			result.CopyTo(nb, 0);
			Marshal.Copy(data.Bytes, nb, result.Length, (int) data.Length);
			result = nb;



			uint receivedLen = data.Length;
			bytesReceived = (bytesReceived + receivedLen);


			//if(expectedBytes != NSUrlResponse.) {
			progress = ((bytesReceived/(float)expectedBytes)*100)/100;
			percentComplete = progress*100;

			Console.WriteLine(progress + " - " + percentComplete);
			//}
		}
	
		public override void FinishedLoading(NSUrlConnection connection)
		{
			//File.WriteAllBytes(Path.GetTempPath() + "temp.pdf", result);
		}

		public override void FailedWithError(NSUrlConnection connection, NSError error)
		{

		}
	}
}

