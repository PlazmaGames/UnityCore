using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlazmaGames.Core.MonoSystem;
using PlazmaGames.Core.Network;
using PlazmaGames.Settings;
using PlazmaGames.Core.Events;

namespace PlazmaGames.Core
{
	public abstract class GameManager : MonoBehaviour
	{
		protected static GameManager _instance;

		private readonly MonoSystemManager _monoSystemManager = new MonoSystemManager();
		private readonly EventManager _eventManager = new EventManager();
		private readonly NetworkRequestEmitter _networkEmitter = new NetworkRequestEmitter();

		/// <summary>
		/// Add a MonoSystems to the GameManager. A MonoSystem takes the place of other singleton classes.
		/// </summary>
		public static void AddMonoSystem<TMonoSystem, TBindTo>(TMonoSystem monoSystem) where TMonoSystem : IMonoSystem, TBindTo => _instance._monoSystemManager.AddMonoSystem<TMonoSystem, TBindTo>(monoSystem);

		/// <summary>
		/// Fetches an attached MonoSystem of type TMonoSystem.
		/// </summary>
		public static TMonoSystem GetMonoSystem<TMonoSystem>() => _instance._monoSystemManager.GetMonoSystem<TMonoSystem>();

		/// <summary>
		/// Adds an event listener to an event of type TEvent
		/// </summary>
		public static void AddEventListener<TEvent>(TEvent eventType, EventListener listener) where TEvent : IComparable => _instance._eventManager.AddListener(Convert.ToInt32(eventType), listener);

		/// <summary>
		/// Removes an event listener to an event of type TEvent
		/// </summary>
		public static void RemoveEventListener<TEvent>(TEvent eventType, EventListener listener) where TEvent : IComparable => _instance._eventManager.RemoveListener(Convert.ToInt32(eventType), listener);

		/// <summary>
		/// Emits a game event of type TEvent.
		/// </summary>
		public static void EmitEvent<TEvent>(TEvent eventType, Component sender = null, object data = null) where TEvent : IComparable => _instance._eventManager.Emit(Convert.ToInt32(eventType), sender, data);

		/// <summary>
		/// Emit a network request of type TRequest.
		/// </summary>
		public static void EmitNetworkRequest<TRequest>(TRequest type, PacketReader packet, int fromID = -1) where TRequest : IComparable => _instance._networkEmitter.Emit(Convert.ToInt32(type), packet, fromID);

		/// <summary>
		/// Attaches a callback linked to a network request of type TRequest.
		/// </summary>
		public static void AddNetworkRequest<TRequest>(TRequest type, Action<PacketReader, int> callback) where TRequest : IComparable => _instance._networkEmitter.Subscribe(Convert.ToInt32(type), callback);

		/// <summary>
		/// Removes a callback linked to a network request of type TRequest.
		/// </summary>
		public static void RemoveNetworkRequest<TRequest>(TRequest type, Action<PacketReader, int> callback) where TRequest : IComparable => _instance._networkEmitter.Unsubscribe(Convert.ToInt32(type), callback);

		/// <summary>
		/// Initialzes the GameManager automatically on scene load.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			if (_instance) return;

			PlazmaGamesSettings settings = PlazmaGamesSettings.GetSettings();
			string prefabPath = (settings != null) ? settings.GetSceneGameManagerNameOrDefault(SceneManager.GetActiveScene().name) : "GameManager";

			GameManager gameManagerPrefab = Resources.Load<GameManager>(prefabPath);
			GameManager gameManager = Instantiate(gameManagerPrefab);

			gameManager.name = gameManager.GetApplicationName();

			DontDestroyOnLoad(gameManager);

			_instance = gameManager;

			gameManager.OnInitalized();
		}

		/// <summary>
		/// Fetches the name of the application.
		/// </summary>
		protected abstract string GetApplicationName();

		/// <summary>
		/// Function to be ran after the GameManager is Initalized.
		/// </summary>
		protected abstract void OnInitalized();

		protected virtual void Update()
		{
			_networkEmitter.CheckForRequest();
		}
	}
}
