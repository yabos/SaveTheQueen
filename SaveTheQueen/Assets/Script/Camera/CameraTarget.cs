using Aniz.NodeGraph.Level.Group;
using UnityEngine;

namespace Aniz.Cam
{
    public class CameraTarget
    {
        private Vector3 m_targetPos;
        private readonly CameraImpl m_cameraImpl;
        private ActorRoot m_targetActorRoot;

        public bool IsLocked
        {
            get { return m_targetActorRoot != null; }
        }

        public ActorRoot TargetActorRoot
        {
            get { return m_targetActorRoot; }
        }

        public Vector3 TargetPos
        {
            get
            {
                if (m_targetActorRoot != null)
                {
                    //m_targetPos = m_targetActor.Mesh.LookTransform.position;
                }
                return m_targetPos;
            }
        }



        public CameraTarget(CameraImpl impl)
        {
            m_cameraImpl = impl;
        }


        public void SetTarget(Vector3 targetPos)
        {
            m_targetPos = targetPos;
        }

        public void SetTarget(ActorRoot targetActorRoot)
        {
            m_targetActorRoot = targetActorRoot;
        }

    }
}