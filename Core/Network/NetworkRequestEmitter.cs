using System;
using System.Collections.Generic;
using System.Linq;

namespace PlazmaGames.Core.Network
{
	internal sealed class NetworkRequestEmitter
	{
		Dictionary<int, (List<PacketReader>, List<Action<PacketReader>>)> _events = new Dictionary<int, (List<PacketReader>, List<Action<PacketReader>>)>();

        public void Emit(int type, PacketReader val)
		{
			if (!_events.ContainsKey(type)) return;
			_events[type].Item1.Add(val);
		}

		public void Subscribe(int type, Action<PacketReader> callback)
		{
			if (!_events.ContainsKey(type)) _events[type] = (new List<PacketReader>(), new List<Action<PacketReader>>());
			_events[type].Item2.Add(callback);
		}

		public void Unsubscribe(int type, Action<PacketReader> callback)
		{
            if (!_events.ContainsKey(type)) return;
            _events[type].Item2.Remove(callback);
        }

		public void CheckForRequest()
		{
            foreach ((List<PacketReader>, List<Action<PacketReader>>) data in _events.Values.ToList())
			{
                for (int i = data.Item1.Count - 1; i >= 0; i--)
                {
                    foreach (Action<PacketReader> act in data.Item2) act.Invoke(new PacketReader(data.Item1[i].GetPacket()));
                    data.Item1.RemoveAt(i);
                }
            }
        }
	}
}
