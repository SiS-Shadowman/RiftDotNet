using RiftDotNet;
using log4net;

namespace Sample
{
	internal class Program
	{
		/// <summary>
		/// Without this line, log4net refuses to do anything.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

		private static void Main(string[] args)
		{
			var manager = Factory.CreateDeviceManager();
			var devices = manager.HMDDevices;
			var device = devices[0];
		}
	}
}