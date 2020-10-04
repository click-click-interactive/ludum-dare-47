using UnityEngine;
using UnityEditor;

namespace Assets.Scripts
{
    public interface ITriggerObject
    {
        void EnableAutomaticAction();
        void DisableAutomaticAction();
        void ManualAction();
    }
}