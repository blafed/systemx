using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace Nord.Rex
{
    [System.Serializable]
    public class Cell
    {
        [Range(0, 1)]
        public float stored;
        public List<Connection> cons = new List<Connection>();
        public Tissue tissue;

        ActivationPath createNPath(ActivationPath parent) => new ActivationPath { parent = parent, cell = this };
        public float totalWeight()
        {
            float f = 0;
            cons.ForEach(x => f += x.weight);
            return f;
        }

        public float activate(float feed, ActivationPath path)
        {
            var total = stored + feed;
            var left = total;
            ActivationPath nPath = null;
            if (cons.Count > 0 && total > 0)
            {
                nPath = createNPath(path);
            }
            for (int i = 0; i < cons.Count; i++)
            {
                var x = cons[i];
                var m = x.weight * total;
                x.other.activate(m, nPath);
                left -= m;
            }
            if (left > 1)
            {
                if (path == null)
                {
                    nPath = createNPath(path);
                    var c = tissue.addCell();
                    this.cons.Add(new Connection { other = c, weight = Random.value });
                    var otherStored = c.activate(left, nPath);
                    left -= otherStored;
                }
                else
                {
                    var parentCell = path.cell;
                }
            }
            stored = left;

            return left;
        }
    }
    public class ActivationPath
    {
        public ActivationPath parent;
        public Cell cell;
    }
    [System.Serializable]
    public class Connection
    {
        public Cell other;
        [Range(0, 1)]
        public float weight;
    }
    [System.Serializable]
    public class Tissue
    {
        public static Tissue activator;
        List<int> inputCells = new List<int>();
        List<int> outputCells = new List<int>();
        List<Cell> cells = new List<Cell>();

        public void cycle(float[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                var inp = inputs[i];
                var cellIndex = inputCells[i];
                var cell = cells[cellIndex];

            }
        }
        public Cell addCell()
        {
            var c = new Cell();
            c.tissue = this;
            cells.checkCapacityAdd(c, 10);
            return c;
        }
    }
}
namespace Nord.Rex2
{
    // public abstract class Cell{
    //     public Cell container;
    //     List<Link> links = new List<Link>();
    //     int linkCount => links.Count;
    //     Link getLink(int index) => links[index];
    //     public virtual Link getLink(Cell other) => links.Find(x=>x.cell == other);
    //     public abstract Link setLink(Cell other, float weight);
    // }
    public abstract class Cell
    {
        public abstract Cell parent { get; }
        public abstract Link getLink(Cell other);
        public abstract Link setLink(Cell other, float weight);
        public abstract Link getLink(int index);
        public abstract int linkCount { get; }
        public abstract Cell getNearest(Cell targetCell, int index);
        public abstract Cell createNewCell();
        public virtual float stored { get; set; }
        public abstract Cell inputCell { get; }
        public abstract Cell outputCell { get; }
        ActivationPath getNPath(ActivationPath path) => new ActivationPath
        {
            parent = path,
            cell = this,
        };
        public virtual float activate(float feed, ActivationPath path)
        {
            if (inputCell != null)
            {
                return inputCell.activate(feed, path);
            }
            var total = feed + stored;
            total = total.limitMin(0);
            var left = total;
            for (int i = 0; i < this.linkCount; i++)
            {
                if (left.approx(0))
                    break;
                var l = getLink(i);
                var w = l.weight * total;
                w = Mathf.Min(w, left);
                left -= w;
                activate(w, getNPath(path));
            }
            this.stored = left;
            return left;
        }

    }
    public class ActivationPath
    {
        public ActivationPath parent;
        public Cell cell;

        public override bool Equals(object obj)
        {
            var ap = obj as ActivationPath;
            return Equals(parent, ap) && Equals(cell, parent);
        }
        public override int GetHashCode()
        {
            var p = parent == null ? 0 : parent.GetHashCode();
            var c = cell == null ? 0 : cell.GetHashCode();
            return new Vector2Int(p, c).GetHashCode();
        }

    }
    public class Link
    {
        public Cell cell;
        public float weight;
    }
}