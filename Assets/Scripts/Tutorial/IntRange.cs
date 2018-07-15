using System;
using UnityEngine;

namespace Tutorial
{
    // Serializable so it will show up in the inspector.
    [Serializable]
    public class IntRange
    {
        public Vector2 range;
        public int min { get; private set; }       // The minimum value in this range.
        public int max { get; private set; }      // The maximum value in this range.


        // Constructor to set the values.
        public IntRange(int min, int max)
        {
            range.x = min;
            range.y = max;
            this.min = min;
            this.max = max;
        }


        // Get a random value from the range.
        public int Random
        {
            get { return UnityEngine.Random.Range(min, max); }
        }
    }
}