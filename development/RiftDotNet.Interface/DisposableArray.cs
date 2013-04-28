using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RiftDotNet
{
	public sealed class DisposableArray<T>
		: IEnumerable<T>
		  , IDisposable
		where T : class, IDisposable
	{
		private readonly T[] _data;

		public DisposableArray()
		{
			_data = new T[0];
		}

		public DisposableArray(IEnumerable<T> data)
		{
			_data = data.ToArray();
		}

		public DisposableArray(T[] data)
		{
			if (data == null)
				throw new ArgumentNullException();

			_data = data;
		}

		public T this[int index]
		{
			get { return _data[index]; }
			set { _data[index] = value; }
		}

		public int Length
		{
			get { return _data.Length; }
		}

		#region IDisposable Members

		public void Dispose()
		{
			for (int i = 0; i < _data.Length; ++i)
			{
				var value = _data[i];
				if (value != null)
				{
					value.Dispose();
					_data[i] = null;
				}
			}
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>) _data).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}