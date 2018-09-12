using Aniz.NodeGraph.Level.Group.Info;
using Aniz.Event;
using Aniz.InputButton.Controller;
using Lib.Battle;
using Lib.Event;
using Lib.InputButton;
using UnityEngine;

namespace Aniz.Contents.Entity
{
    public class EntityMessageListener : MessageListener
    {
        private readonly IEntity m_entity;

        public EntityMessageListener(IEntity entity)
        {
            m_entity = entity;

            //AddHandler((uint)eMessage.InputCommand, OnInputCommand);
            AddHandler((uint)eMessage.Action, OnAction);
            AddHandler((uint)eMessage.Hit, OnHit);
            AddHandler((uint)eMessage.Dead, OnDead);
            //AddHandler((uint)eMessage.SyncEvent, OnSyncEvent);
        }

        private bool OnInputCommand(IMessage message)
        {
            //if (Global.SimulationMgr.IsReplay)
            //    return true;

            //InputCommandMsg commandMsg = message as InputCommandMsg;
            //if ((commandMsg.SyncInput._input & InputSystem.ATTACK_BUTTONS) != 0)
            //{
            //    Attack(m_player, commandMsg.SyncInput._input, new Vector3(commandMsg.SyncInput._axisInputX, 0f, commandMsg.SyncInput._axisInputY));
            //}
            //else if ((commandMsg.SyncInput._input & InputSystem.NAVIGATION_BUTTONS) != 0)
            //{
            //    if (commandMsg.SyncInput._input > 0)
            //    {
            //        StateGenerator.BuildDirMove(m_player.Main, FsmEventString.WALK, new Vector3(commandMsg.SyncInput._axisInputX, 0f, commandMsg.SyncInput._axisInputY));
            //    }
            //}
            //else
            //{
            //    StateGenerator.BuildWait(m_player.Main, FsmEventString.IDLE);
            //}

            return true;
        }

        private bool OnAction(IMessage pMessage)
        {
            //if (m_player.IsUser && Global.SimulationMgr.IsReplay == false)
            //    return true;

            //ActionMsg actionMsg = pMessage as ActionMsg;

            //StateSync stateSync;
            //StateGenerator.ConvertStateSync(actionMsg.SyncState, out stateSync);
            //StateGenerator.ActionPlay(m_player.Main, stateSync);
            return true;
        }

        private bool OnDead(IMessage pMessage)
        {
            DeadMsg deadMsg = pMessage as DeadMsg;

            StateSync stateSync;
            //StateGenerator.ConvertStateSync(deadMsg.SyncState, out stateSync);

            //IPlayer attacker = Global.GameMgr.Repository.GetPlayer(deadMsg.AttackerId);

            //SkillEvent skillEvent = TableSkill.GetSkillEvent(deadMsg.SyncState._argument);
            //DamageInfo damageinfo = new DamageInfo(attacker, m_player, skillEvent);
            //damageinfo.Damage = deadMsg.Damage;
            //damageinfo.HPResult = 0;

            //stateSync.Damage = damageinfo;
            //stateSync.ActionReset = true;

            //StateGenerator.ActionPlay(m_player.Main, stateSync);
            return true;
        }

        private bool OnHit(IMessage message)
        {
            HitMsg hitMsg = message as HitMsg;

            //IPlayer attacker = Global.GameMgr.Repository.GetPlayer(hitMsg.AttackerId);

            //StateSync stateSync;
            //StateGenerator.ConvertStateSync(hitMsg.SyncState, out stateSync);

            //SkillEvent skillEvent = TableSkill.GetSkillEvent(hitMsg.SkillEventID);
            //DamageInfo damageinfo = new DamageInfo(attacker, m_player, skillEvent);
            //damageinfo.Damage = hitMsg.Damage;
            //damageinfo.HPResult = hitMsg.HPResult;

            //stateSync.Damage = damageinfo;
            //stateSync.ActionReset = true;

            //StateGenerator.ActionPlay(m_player.Main, stateSync);

            return true;
        }

        private void Attack(IEntity actor, uint input, Vector3 axis)
        {
            //if (ButtonController.IsButton(input, ePlayerButton.FIRE1))
            //{
            //    actor.UseSkill(ePlayerButtonEnum.FIRE1, axis);
            //}
            //if (ButtonController.IsButton(input, ePlayerButton.FIRE2))
            //{
            //    actor.UseSkill(ePlayerButtonEnum.FIRE2, axis);
            //}
            //if (ButtonController.IsButton(input, ePlayerButton.FIRE3))
            //{
            //    actor.UseSkill(ePlayerButtonEnum.FIRE3, axis);
            //}
            //if (ButtonController.IsButton(input, ePlayerButton.JUMP))
            //{
            //    //  #ilkyo : testcode
            //    //actor.TakeDamage(new DamageInfo
            //    //{
            //    //    Attacker = null,
            //    //    Defender = actor,
            //    //    Damage = 10,
            //    //    HPResult = 90,
            //    //    SkillEvent = TableSkill.GetSkillEvent(50)
            //    //});
            //    actor.Main.EmergencyEvent("Stage_Start", 1.5f, null);
            //}
            //if (ButtonController.IsButton(input, ePlayerButton.ABILITY1))
            //{
            //    actor.UseSkill(ePlayerButtonEnum.ABILITY1, axis);
            //}
            //if (ButtonController.IsButton(input, ePlayerButton.ABILITY2))
            //{
            //    actor.UseSkill(ePlayerButtonEnum.ABILITY2, axis);
            //}
            //if (ButtonController.IsButton(input, ePlayerButton.ABILITY3))
            //{
            //    actor.UseSkill(ePlayerButtonEnum.ABILITY3, axis);
            //}
            //if (ButtonController.IsButton(input, ePlayerButton.CHAIN1))
            //{
            //    actor.UseSkill(ePlayerButtonEnum.CHAIN1, axis);
            //}
            //if (ButtonController.IsButton(input, ePlayerButton.CHAIN2))
            //{
            //    actor.UseSkill(ePlayerButtonEnum.CHAIN2, axis);
            //}
            //if (ButtonController.IsButton(input, ePlayerButton.CHAIN3))
            //{
            //    actor.UseSkill(ePlayerButtonEnum.CHAIN3, axis);
            //}

        }

        private bool OnSyncEvent(IMessage message)
        {
            //var syncEvent = ((SyncEventMsg)message).syncEvent;
            //m_player.Main.OnSyncEvent(syncEvent);

            return true;
        }
    }
}