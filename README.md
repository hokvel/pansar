Pansar
======

Pansar is a mod enabling customization of armor-related rules.

By default, it follows TT ruleset and limits amount of armor assignable to each part
to structure * 2 (front and back combined), except head which is fixed to 45.
Effectively, it reduces max armor by 33% and forces players to compromise on front
defences if they want to have any rear armor.

Installation
------------

The mod depends on [DynModLib](https://github.com/CptMoore/DynModLib), which itself
depends on [BTML](https://github.com/Mpstark/BattleTechModLoader) and [ModTek](https://github.com/Mpstark/ModTek).

Compatibility
-------------

The mod does not break saved games and can be enabled at any point in the campaign. It is also compatible
with all stock loadouts and additional chassis provided by other mods.


Settings
--------

You can customize settings in `mod.json`. 

* (experimental) `useCurrentStructure` defines if calculations should be based on chassis' default structure or its current value (which might be different for damaged mechs in campaign)
* `structure*LoadCapacityFactor` settings determine how much armor can be mounted (in total, on front and rear sides) per point of structure
* `headMaxArmorOverride` allows to set head armor ignoring other settings, use null to make it scale as other parts

### Defaults

```json
{
	"useCurrentStructure": false,
	"headMaxArmorOverride": 45,
	"structureTotalLoadCapacityFactor": 2.0,
	"structureFrontLoadCapacityFactor": 2.0,
	"structureRearLoadCapacityFactor": 1.0
}
```
