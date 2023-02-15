using UnityEngine;
using System.Collections.Generic;
namespace Shredder
{
    public class ShredderScript : MonoBehaviour, ITitleProvider
    {
        Queue<object> values => title.values;
        bool isWrite;
        bool isRead => !isWrite;

        CustomTitle title = new CustomTitle();
        private class CustomTitle : Title<ShredderScript>
        {
            public Queue<object> values = new Queue<object>();

            public override void write(ShredderScript x)
            {
                x.isWrite = true;
                x.cover();
            }
            public override void read(ShredderScript x)
            {
                x.isWrite = false;
                x.values.Clear();
                x.cover();
            }
        }
        public virtual void cover()
        {

        }
        public void append<T>(ref T x)
        {
            if (isRead)
                values.Enqueue(x);
            else x = (T)values.Dequeue();
        }

        public virtual Title getTitle()
        {
            return title;
        }

        internal virtual void Awake()
        {

        }
        internal virtual void Start()
        {
        }
        internal virtual void OnDestroy()
        {

        }

        public bool isRemoved { get; }
    }
}