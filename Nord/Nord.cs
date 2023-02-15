namespace Nord.Core
{
    public interface IValue
    {
        float value { get; }
    }
    public interface IRelation
    {
        float a { get; }
        float b { get; }
        IRelation other { get; }
    }
    public interface IMemory
    {
        void addRelation(IRelation r, float impact);
        IRelation getRelation(int i);
        IRelation highest();
    }

    public abstract class RelationFactory
    {
        public abstract IRelation create(float a, float b);
        public abstract IRelation create(IRelation a, IRelation b);
    }
    public struct RelationType
    {
        public const long x = 1, y = 2;
        public const long xy = x | y;
    }
    public class Mandatory
    {
        public RelationFactory factory;
        public IMemory memory;
        public void cycle()
        {
            var r = memory.getRelation(0);
            if (r.other != null)
            {
            }
        }
    }
}