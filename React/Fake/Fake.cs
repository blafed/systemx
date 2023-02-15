using Shredder;
using UnityEngine;

namespace React.Fake
{
    public class FakeScript : ShredderScript
    {
        public ShredderCopy copy = new ShredderCopy();
        

        public virtual void copyThisGO()
        {
            copy.read(gameObject);
        }
        public virtual void render()
        {
            copy.write(gameObject);
        }
    }

    public class FakeFactory : ShredderFactory
    {
        public static FakeFactory factory = new FakeFactory();
        public override IDependencyRef getDependencyRef(Object o)
        {
            var f = object2Depend(o);
            if (!f.container)
            {
                
            }

            return f;
        }

        static FakeDepend object2Depend(Object o)
        {
            return default;
        }

        public static Object fakeDependGet(FakeDepend f)
        {
            return null;
        }
        public static void fakeDependSet(FakeDepend f, Object o){}
        
    }

    public enum ObjectsContainerType
    {
        currentScene,
        global,
    }
    
    [System.Serializable]
    public struct FakeDepend : IDependencyRef
    {
        public FakeScript container;
        public int index;
        public TitleName title;
        public Object obj
        {
            get => FakeFactory.fakeDependGet(this);
            set => FakeFactory.fakeDependSet(this, value);
        }
    }
}