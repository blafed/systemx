using System;
using System.Collections.Generic;
using UnityEngine;

namespace React.Fake
{
    public class ObjectContainerScript : MonoBehaviour
    {
        public ObjectContainer container = new ObjectContainer();
        public List<FakeDepend> fakeDepends = new List<FakeDepend>();

        public static ObjectContainerScript current =>
            !_current ? _current = FindObjectOfType<ObjectContainerScript>() : _current;
        private static ObjectContainerScript _current;
    }
}