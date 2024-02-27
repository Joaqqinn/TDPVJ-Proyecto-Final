using Bardent.Weapons.Components;
using UnityEngine;

namespace Bardent.Weapons.Components
{
    public class LineTrayectoryData : ComponentData<AttackTrayectory>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(LinePredictsTrayectory);
        }
    }
}