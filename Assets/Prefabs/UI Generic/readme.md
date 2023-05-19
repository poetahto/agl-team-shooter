# Poetools  UI
_A collection of Unity assets that aims to streamline 
the UI development process. To use it, simply
copy-and-paste this entire folder into your project._

### TODO
- Integrate more UI elements, add more common logic.

## Quick Start

Loading UI Prefabs from C# scripts, without worrying
about Resources.Load or Addressable calls.

```csharp
GameObject buttonPrefab = UIPrefabs.Instance.Button();
GameObject sliderPrefab = UIPrefabs.Instance.Slider();
// ... and so on, for each UI element you might need.
```

Quickly binding logic to UGUI elements.

```csharp
IMenuBuilder menu;
menu = new ExistingMenuBuilder(); // Searches for children by name.
menu = new AutoMenuBuilder(); // Creates new children.

ISerializer customSerializer = // Your custom solution here.

// If no serializer provided, defaults to PlayerPrefs.
menu.Register("fov", new Slider(value => Debug.Log($"FOV is {value}"))
    .Register("sensitivity", new Slider().WithLabel("Mouse Sensitivity"))
    .Register("difficulty", new Dropdown(customSerializer)
        .AddOption("Easy", () => Debug.Log("Set to easy")
        .AddOption("Hard", () => Debug.Log("Set to hard"))
    .Build(gameObject);
```

Reusing menu logic that is very common in games.

```csharp
new AutoMenuBuilder()
    .Register("resolution", CommonMenuItems.ResolutionDropdown())
    .Register("quality", CommonMenuItems.QualityDropdown())
    .Register("fullscreen", CommonMenuItems.FullscreenToggle())
    .Register("quit", CommonMenuItems.QuitGameButton())
    .Build(videoSettings)
```


## Contents 

### Root Folder
In this root folder, you'll find a bunch of prefabs
for many different types of UI elements. It can be 
helpful to use these so you can quickly propagate
UI changes across the entire project.

UIPrefabsAddressable and Resources/UIPrefabs
allow any of these prefabs to be easily loaded via C#
scripting, without the need for direct Resource or
Addressable calls.

### Resources Folder
This folder is used as a fallback, in-case Addressables 
are not being used.

### Scripts Folder
This is where all of the C# scripts for the UI systems
can be found. Each script should have its public API
fully documented. 

## Customization
This package was designed to be easy to extend if you
find that some elements are not included. 

### Custom Prefabs
To use custom prefabs, you can replace the references
in the UIPrefabs asset: it is the single point of
communication between the C# and the prefab world.

However, the default menu items do make assumptions
about what components might be found on the prefabs,
so it might be easier to create variants of the existing
ones and tweak them from there. 

### Custom Menu Items
To fully integrate a custom UI element into the project, 
this would be the best way:

1) Add it to UI Prefabs
2) Create a new class that implements _IMenuItem_.
   1) The _Generate()_ method usually loads in the prefab
from UIPrefabs.
   2) The _BindTo()_ method should apply serialization and
begin listening to events.
   3) Pass in the custom state you need through the 
constructor.

Adding extension methods may be useful for certain types
of menu items; for example, the _Label_ item uses a
_WithLabel()_ extension on all _IMenuItem_'s to make
things much more readable.

### Custom Serializers
To use a custom serializer over the default Unity
PlayerPrefs, you just need to implement the ISerializer
interface:

1) The _Write()_ method should store some arbitrary data,
keyed with an id.
2) The _Read()_ method should look up some arbitrary data,
keyed with an id.

In order to ensure your serializer works with the default
menu items, it should at least be able to serialize:
- bool
- float
- string
- int
