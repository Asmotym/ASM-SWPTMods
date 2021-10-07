# AedenthornSkillFrameworkPlusPlus

This mod provides methods to create and manage skills in the game for your mods.

This extends base code of the `SkillFramework` mod from [Aedenthorn](https://github.com/aedenthorn),
by reformatting the code and providing more keys to work with skill easily in your plugin.

## Usage

* Add the `.dll` to your project
* Import it in your code (`using AedenthornSkillFrameworkPlusPlus;`)

You can then use the following method to create a skill:

```C#
SkillAPI.AddSkill(skillId, skillName, skillDescription, skillCategory, skillIcon, maxPoints.Value, reqLevel.Value, false);
```

Adding a skill **should** be done in the `public void Start()` method of your BepInEx plugin.

Here's the ``AddSkill`` method signature:

```C#
public static void AddSkill(
            string id,
            List<string> name,
            List<string> description,
            int category,
            Texture2D icon,
            int maxPoints,
            int reqLevel,
            bool isActiveSkill
            );
```

You can take a look at my **MoreSkills** plugin which uses this to create skills.

## Credits

All credits goes to [Aedenthorn](https://github.com/aedenthorn) for providing the base code.
