using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Bindings.Demo
{
    public class DemoPlayerController : MonoBehaviour
    {
        void OnEnable()
        {
            KnotBindings.Global.RegisterPropertyChanged<float>("PlayerPosition", OnPlayerPositionChanged);
        }

        void OnDisable()
        {
            KnotBindings.Global.UnRegisterPropertyChanged<float>("PlayerPosition", OnPlayerPositionChanged);
        }

        void OnPlayerPositionChanged(float oldvalue, float newvalue, object setter)
        {
            SetPlayerPosition(newvalue);
        }

        void Update()
        {
            var input = Input.GetAxis("Horizontal");
            if (!Mathf.Approximately(0, input))
                KnotBindings.Global.Set("PlayerPosition", transform.position.x + input);
        }

        void SetPlayerPosition(float pos)
        {
            pos = Mathf.Clamp(pos, -5, 5);
            transform.position = new Vector3(pos, transform.position.y, transform.position.z);
        }
    }
}
