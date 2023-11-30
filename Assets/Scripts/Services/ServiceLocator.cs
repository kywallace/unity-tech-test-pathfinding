#nullable enable

using System.Collections.Generic;
using System;

namespace Services
{
	internal class ServiceLocator
	{
		private static ServiceLocator? _locator = null;

		private readonly Dictionary<object, object> _services = new();

		public static ServiceLocator Instance => _locator ??= new ServiceLocator();

		public void AddService<T>(T service)
		{
			_services.Add(typeof(T), service!);
		}

		public T GetService<T>()
		{
			try
			{
				return (T)_services[typeof(T)];
			}
			catch (Exception)
			{
				throw new NotImplementedException("Service not available.");
			}
		}
	}
}

#nullable disable
