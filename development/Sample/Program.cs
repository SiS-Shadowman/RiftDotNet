using System;
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
			using (var manager = Factory.CreateDeviceManager())
			using (var fusion = Factory.CreateSensorFusion())
			{
				IHMDDevice device = null;
				while (true)
				{
					Console.WriteLine("Looking for devices...");
					using (var devices = manager.HMDDevices)
					{
						if (devices.Length == 0)
						{
							Console.WriteLine("Could not find a rift device.");
							Console.WriteLine("Please attach your device and press any key to continue");
							Console.ReadKey();
							Console.WriteLine();
						}
						else
						{
							var handle = devices[0];
							Console.WriteLine("Found a device: {0}",
								handle.DeviceInfo.DisplayDevice);

							device = handle.CreateDevice();
							break;
						}
					}
				}

				// Now you've got a reference to the rift device. Go crazy with it ;)
				// (But don't forget to dispose of it ;)

				device.Dispose();
			}
		}
	}
}