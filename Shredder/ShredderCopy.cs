using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

namespace Shredder
{
    public class ShredderCopy
    {
        private static ListX currentList = new ListX();
        class ListX
        {
            private GameObject go;
            private Component[] comps;

            public void set(GameObject go, Component[] comps)
            {
                this.comps = comps;
                this.go = go;
            }

            public Object this[int x]
            {
                get => x == 0 ? go : comps[x - 1];
            }

            public int Count
            {
                get
                {
                    var i = 0;
                    if (go)
                        i = 1;
                    if (comps != null)
                        i += comps.Length;
                    return i;
                }
            }
        }
        public int index { get; set; }
        // public Title mainTitle = Title.getTitle(TitleName.GameObject);
        public List<Title> titles = new List<Title>();

        public List<ShredderCopy> children = new List<ShredderCopy>();

        // public Dictionary<string, int> keyValue = new Dictionary<string, int>();

        public void read(GameObject go)
        {
            var mainTitle = Title.getTitle(TitleName.GameObject);
            //mainTitle.read(go);
            titles.Clear();
            children.Clear();

            var components = go.GetComponents(typeof(Component));

            titles.Add(mainTitle);
            currentList.set(go, components);


            // foreach (var comp in components)
            // {
            //     var t = Title.getTitle(comp);
            //     if (t == null)
            //     {
            //         Debug.Log(comp + " has no title");
            //         continue;
            //
            //     }
            //
            //     t.index = i;
            //     t.read(comp);
            //     titles.addCheckCapacity(t, components.Length);
            //     i++;
            // }

            for (int i = 0; i < currentList.Count; i++)
            {
                var c = currentList[i];
                var t = Title.getTitle(c);

                if (t == null)
                {
                    Debug.Log(c + " has no title", c);
                    continue;
                }

                t.index = i;
                t.read(c);
                titles.checkCapacityAdd(t, currentList.Count);
            }


            int j = 0;
            foreach (Transform x in go.transform)
            {
                var sc = new ShredderCopy();
                sc.index = j;
                sc.read(x.gameObject);
                children.checkCapacityAdd(sc, go.transform.childCount);
                j++;
            }
        }
        public void write(GameObject go)
        {

            // mainTitle.write(go);
            var comps = go.GetComponents<Component>();
            currentList.set(go, comps);
            for (int i = 0; i < currentList.Count; i++)
            {
                Object c;
                TitleName compTitleName;
                var t = titles[i];

                if (currentList.Count <= i)
                {
                    c = t.create(go);
                    //go.AddComponent(t.type);
                    Debug.Log(t.name);
                    compTitleName = t.name;
                }
                else
                {
                    c = currentList[i];
                    compTitleName = Title.getTitleName(c);
                }

                if (compTitleName != t.name)
                {
                    Object.Destroy(c);
                    c = go.AddComponent(t.type);
                }

                t.write(c);
            }

            for (int i = titles.Count; i < currentList.Count; i++)
            {
                Object.Destroy(currentList[i]);
            }

            for (int i = 0; i < children.Count; i++)
            {
                var c = children[i];
                c.write(go.transform.GetChild(i).gameObject);
            }

            for (int i = children.Count; i < go.transform.childCount; i++)
            {
                var c = go.transform.GetChild(i).gameObject;
                Object.Destroy(c);
            }
        }
    }
}
// namespace Shredder
// {
//     using Titles;
//     public class ShredderCopy
//     {
//         private Title go = Title.getTitle(TitleName.GameObject);
//         private List<Title> titles = new List<Title>();
//         public void write(GameObject o)
//         {
//             go.write(o);
//             var comps = o.GetComponents<Component>();
//             for (int i = 0; i < titles.Count; i++)
//             {
//                 var t = titles[i];
//                 t.write(comps[i]);
//             }
//         }
//
//         public void read(GameObject o)
//         {
//             go.read(o);
//             var comps = o.GetComponents<Component>();
//             for (int i = 0; i < comps.Length; i++)
//             {
//                 TitleName n = Title.getTitleName(comps[i]);
//                 var x = Title.getTitle(n);
//                 Debug.Log(n);
//                 Debug.Log(x);
//                 var t = Title.getTitle(comps[i]);
//                 t.index = i;
//                 titles.addCheckCapacity(t, comps.Length);
//                 t.read(comps[i]);
//             }
//         }
//     }
// }