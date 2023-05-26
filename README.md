![Unity version](https://img.shields.io/badge/Unity-2020.3%2B-blue)
![Dependencies](https://img.shields.io/badge/dependencies-none-green)
![Platforms](https://img.shields.io/badge/platforms-all-blue)

![ezgif-4-cafed11d6c](https://github.com/V0odo0/KNOT-Bindings/assets/10213769/cc5f3569-40b4-4476-a045-ec7f32767d64)
## Installation

Install via Package Manager

![Screenshot_3](https://user-images.githubusercontent.com/10213769/162617479-51c3d2d5-8573-44a2-bc56-8c68d09183f1.png)

```
https://github.com/V0odo0/KNOT-Bindings.git
```

*or*

Add dependency to your /YourProjectName/Packages/manifest.json

```
"com.knot.bindings": "https://github.com/V0odo0/KNOT-Bindings.git",
```

## Usage example #1: Property override

```C#
//Global container that exists till domain reload / application shut down
var bindingsContainer = KnotBindings.Global;

//Add property with default value
bindingsContainer.Set("EnablePlayerControls", true);

//Read property value
Debug.Log(bindingsContainer.Get("EnablePlayerControls", false)); //true

//Override property value by setting higher priority 
bindingsContainer.Set("EnablePlayerControls", false, 100);

//Read property value after override
Debug.Log(bindingsContainer.Get("EnablePlayerControls", false)); //false

//Delete previously set override value
bindingsContainer.Delete<bool>("EnablePlayerControls", 100);

//Read property value after override
Debug.Log(bindingsContainer.Get("EnablePlayerControls", true)); //true
```

## Usage example #2: Two-Way Databinding
```C#
//Class that changes CameraZoom property by player input and applies it to Camera
public class CameraController : MonoBehaviour
{
    public float CameraZoom;

    void OnEnable()
    {
        //Register property changed callback
        KnotBindings.Global.RegisterPropertyChanged<float>("CameraZoom", OnCameraZoomPropertyChanged);
    }

    void OnDisable()
    {
        //UnRegister callbacks
        KnotBindings.Global.UnRegisterPropertyChanged<float>("CameraZoom", OnCameraZoomPropertyChanged);
    }

    void OnCameraZoomPropertyChanged(float oldvalue, float newvalue, object setter)
    {
        //Return if the property has been changed by this CameraController
        if (setter as Object == this)
            return;

        //Apply zoom from binding property value
        CameraZoom = newvalue;
        ApplyCameraZoom();
    }

    void Update()
    {
        //Read mouse scroll input
        if (Input.GetMouseButtonDown(2))
        {
            //Modify CameraZoom
            CameraZoom += Input.mouseScrollDelta.y;
            CameraZoom = Mathf.Clamp(CameraZoom, 1, 10);

            //Set property value and provide the setter (this CameraController)
            KnotBindings.Global.Set("CameraZoom", CameraZoom, setter: this);

            ApplyCameraZoom();
        }
    }

    void ApplyCameraZoom()
    {
        var cameraLocalPos = Camera.main.transform.localPosition;
        cameraLocalPos.z = CameraZoom;
        Camera.main.transform.localPosition = cameraLocalPos;
    }
}
```

```C#
//Class that changes CameraZoom property by Slider
public class UICameraSettingsPanel : MonoBehaviour
{
    public Slider CameraZoomSlider;

    void Awake()
    {
        CameraZoomSlider.minValue = 1;
        CameraZoomSlider.maxValue = 10;
    }

    void OnEnable()
    {
        //Register slider value changed callback
        CameraZoomSlider.onValueChanged.AddListener(OnCameraZoomChanged);

        //Register property changed callback
        KnotBindings.Global.RegisterPropertyChanged<float>("CameraZoom", OnCameraZoomPropertyChanged);
    }

    void OnDisable()
    {
        //UnRegister callbacks
        CameraZoomSlider.onValueChanged.RemoveListener(OnCameraZoomChanged);
        KnotBindings.Global.UnRegisterPropertyChanged<float>("CameraZoom", OnCameraZoomPropertyChanged);
    }

    void OnCameraZoomChanged(float cameraZoom)
    {
        //Set property value and provide the setter (this UICameraController)
        KnotBindings.Global.Set("CameraZoom", cameraZoom, setter: this);
    }

    void OnCameraZoomPropertyChanged(float oldvalue, float newvalue, object setter)
    {
        //Return if the property has been changed by this UICameraController
        if (setter as Object == this)
            return;

        //Apply zoom from binding property value
        CameraZoomSlider.SetValueWithoutNotify(newvalue);
    }
}
```
