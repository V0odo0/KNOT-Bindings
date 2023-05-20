using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Knot.Bindings.Demo
{
    public class DemoUIManager : MonoBehaviour
    {
        public Slider PlayerPositionSlider;
        

        void OnEnable()
        {
            KnotBindings.Global.RegisterPropertyChanged<float>("PlayerPosition", OnPlayerPositionChanged);
            PlayerPositionSlider.onValueChanged.AddListener(OnPlayerPositionChangedBySlider);
        }

        void OnDisable()
        {
            KnotBindings.Global.UnRegisterPropertyChanged<float>("PlayerPosition", OnPlayerPositionChanged);
            PlayerPositionSlider.onValueChanged.RemoveListener(OnPlayerPositionChangedBySlider);
        }

        void OnPlayerPositionChanged(float oldvalue, float newvalue, object setter)
        {
            if (setter == this)
                return;

            PlayerPositionSlider.SetValueWithoutNotify(newvalue);
        }

        void OnPlayerPositionChangedBySlider(float arg0)
        {
            KnotBindings.Global.Set("PlayerPosition", arg0, setter:this);
        }
    }
}
