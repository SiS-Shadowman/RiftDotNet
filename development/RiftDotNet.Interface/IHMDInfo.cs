namespace RiftDotNet
{
	/// <summary>
	/// This structure describes various aspects of the HMD allowing us to configure rendering.
	///
	///  Currently included data:
	///   - Physical screen dimensions, resolution, and eye distances.
	///     (some of these will be configurable with a tool in the future).
	///     These arguments allow us to properly setup projection across HMDs.
	///   - DisplayDeviceName for identifying HMD screen; system-specific interpretation.
	///
	/// </summary>
	public interface IHMDInfo
		: IDeviceInfo
	{
		/// <summary>
		/// Size of the entire screen, in pixels.
		/// </summary>
		uint HResolution { get; }

		/// <summary>
		/// Size of the entire screen, in pixels.
		/// </summary>
		uint VResolution { get; }

		/// <summary>
		/// Physical dimensions of the active screen in meters. Can be used to calculate
		/// projection center while considering IPD.
		/// </summary>
		float HScreenSize { get; }

		/// <summary>
		/// Physical dimensions of the active screen in meters. Can be used to calculate
		/// projection center while considering IPD.
		/// </summary>
		float VScreenSize { get; }

		/// <summary>
		/// Physical offset from the top of the screen to the eye center, in meters.
		/// This will usually, but not necessarily be half of VScreenSize.
		/// </summary>
		float VScreenCenter { get; }

		/// <summary>
		/// Distance from the eye to screen surface, in meters.
		/// Useful for calculating FOV and projection.
		/// </summary>
		float EyeToScreenDistance { get; }

		/// <summary>
		/// Distance between physical lens centers useful for calculating distortion center.
		/// </summary>
		float LensSeparationDistance { get; }

		/// <summary>
		/// Configured distance between the user's eye centers, in meters. Defaults to 0.064.
		/// </summary>
		float InterpupillaryDistance { get; }

		/// <summary>
		/// Radial distortion correction coefficients.
		/// The distortion assumes that the input texture coordinates will be scaled
		/// by the following equation:    
		///   uvResult = uvInput * (K0 + K1 * uvLength^2 + K2 * uvLength^4)
		/// Where uvInput is the UV vector from the center of distortion in direction
		/// of the mapped pixel, uvLength is the magnitude of that vector, and uvResult
		/// the corresponding location after distortion.
		/// </summary>
		float[] DistortionK { get; }

		/// <summary>
		/// Desktop coordinate position of the screen (can be negative; may not be present on all platforms)
		/// </summary>
		int DesktopX { get; }

		/// <summary>
		/// Desktop coordinate position of the screen (can be negative; may not be present on all platforms)
		/// </summary>
		int DesktopY { get; }

		/// <summary>
		/// Windows:
		/// String - "\\\\.\\DISPLAY3", etc. Can be used in EnumDisplaySettings/CreateDC.
		/// MacOS:
		/// int - DisplayId
		/// </summary>
		object DisplayDevice { get; }
	}
}