using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lib.Battle
{
    public interface ICombatFinder
    {
        int GetCombatCount(eCombatType nodeType);
        bool GetCombats(eCombatType nodeType, ref List<ICombatOwner> lstActor);
        bool GetCombatTransforms(eCombatType nodeType, ref List<Transform> lstTransforms);

        /// <summary>
        /// 인자로 주어진 battleTeam과 다른 team의 actor 목록을 얻어옴. - 17.07.21. #jonghyuk
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="lstActor"></param>
        /// <param name="battleTeam"></param>
        /// <returns></returns>
        bool GetCombatsInOtherTeam(eCombatType nodeType, ref List<ICombatOwner> lstActor, int battleTeam);
    }
}
