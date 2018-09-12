namespace Lib.UniBt
{
    public class RuntimeDecorator : IRuntime
    {
        public Node parent;
        public Decorator decorator;
        public System.Func<bool> decoratorFunc;
        //public float tick;
        public bool inversed;
        public bool activeSelf;
#if UNITY_EDITOR
        public bool closed;
#endif
        //public System.IDisposable subscription;

        public RuntimeDecorator(Node parent, Decorator decorator, bool inversed)
        {
            this.parent = parent;
            this.decorator = decorator;
            this.inversed = inversed;
            this.activeSelf = false;
        }

        public RuntimeDecorator(Node parent, Decorator decorator, bool inversed, bool activeSelf)
        {
            this.parent = parent;
            this.decorator = decorator;
            this.inversed = inversed;
            this.activeSelf = activeSelf;
        }

        public void OnStart()
        {
        }

        public void OnFinish()
        {
        }

        public eBTStatus OnUpdate()
        {
            return eBTStatus.Inactive;
        }
    }
}
