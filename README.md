# Singleton Scriptable Objects

Enables accessing singleton `ScriptableObject` assets statically:
```cs
[CreateAssetMenu]
public class GameSettings : SingletonScriptableObject<GameSettings>
{
    public GameObject playerPrefab;
}

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        GameSettings settings = GameSettings.Singleton;
        Instantiate(settings.playerPrefab);
    }
}
```

### Installation

Use the following URL in Unity's package manager:
```
https://github.com/popcron/singleton-scriptable-objects.git
```

### Implementation

The approach involved ensures that scriptable objects inheriting from `SingletonScriptableObject<T>`
are always present in preloaded objects. This allows them to be located anywhere in the assets of the project.

### Contributing and design

This package is a boiled down version of the patterns that I used in past years. It's goal is to enable reliable
access of scriptable objects, without hacky workarounds or relying on "Resources" folder, and accessible to non programmers.

Contributions that fit this are welcome.
