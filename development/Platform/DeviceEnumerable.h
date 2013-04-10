#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"
#include "TypedDeviceHandle.h"
#include "DeviceEnumerator.h"

using namespace System;
using namespace System::Collections::Generic;




namespace RiftDotNet
{
	namespace Platform
	{
		generic <typename T> where T : DeviceBase
		public ref class DeviceEnumerable
			: public IEnumerable<TypedDeviceHandle<T>^>
		{
		public:

			DeviceEnumerable(const OVR::DeviceEnumerator<>& native)
			{
				_native = new OVR::DeviceEnumerator<>(native);
			}

			~DeviceEnumerable()
			{
				delete _native;
				_native = nullptr;
			}

			virtual IEnumerator<TypedDeviceHandle<T>^>^ GetEnumerator()
			{
				return gcnew Enumerator(new OVR::DeviceEnumerator<>(*_native));
			}

			virtual System::Collections::IEnumerator^ GetEnumerator2() = System::Collections::IEnumerable::GetEnumerator
			{
				return GetEnumerator();
			}

		private:

			ref class Enumerator sealed
				: public IEnumerator<TypedDeviceHandle<T>^>
			{
			public:

				Enumerator(OVR::DeviceEnumerator<>* native)
				{
					_native = native;
				}

				~Enumerator()
				{
					delete _native;
					_native = nullptr;
				}

				property TypedDeviceHandle<T>^ Current { 
					virtual TypedDeviceHandle<T>^ get() {
						auto type = (DeviceType)_native->GetType();
						if (type == DeviceType::None)
						{
							throw gcnew InvalidOperationException();
						}

						// It's important that we create a copy of the *current* state of the
						// enumeration, otherwise Next() is going to influence 
						return gcnew TypedDeviceHandle<T>(new OVR::DeviceHandle(*_native));
					}
				};

				property Object^ CurrentBase { 
					virtual Object^ get() = System::Collections::IEnumerator::Current::get { 
						return Current;
					} 
				};

				virtual bool MoveNext() { 
					return _native->Next();
				}

				virtual void Reset() { 
					_native->Clear();
				}

			private:

				OVR::DeviceEnumerator<>* _native;
			};

		private:

			OVR::DeviceEnumerator<>* _native;
		};
	}
}