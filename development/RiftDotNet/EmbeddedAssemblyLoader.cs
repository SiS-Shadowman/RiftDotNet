using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RiftDotNet
{
	public sealed class EmbeddedAssemblyLoader
		: IDisposable
	{
		public EmbeddedAssemblyLoader()
		{
			AppDomain.CurrentDomain.AssemblyResolve += Resolve;

			/*switch (Marshal.SizeOf(IntPtr.Zero))
			{
				case 8:
					Resolve("RiftDotNet.x64.dll");
					break;

				default:
					Resolve("RiftDotNet.Win32.dll");
					break;
			}*/
		}

		#region IDisposable Members

		public void Dispose()
		{
			AppDomain.CurrentDomain.AssemblyResolve -= Resolve;
		}

		#endregion

		private Assembly Resolve(object sender, ResolveEventArgs args)
		{
			return Resolve(args.Name);
		}

		private Assembly Resolve(string name)
		{
			Assembly assembly = Assembly.GetCallingAssembly();
			Stream imageStream = assembly.GetManifestResourceStream("RiftDotNet." + name);
			if (imageStream == null)
				return null;

			long bytestreamMaxLength = imageStream.Length;
			var buffer = new byte[bytestreamMaxLength];
			imageStream.Read(buffer, 0, (int)bytestreamMaxLength);

			// For fucks sake, microsoft is a huge dick.
			// Where's the fucking difference between dumping the shitty byte array to
			// a file versus loading it from memory?!?
			Assembly requestedAssembly = Assembly.Load(buffer);
			return requestedAssembly;
		}
	}
}