using RiftDotNet;

namespace Sample
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var manager = Factory.CreateDeviceManager();
			var devices = manager.HMDDevices;
			var device = devices[0];
		}
	}
}