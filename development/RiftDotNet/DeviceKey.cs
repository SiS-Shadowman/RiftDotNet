using System;

namespace RiftDotNet
{
	/// <summary>
	/// This structure uniquely identifies device instances.
	/// </summary>
	public struct DeviceKey : IEquatable<DeviceKey>
	{
		private readonly DeviceType _deviceType;
		private readonly string _manufacturer;
		private readonly string _productName;
		private readonly uint _version;
		private readonly string _serialNumber;

		public DeviceKey(ISensorInfo deviceInfo)
		{
			_deviceType = deviceInfo.Type;
			_productName = deviceInfo.ProductName;
			_manufacturer = deviceInfo.Manufacturer;
			_version = deviceInfo.Version;
			_serialNumber = deviceInfo.SerialNumber;
		}

		#region IEquatable<DeviceKey> Members

		public bool Equals(DeviceKey other)
		{
			return _deviceType == other._deviceType &&
			       string.Equals(_productName, other._productName) &&
			       string.Equals(_manufacturer, other._manufacturer) &&
			       string.Equals(_serialNumber, other._serialNumber) &&
			       _version == other._version;
		}

		#endregion

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is DeviceKey && Equals((DeviceKey)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (int)_deviceType;
				hashCode = (hashCode * 397) ^ (_productName != null ? _productName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (_manufacturer != null ? _manufacturer.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int)_version;
				hashCode = (hashCode * 397) ^ (_serialNumber != null ? _serialNumber.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(DeviceKey left, DeviceKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(DeviceKey left, DeviceKey right)
		{
			return !left.Equals(right);
		}
	}
}