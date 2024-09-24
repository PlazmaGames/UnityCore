using PlazmaGames.Attribute;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlazmaGames.SO
{
    public abstract class SODatabase<TBase> : ScriptableObject where TBase : BaseSO
    {
        [SerializeField] protected List<TBase> _database;
        [SerializeField, InspectorButton("InitDatabase")] protected bool _initDatabase;
        [SerializeField, InspectorButton("ClearDatabase")] protected bool _clearDatabase;

        [ContextMenu(itemName: "Init Database")]
        public void InitDatabase()
        {
            ClearDatabase();

            List<TBase> foundTBases = Resources.LoadAll<TBase>("").OrderBy(e => e.id).Where(e => e.includeInDatabase).ToList();

            List<TBase> hasIDInRange = foundTBases.Where(e => e.id != -1 && e.id < foundTBases.Count).OrderBy(e => e.id).ToList();
            List<TBase> hasIDNotInRange = foundTBases.Where(e => e.id != -1 && e.id >= foundTBases.Count).OrderBy(e => e.id).ToList();
            List<TBase> noID = foundTBases.Where(e => e.id <= -1).ToList();

            int index = 0;
            for (int i = 0; i < foundTBases.Count; i++)
            {
                TBase newTBase;
                newTBase = hasIDInRange.Find(e => e.id == i);
                if (newTBase != null) _database.Add(newTBase);
                else if (index < noID.Count)
                {
                    noID[index].id = i;
                    newTBase = noID[index];
                    index++;
                    _database.Add(newTBase);
                }
            }

            foreach (TBase item in hasIDNotInRange)
            {
                item.id = _database.Count;
                _database.Add(item);
            }
        }

        [ContextMenu(itemName: "Clear Database")]
        public void ClearDatabase()
        {
            _database = new();

            List<TBase> foundTBases = Resources.LoadAll<TBase>("").ToList();
            foreach (TBase item in foundTBases)
            {
                item.id = -1;
            }
        }

        public List<TBase> GetAllEntries()
        {
            return new List<TBase>(_database);
        }

        public TBase GetEntry(int id)
        {
            return _database.Find(e => e.id == id);
        }

        public TBase GetEntry<T>(int id) where T : TBase
        {
            TBase card = _database.Find(e => e.id == id);

            if (card != null && card.GetType().IsAssignableFrom(typeof(T))) return card;
            else return null;
        }
    }
}
