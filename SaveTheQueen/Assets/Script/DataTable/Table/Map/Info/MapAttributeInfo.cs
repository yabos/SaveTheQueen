using System.Collections.Generic;

namespace Aniz.Data.Map.Info
{
    public class MapAttributeInfo
    {
        int dwWidth;
        List<ushort> lstAttribute;		// 속성
        List<ushort> lstMaterial;		// 재질
        List<byte> lstDirection;		// 셀방향

        public ushort GetAttribute(int i_dwX, int i_dwY)
        {
            return lstAttribute[ i_dwX + i_dwY * dwWidth ]; 
        }

        public ushort GetMaterial(int i_dwX, int i_dwY)
        {
            return lstMaterial[i_dwX + i_dwY * dwWidth];
        }

        public byte GetDirection(int i_dwX, int i_dwY)
        {
            return lstDirection[i_dwX + i_dwY * dwWidth];
        }
        
    }
}