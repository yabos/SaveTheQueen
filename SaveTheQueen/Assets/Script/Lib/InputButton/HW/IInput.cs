using System;
using UnityEngine;

namespace Lib.InputButton
{
    public interface IInput
    {
        // controller index by type
        void SetInputIndex(int index);
        void OnUpdate();
        Vector2 GetAxisInput();
        uint GetInput();
        uint GetInputDown();
        uint GetInputUp();
        bool GetButton(ePlayerButton button);
        bool GetButtonDown(ePlayerButton button);
        bool GetButtonUp(ePlayerButton button);

        void SetAxisInput(Vector2 axis);
        void SetButtonDown(ePlayerButton button);
        void SetButtonUp(ePlayerButton button);

        void ClearInput();
    }
}