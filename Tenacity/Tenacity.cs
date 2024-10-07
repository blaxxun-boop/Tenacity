using System;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using JetBrains.Annotations;
using SkillManager;

namespace Tenacity;

[BepInPlugin(ModGUID, ModName, ModVersion)]
[BepInIncompatibility("org.bepinex.plugins.valheim_plus")]
public class Tenacity : BaseUnityPlugin
{
	private const string ModName = "Tenacity";
	private const string ModVersion = "1.0.3";
	private const string ModGUID = "org.bepinex.plugins.tenacity";

	public void Awake()
	{
		Skill tenacity = new("Tenacity", "tenacity-icon.png");
		tenacity.Description.English("Reduces damage taken slightly.");
		tenacity.Name.German("Hartnäckigkeit");
		tenacity.Description.German("Reduziert den erlittenen Schaden.");
  		tenacity.Name.Chinese("韧性");
		tenacity.Description.Chinese("略微降低承受的伤害");
		tenacity.Configurable = true;

		Assembly assembly = Assembly.GetExecutingAssembly();
		Harmony harmony = new(ModGUID);
		harmony.PatchAll(assembly);
	}

	[HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
	private class ReduceDamageTaken
	{
		[UsedImplicitly]
		private static void Prefix(Character __instance, HitData hit)
		{
			if (__instance is Player player)
			{
				player.RaiseSkill("Tenacity", (float)Math.Sqrt(hit.GetTotalDamage()));

				hit.ApplyModifier(1 - player.GetSkillFactor("Tenacity") * 0.2f);
			}
		}
	}
}
