using UnityEngine;
using System.Collections.Generic;

namespace React
{
    using Core;
    [System.Serializable]
    public class RFC
    {
        public List<Hook> hooks = new List<Hook>();
        IShred shredder;
        public virtual void render(GameObject go)
        {
        } //applyshredder
    }

    public interface IShred
    {
        void read(Object o);
        void write(Object o);

    }
    public class ShredderInterface
    {
        public static IShred createShredder(Object o)
        {
            return null;
        }
    }
}