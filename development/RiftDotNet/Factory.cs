using System;
using System.Runtime.InteropServices;

namespace RiftDotNet
{
	/// <summary>
	///     This class is meant to be used to create the objects for communicating
	///     with oculus rift hardware.
	///     Any application should create one device manager in order to query the devices
	///     currently installed on the system.
	/// </summary>
	public static class Factory
	{
		/// <summary>
		///     The actual factory corresponding to the platform this code is currently
		///     executed on. The actual implementation is written in c++\cli.
		/// </summary>
		private static IFactory _platformFactory;

		/// <summary>
		/// For testing purposes only.
		/// </summary>
		public static IFactory PlatformFactory
		{
			get{ return _platformFactory; }
		}

		static Factory()
		{
			// Due the way the JIT-compiler works, the methods are not compiled unless executed.
			// This also means that the referenced assemblies are not loaded before, hence this works.
			// However it will fail horribly if microsoft decides to change the rules for its jit
			// compiler.
			if (Environment.Is64BitProcess)
				Loadx64();
			else
				LoadWin32();
		}

		private static void Loadx64()
		{
			_platformFactory = x64.Factory.Instance;
		}

		private static void LoadWin32()
		{
			_platformFactory = Win32.Factory.Instance;
		}

		/// <summary>
		///     Creates a new device manager which can be used to communicate with the HMD.
		/// </summary>
		/// <remarks>
		///     The caller is responsible for disposing of the returned object.
		/// </remarks>
		/// <remarks>
		///     Only one instance of IDeviceManager is needed for everyday use.
		/// </remarks>
		/// <returns></returns>
		public static IDeviceManager CreateDeviceManager()
		{
			return _platformFactory.CreateDeviceManager();
		}

		/// <summary>
		///     Creates a high-level object which accumulates information from the given
		///     sensor and exposes it through various properties, such as orientation, acceleration
		///     etc..
		/// </summary>
		/// <remarks>
		///     The caller is responsible for disposing of the returned object.
		/// </remarks>
		/// <param name="sensorDevice">The sensor device who's input is to be processed</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">In case null is passed for the sensor device</exception>
		public static ISensorFusion CreateSensorFusion(ISensorDevice sensorDevice = null)
		{
			return _platformFactory.CreateSensorFusion(sensorDevice);
		}
	}
}