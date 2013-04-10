namespace RiftDotNet
{
	/// <summary>
	/// SensorRange specifies maximum value ranges that SensorDevice hardware is configured
	/// to detect. Although this range doesn't affect the scale of MessageBodyFrame values,
	/// physical motions whose positive or negative magnitude is outside the specified range
	/// may get clamped or misreported. Setting lower values may result in higher precision
	/// tracking.
	/// </summary>
	public struct SensorRange
	{
		public float MaxAcceleration;
		public float MaxMagneticField;
		public float MaxRotationRate;

		public SensorRange(float maxAcceleration, float maxRotationRate, float maxMagneticField)
		{
			MaxAcceleration = maxAcceleration;
			MaxRotationRate = maxRotationRate;
			MaxMagneticField = maxMagneticField;
		}
	}
}