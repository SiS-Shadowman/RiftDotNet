using log4net;

namespace RiftDotNet
{
	public interface ITestingFactory
		: IFactory
	{
		/// <summary>
		/// Creates an empty ISensorInfo instance.
		/// </summary>
		/// <returns></returns>
		ISensorInfo CreateSensorInfo();

		/// <summary>
		/// Creates an empty IHMDInfo instance.
		/// </summary>
		/// <returns></returns>
		IHMDInfo CreateHMDInfo();

		/// <summary>
		/// A reference to the logger used by the Rift SDK.
		/// </summary>
		/// <returns></returns>
		ILog GetLogger();
	}
}