RiftDotNet
==========

This project is a simple .NET wrapper around the Oculus Rift SDK. The structure is virtually identical to the one of the SDK, and thus the c++ samples
can easily be substituted for .NET, for example C#, code.

Using RiftDotNet
================

In order to use this project, a reference to RiftDotNet and RiftDotNet.Interface is required. Those assemblies are built with
any CPU and work both on x86 and x64 systems. The static Factory class can then be used to create a DeviceManager and a SensorFusion,
almost identical to the original SDK. The interfaces in this wrapper have similar names as the c++ sdk (for example RiftDotNet.IHMDInfo <==> OVR::HMDInfo)
and should be straight forward to use.

External dependencies are SharpDX (for mathematical structures like quaternion or vectors) and Log4Net.

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
