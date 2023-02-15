using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class PairList<K, V> : IEnumerable<PairList<K, V>.Item>
{

    [System.Serializable]
    public struct Item
    {
        public K key;
        public V value;

        public Item(K key, V value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public List<Item> items = new List<Item>();


    public PairList()
    {

    }
    public PairList(Dictionary<K, V> from)
    {
        foreach (var x in from)
        {
            set(x.Key, x.Value);
        }
    }

    public bool isset(K key)
    {

        foreach (var x in items)
        {
            if (Equals(key, x.key))
                return true;
        }
        return false;
    }

    public V get(K key, V def = default)
    {
        foreach (var x in items)
        {
            if (Equals(key, x.key))
                return x.value;
        }
        return def;
    }
    public void Add(K key, V value)
    {
        set(key, value);
    }
    public void set(K key, V value)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (Equals(items[i].key, key))
            {
                items[i] = new Item { key = key, value = value };
                return;
            }
        }
        items.Add(new Item { key = key, value = value });
    }

    public PairList<K, V> clone()
    {
        var p = new PairList<K, V>();
        p.items = new List<Item>(this.items);
        return p;
    }

    IEnumerator<Item> IEnumerable<Item>.GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return items.GetEnumerator();
    }

}