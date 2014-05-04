using Eppy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.AI
{
    public class PriorityQueue<T, K> where K : IComparable<K>
    {
        private List<Tuple<T, K>> data;

        public PriorityQueue()
        {
            this.data = new List<Tuple<T, K>>();
        }

        public void Enqueue(T item, K priority)
        {
            data.Add(new Tuple<T, K>(item, priority));
            int ci = data.Count - 1; // child index; start at end
            while (ci > 0)
            {
                int pi = (ci - 1) / 2; // parent index
                if (data[ci].Item2.CompareTo(data[pi].Item2) >= 0) break; // child item is larger than (or equal) parent so we're done
                var tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
                ci = pi;
            }
        }

        public Tuple<T, K> Dequeue()
        {
            // assumes pq is not empty; up to calling code
            int li = data.Count - 1; // last index (before removal)
            Tuple<T, K> frontItem = data[0];   // fetch the front
            data[0] = data[li];
            data.RemoveAt(li);

            --li; // last index (after removal)
            int pi = 0; // parent index. start at front of pq
            while (true)
            {
                int ci = pi * 2 + 1; // left child index of parent
                if (ci > li) break;  // no children so done
                int rc = ci + 1;     // right child
                if (rc <= li && data[rc].Item2.CompareTo(data[ci].Item2) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                    ci = rc;
                if (data[pi].Item2.CompareTo(data[ci].Item2) <= 0) break; // parent is smaller than (or equal to) smallest child so done
                var tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp; // swap parent and child
                pi = ci;
            }
            return frontItem;
        }

        public T Peek()
        {
            T frontItem = data[0].Item1;
            return frontItem;
        }

        public int Count()
        {
            return data.Count;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < data.Count; ++i)
                s += data[i].ToString() + " ";
            s += "count = " + data.Count;
            return s;
        }

        public bool IsConsistent()
        {
            // is the heap property true for all data?
            if (data.Count == 0) return true;
            int li = data.Count - 1; // last index
            for (int pi = 0; pi < data.Count; ++pi) // each parent index
            {
                int lci = 2 * pi + 1; // left child index
                int rci = 2 * pi + 2; // right child index

                if (lci <= li && data[pi].Item2.CompareTo(data[lci].Item2) > 0) return false; // if lc exists and it's greater than parent then bad.
                if (rci <= li && data[pi].Item2.CompareTo(data[rci].Item2) > 0) return false; // check the right child too.
            }
            return true; // passed all checks
        } // IsConsistent
    } // PriorityQueue
}
