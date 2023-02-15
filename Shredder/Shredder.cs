using UnityEngine;
using System.Collections.Generic;

namespace Shredder
{
    using Titles;
    public class ShredderCopy1
    {
        public static Title createTitleFromType(Object obj)
        {
            if (obj is Rigidbody)
                return new Titles._Rigidbody();
            if (obj is Transform)
                return new _Transform();
            if (obj is GameObject)
                return new _GameObject();
            return null;
        }
        Transform transform => this.go.transform;
        GameObject go;
        int parentIndex;
        int index;

        List<ShredderCopy1> children = new List<ShredderCopy1>();
        List<Title> titles = new List<Title>();

        public void write(GameObject root, ShredderCopy1 copy)
        {
            var components = root.GetComponents(typeof(Component));

            titles[0].write(root);
            for (int i = 1; i < components.Length; i++)
            {
                var t = titles[i];
                t.write(components[i]);
            }

        }


        static ShredderCopy1 create(GameObject go, ShredderCopy1 root)
        {
            var sc = new ShredderCopy1();
            sc.go = go;
            var components = go.GetComponents(typeof(Component));
            //MAIN TITLE
            var mainTitle = createTitleFromType(go);
            mainTitle.read(go);
            sc.titles.checkCapacityAdd(mainTitle, components.Length);
            //OTHER TITLES
            foreach (var c in components)
            {
                var title = createTitleFromType(c);
                title.read(c);
                sc.titles.checkCapacityAdd(title, components.Length + 1);
            }
            //CHILDREN
            if (root == null)
                root = sc;
            foreach (Transform item in go.transform)
            {
                var child = create(item.gameObject, root);
                root.children.checkCapacityAdd(child, go.transform.childCount);
                child.index = root.children.Count;
                child.parentIndex = sc.index;
            }
            return sc;
        }

        public static ShredderCopy1 create(GameObject go)
        {
            return create(go, null);
        }

    }

}