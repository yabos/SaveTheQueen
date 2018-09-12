using UnityEngine;

namespace Aniz.Graph
{
    // API base : Unity 5.4
    public class RockRandom
    {
        Random.State state;

        bool UnsafeMode { get; set; }

        void SaveState()
        {
            if (!this.UnsafeMode)
                this.state = Random.state;
        }

        void LoadState()
        {
            if (!this.UnsafeMode)
                Random.state = this.state;
        }

        //
        // public
        //

        public void EnterUnsafeMode()
        {
            Debug.Assert(!this.UnsafeMode);

            LoadState();
            this.UnsafeMode = true;
        }

        public void LeaveUnsafeMode()
        {
            Debug.Assert(this.UnsafeMode);

            this.UnsafeMode = false;
            SaveState();
        }

        public void InitState(int seed)
        {
            Random.InitState(seed);
            this.UnsafeMode = false;
            SaveState();
        }

        public RockRandom(int seed)
        {
            InitState(seed);
        }

        public Vector2 insideUnitCircle
        {
            get
            {
                LoadState();
                Vector2 retval = Random.insideUnitCircle;
                SaveState();
                return retval;
            }
        }

        public Vector3 insideUnitSphere
        {
            get
            {
                LoadState();
                Vector3 retval = Random.insideUnitSphere;
                SaveState();
                return retval;
            }
        }

        public Vector3 onUnitSphere
        {
            get
            {
                LoadState();
                Vector3 retval = Random.onUnitSphere;
                SaveState();
                return retval;
            }
        }

        public Quaternion rotation
        {
            get
            {
                LoadState();
                Quaternion retval = Random.rotation;
                SaveState();
                return retval;
            }
        }

        public Quaternion rotationUniform
        {
            get
            {
                LoadState();
                Quaternion retval = Random.rotationUniform;
                SaveState();
                return retval;
            }
        }

        // range : [0, 1]
        public float value
        {
            get
            {
                LoadState();
                float retval = Random.value;
                SaveState();
                return retval;
            }
        }

        public Color ColorHSV()
        {
            LoadState();
            Color retval = Random.ColorHSV();
            SaveState();
            return retval;
        }

        public Color ColorHSV(float hueMin, float hueMax)
        {
            LoadState();
            Color retval = Random.ColorHSV(hueMin, hueMax);
            SaveState();
            return retval;
        }

        public Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax)
        {
            LoadState();
            Color retval = Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax);
            SaveState();
            return retval;
        }

        public Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax)
        {
            LoadState();
            Color retval = Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax);
            SaveState();
            return retval;
        }

        public Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax)
        {
            LoadState();
            Color retval = Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, alphaMin, alphaMax);
            SaveState();
            return retval;
        }

        // range : [min, max)
        public float Range(float min, float max)
        {
            LoadState();
            float rand = Random.Range(min, max);
            SaveState();
            return rand;
        }

        // range : [min, max)
        public int Range(int min, int max)
        {
            LoadState();
            int rand = Random.Range(min, max);
            SaveState();
            return rand;
        }
    }
}
