using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace React.Fake
{
    [System.Serializable]
    public class ObjectContainer : IList<Object>
    {
        [SerializeField] private List<Object> objs = new List<Object>();
        public IEnumerator<Object> GetEnumerator()
        {
            return objs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) objs).GetEnumerator();
        }

        public void Add(Object item)
        {
            objs.Add(item);
        }

        public void Clear()
        {
            objs.Clear();
        }

        public bool Contains(Object item)
        {
            return objs.Contains(item);
        }

        public void CopyTo(Object[] array, int arrayIndex)
        {
            objs.CopyTo(array, arrayIndex);
        }

        public bool Remove(Object item)
        {
            return objs.Remove(item);
        }

        public int Count => objs.Count;

        public bool IsReadOnly => false;

        public int IndexOf(Object item)
        {
            return objs.IndexOf(item);
        }

        public void Insert(int index, Object item)
        {
            objs.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            objs.RemoveAt(index);
        }

        public Object this[int index]
        {
            get => objs[index];
            set => objs[index] = value;
        }
    }

    // public class FakeDependContainer
    // {
    //     public List<FakeDepend> list = new List<FakeDepend>();
    //     
    //     public int add(FakeDepend d){}
    // }
}