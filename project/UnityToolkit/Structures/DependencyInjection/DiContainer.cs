using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// ReSharper disable CanSimplifyDictionaryLookupWithTryGetValue

namespace UnityToolkit.Structures.DependencyInjection;

/// <summary>
/// The dependency injection container. Handles resolving and injecting services that are registered.
/// </summary>
public class DiContainer
{
	[NotNull] private readonly Dictionary<Type, Dictionary<string, Func<DiContainer, string, object>>> _services = [];
	[NotNull] private readonly Dictionary<Type, Dictionary<string, object>> _singletonServices = [];
	
	private const string DEFAULT_KEY = "default";
	
	/// <summary>
	/// Creates and registers a Func, which will create a new instance of TImplementation, under the type TService.
	/// </summary>
	/// <typeparam name="TService">The type the service will be registered under.</typeparam>
	/// <typeparam name="TImplementation">The actual service type implementation.</typeparam>
	public void AddTransient<TService, TImplementation>([NotNull] string key = DEFAULT_KEY)
		where TService : class
		where TImplementation : TService
	{
		Type serviceType = typeof(TService);
		
		if (_singletonServices.ContainsKey(serviceType))
		{
			throw new InvalidOperationException($"Service '{serviceType.FullName}' is already registered as a singleton service.");
		}
		
		if (_services.ContainsKey(serviceType))
		{
			_services[serviceType][key] = ServiceCreator;
			return;
		}
		
		_services[serviceType] = new Dictionary<string, Func<DiContainer, string, object>>
		{
			{ key, ServiceCreator }
		};
		return;
		
		object ServiceCreator(DiContainer container, string dependencyKey) =>
			container.CreateInstance(typeof(TImplementation), dependencyKey);
	}
	
	/// <summary>
	/// Creates and registers an instance of TService. Only the latest instance will be registered.
	/// </summary>
	/// <typeparam name="TService">The type the service will be registered under.</typeparam>
	/// /// <typeparam name="TImplementation">The actual service type implementation.</typeparam>
	public void AddSingleton<TService, TImplementation>([NotNull] string key = DEFAULT_KEY)
		where TService : class
		where TImplementation : TService
	{
		Type serviceType = typeof(TService);
		Type implementationType = typeof(TImplementation);
		
		if (_services.ContainsKey(serviceType))
		{
			throw new InvalidOperationException($"Service '{serviceType.FullName}' is already registered as a transient service.");
		}
		
		if (_singletonServices.ContainsKey(serviceType))
		{
			_singletonServices[serviceType][key] = CreateInstance(implementationType, key);
			return;
		}
		
		_singletonServices[serviceType] = new Dictionary<string, object>
		{
			{ key, CreateInstance(implementationType, key) }
		};
	}
	
	/// <summary>
	/// Resolves the given type and retrieves the associated instance.
	/// </summary>
	/// <typeparam name="TService">The type to resolve from the container's registrations.</typeparam>
	/// <returns>The instance object associated with the given type.</returns>
	[NotNull]
	public TService Resolve<TService>([NotNull] string key = DEFAULT_KEY)
	{
		return (TService)Resolve(typeof(TService), key);
	}
	
	[NotNull]
	private object Resolve([NotNull] Type type, [NotNull] string key)
	{
		if (_singletonServices.ContainsKey(type) &&
			(_singletonServices[type].TryGetValue(key, out object instance) || _singletonServices[type].TryGetValue(DEFAULT_KEY, out instance)))
		{
			return instance;
		}
		
		if (_services.ContainsKey(type) &&
			(_services[type].TryGetValue(key, out Func<DiContainer, string, object> creator) || _services[type].TryGetValue(DEFAULT_KEY, out creator)))
		{
			return creator(this, key);
		}
		
		throw new InvalidOperationException($"No registration for {type.FullName} was found.");
	}
	
	[NotNull]
	private object CreateInstance([NotNull] Type type, [NotNull] string key)
	{
		ConstructorInfo constructor = type.GetConstructors().FirstOrDefault();
		if (constructor == null)
		{
			throw new InvalidOperationException($"No public constructor found for {type.FullName}");
		}
		
		ParameterInfo[] parameters = constructor.GetParameters();
		int parametersLength = parameters.Length;
		var parametersToInject = new object[parametersLength];
		
		for (var i = 0; i < parametersLength; i++)
		{
			ParameterInfo parameter = parameters[i];
			parametersToInject[i] = Resolve(parameter.ParameterType, key);
		}
		
		return Activator.CreateInstance(type, parametersToInject)!;
	}
}