using Lib.InputButton;
using Aniz.Contents.Entity;

using UnityEngine;

namespace Aniz.InputButton.Controller
{
    public class PlayerButtonController : ButtonController
    {
        private const float m_attackInputDelta = 0.3f;

        private IInput m_maininput;
        private IEntity m_entity;

        protected uint m_prevDirection;
        protected byte m_prevDir;

        protected uint m_prevAttack;

        protected uint m_preInput;

        protected bool m_attackInputReady;
        protected float m_attackInputTime;

        protected float m_cameraAngle = float.MaxValue;

        public PlayerButtonController()
        {
            m_maininput = Global.InputMgr.System.MainInput;
            m_attackInputReady = true;
        }

        public void InitActor(IEntity entity, long NetID)
        {
            playerNetID = NetID;
            m_entity = entity;
        }

        public override void OnFixedUpdate(float deltaTime)
        {
            if (m_maininput == null)
                return;

            if (m_attackInputReady == false)
            {
                m_attackInputTime -= deltaTime;
                if (m_attackInputTime < 0f)
                    m_attackInputReady = true;
            }

            SetInput(m_maininput.GetInput());
            SetAxisInput(m_maininput.GetAxisInput());
            base.OnFixedUpdate(deltaTime);

            if (m_entity == null)
                return;
            if (m_entity.IsDeath)
                return;

            CheckMoveEvent();
        }

        private void CheckMoveEvent()
        {
            bool forceUpdateMove = false;

            var cameraAngle = Global.CameraMgr.MainCamera.transform.rotation.eulerAngles;
            if (m_cameraAngle != cameraAngle.y)
            {
                forceUpdateMove = true;
                m_cameraAngle = cameraAngle.y;
            }

            var alignToCameraDir = Quaternion.Euler(0, m_cameraAngle, 0) * new Vector3(m_axisInput.x, 0, m_axisInput.y);
            var moveDir = new Vector2(alignToCameraDir.x, alignToCameraDir.z);
            var actorTransform = m_entity.GetOwnerTransform();

            if (m_attackInputReady == false)
            {
                m_input &= InputSystem.NAVIGATION_BUTTONS;
            }

            if ((m_input & InputSystem.ATTACK_BUTTONS) != 0)
            {
                m_attackInputReady = false;
                m_attackInputTime = m_attackInputDelta;
            }
            /*
            // #SYNC_TEST 키 입력에 상관 없이 매 frame 입력을 보내야 함
            if (Global.NetMgr.IsUnityBattleServerConnection())
            {
                var actor = m_player.Main;
                actor.LastSentIndex = ++actor.NextIndexOfInputEventToSend; // start from 1

                NetBattleHandler.SendInput(m_player.NetID, m_input, moveDir,
                    actorTransform.position, actorTransform.forward, actor.StepCnt, actor.NextIndexOfInputEventToSend);
            }
            else
            {
                // #ilkyo : need optimization
                // #NOTE : 공격 중 이동 키를 계속 입력하면 공격이 끝났을 때 이동이 되어야 한다.
                // 하지만 이 때 preInput과 input이 같기 때문에 아래 루틴이 실행되지 않는다.
                if ((m_input != 0 && forceUpdateMove) || // 카메라 엥글이 바뀌어서 진행 방향이 바뀜.
                    (m_input != 0 && (m_preInput != m_input)) || // 버튼인 눌렸는데 이전 프레임 버튼입력과 다르면
                (m_input == 0 && m_preInput != 0))    // 버튼이 안눌렸는데 이전 프레임에 버튼이 눌렸으면 멈췄다는 패킷 보냄
                {
                    //if (m_attackInputReady == false)
                    //    return;

                    //curInput = m_input & InputSystem.ATTACK_BUTTONS;
                    //if ((m_input & InputSystem.ATTACK_BUTTONS) != 0)
                    //{
                    //    NetBattleHandler.SendInput(m_player.NetID, curInput, m_axisInput,
                    //        m_player.Main.transform.position, m_player.Main.transform.forward, TimeManager.serverTick, ++m_lastSentInputPacketIndex, true);

                    //    m_attackInputReady = false;
                    //}

                    LocalActionHandler.SendInput(m_player.NetID, m_input, moveDir, actorTransform.position, actorTransform.forward, TimeManager.serverTick);
                }
            }
            */
            //if ((m_input != 0 && (m_preInput != m_input)) || // 버튼인 눌렸는데 이전 프레임 버튼입력과 다르면
            //    (m_input == 0 && m_preInput != 0))    // 버튼이 안눌렸는데 이전 프레임에 버튼이 눌렸으면
            //{
            //    m_attackInputTime = m_attackInputDelta;
            //}

            m_preInput = m_input;
        }
    }
}