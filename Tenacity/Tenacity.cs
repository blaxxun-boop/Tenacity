﻿using System;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using JetBrains.Annotations;
using SkillManager;

namespace Tenacity
{
	[BepInPlugin(ModGUID, ModName, ModVersion)]
	public class Tenacity : BaseUnityPlugin
	{
		private const string ModName = "Tenacity";
		private const string ModVersion = "1.0";
		private const string ModGUID = "org.bepinex.plugins.tenacity";

		public void Awake()
		{
			Skill tenacity = new("Tenacity", "tenacity-icon.png");
			tenacity.Description.English("Reduces damage taken by 0.2% per level.");
			tenacity.Name.German("Hartnäckigkeit");
			tenacity.Description.German("Reduziert erlittenen Schaden um 0,2% pro Stufe.");
			tenacity.Name.Turkish("Dayanıklılık");
			tenacity.Description.Turkish("Seviye başına alınan hasarı %0.2 azaltır.");
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
					player.RaiseSkill("Tenacity", (float) Math.Sqrt(hit.GetTotalDamage()));

					hit.ApplyModifier(1 - player.GetSkillFactor("Tenacity") * 0.2f);
				}
			}
		}
	}
}
