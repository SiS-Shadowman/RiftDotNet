RiftDotNet
==========

This project is a c# wrapper around the Oculus Rift SDK. It contains simple wrappers around most the native c++ classes which
can be found in the SDK, and also a high level interface, which is actually the recommended way to go.

Unfortunately, my developer kit has not been shipped yet, as of today (April 24. 2013), which means that big parts of the code have not
been tested yet. It is possible that this code still contains some bugs. If you already have a developer kit or spot a bug 
by browsing through this code, please report it.

Using RiftDotNet
================

In order to use this project, a reference to RiftDotNet and RiftDotNet.Interface is required. Those assemblies are built with
any CPU and work both on x86 and x64 systems. External dependencies are SharpDX (for mathematical structures like quaternion or vectors) and Log4Net
which can be found [here](http://sharpdx.org/download/) and [here](http://logging.apache.org/log4net/download_log4net.cgi).

Example using HMDManager (Recommended)
======================================

The following example shows how the SDK can be used:

```c
using (var mgr = new HMDManager())
{
	// We need to ensure that the user has attached his rift to the computer
	// or wait until he has done so.
	var hmd = mgr.AttachedDevice;
	if (hmd == null)
	{
		Console.WriteLine("Please attach your rift to the computer...");
		hmd = mgr.WaitForAttachedDevice(null);
	}

	Console.WriteLine("Found HMD at: {0}", hmd.Info.DisplayDevice);
	Console.WriteLine("Manufacturer: {0}", hmd.Info.Manufacturer);

	hmd.Reset();
	var orientation = hmd.Orientation;
}
```

Example using the native wrapper (Not recommended)
==================================================

The following examples shows how the wrapper around the native SDK can be used:
(In most cases, this should not be needed)

```c
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
```

IDisposable caveats
===================

Most interfaces (when their implementations wrap a native resource) implement IDisposable and *must* be disposed manually, when
they are no longer needed.
Furthermore, some interfaces have properties which themselves implement IDisposable and/or have methods which returns an object
which implements IDisposable. The rule of thumb for using them is: The returned values needs to be disposed of when it is no
longer needed, e.g. referenced by the calling code.

For example, the IDevice interface contains a Parent property. Every access to this property must be accompanied by a call to dispose, otherwise
native resources are leaked (until the GC decides to finalize the wrapper classes):

void DoSomething(IDevice device)
{
	using (var parent = device.Parent)
	{
		if (parent != null)
		{
			DoSomethingElse(parent);
		}
	}
}

This behaviour is consistent with the implementation of SharpDX.

Behind the scenes
=================

The wrapper classes are written in c++/cli and link against the native SDK,
which is compiled as a static library. This c++/cli project is built for both x64 and the x86 platform
and referenced by the RiftDotnet which selects the correct implementation based on the bitness of the
runtime.
