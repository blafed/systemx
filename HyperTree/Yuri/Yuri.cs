using System.Collections.Generic;
namespace Yuri
{
    [System.Serializable]
    public class Arsenal
    {
        public Arsenal(FactContainer fc, params FiberType[] fiberTypes)
        {
            factContainer = fc;
            this.fiberTypes = new List<FiberType>(fiberTypes);
        }
        public float[] lastFibers;
        public FactContainer factContainer;
        public List<FiberType> fiberTypes = new List<FiberType>();
        public virtual void step(float[] fibers)
        {
            for (int i = 0; i < fibers.Length; i++)
            {
                var t = fiberTypes[i];
                var f = fibers[i];

                if (t == FiberType.MOTION)
                {

                }
            }
            lastFibers = fibers;
        }


    }
    public enum FiberType
    {
        normalSense,
        motive,
        happy,
        MOTION,

    }

    public interface ISentence
    {
        int count { get; }
        void split(IList<ISentence> sentences, int startIndex);
    }

    public enum FactType
    {
        predict,
        response,
        happyRule,

    }
    public class FactContainer
    {
        public virtual Fact findNext()
        {
            return null;
        }
    }

    public class Fact
    {

    }

    public class Symbol
    {
        public List<Symbol> children;
        public virtual float value { get; set; }
        public SymbolOp op;
    }
    public enum VariableType
    {

    }
    public enum SymbolOp
    {
        none,
        negative,
        sum,
        mult,
        inverse,
        div,
        sub,


    }
}