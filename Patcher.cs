using ABN;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VitaminPatch
{

    public class MyPatcher
    {

        public static void ApplyPatches()
        {
            var harmony = new Harmony("com.VitaminMenu.patch");


            MethodInfo DroneMethod = AccessTools.Method(typeof(DroneComponent), "InternalUpdate", new Type[] { typeof(CraftData).MakeByRefType(), typeof(PlanetFactory), typeof(Vector3).MakeByRefType(), typeof(float), typeof(float), typeof(double).MakeByRefType(), typeof(double).MakeByRefType(), typeof(double), typeof(double), typeof(float).MakeByRefType() });
            MethodInfo DronenExportMethod = AccessTools.Method(typeof(DroneComponent), "Export", new Type[] { typeof(BinaryWriter) });

            MethodInfo MechaMethod = AccessTools.Method(typeof(Mecha), "GameTick", new Type[] { typeof(long), typeof(float) });
            MethodInfo MechaExportMethod = AccessTools.Method(typeof(Mecha), "Export", new Type[] { typeof(BinaryWriter) });

            MethodInfo BeltMethod = AccessTools.Method(typeof(CargoTraffic), "AlterBeltRenderer", new Type[] { typeof(int), typeof(EntityData[]), typeof(ColliderContainer[]), typeof(bool) });
            MethodInfo AlterBeldConnect = AccessTools.Method(typeof(CargoTraffic), "AlterBeltConnections", new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(bool) });
            MethodInfo ExportBeltMethod = AccessTools.Method(typeof(CargoTraffic), "Export", new Type[] { typeof(BinaryWriter) });

            MethodInfo AbnormalyMethod = AccessTools.Method(typeof(GameAbnormalityData_0925), "TriggerAbnormality", new Type[] { typeof(int), typeof(int), typeof(long[]) });


            VitaminLogger.LogInfo("Loading Patches...");
            harmony.Patch(DroneMethod, new HarmonyMethod(typeof(VitaminPatch.Patches).GetMethod(nameof(VitaminPatch.Patches.DronePrefix))));
            harmony.Patch(DronenExportMethod, new HarmonyMethod(typeof(VitaminPatch.Patches).GetMethod(nameof(VitaminPatch.Patches.ExportPrefix))));
            harmony.Patch(MechaMethod, new HarmonyMethod(typeof(VitaminPatch.Patches).GetMethod(nameof(VitaminPatch.Patches.MechaPrefix))));
            harmony.Patch(MechaExportMethod, new HarmonyMethod(typeof(VitaminPatch.Patches).GetMethod(nameof(VitaminPatch.Patches.MechaExportPrefix))));
            harmony.Patch(BeltMethod, new HarmonyMethod(typeof(VitaminPatch.Patches).GetMethod(nameof(VitaminPatch.Patches.AlterBeltPrefix))));
            harmony.Patch(AlterBeldConnect, new HarmonyMethod(typeof(VitaminPatch.Patches).GetMethod(nameof(VitaminPatch.Patches.AlterBeltConnectPrefix))));
            harmony.Patch(ExportBeltMethod, new HarmonyMethod(typeof(VitaminPatch.Patches).GetMethod(nameof(VitaminPatch.Patches.BeltExportPrefix))));
            harmony.Patch(AbnormalyMethod, new HarmonyMethod(typeof(VitaminPatch.Patches).GetMethod(nameof(VitaminPatch.Patches.AbnormalyPrefix))));
            VitaminLogger.LogInfo("Patches Loaded");


        }
    }
    public static class Patches
    {
        public static float cachedDronespeed = 0.0f;
        public static float cachedWalkSpeed = 0.0f;
        public static int cachedBeltSpeed = 0;
        public static float cachedminingSpeed = 0.0f;
        public static float cachedreplicate = 0.0f;

        public static bool DronePrefix(ref DroneComponent __instance, ref float droneSpeed)
        {
            if (cachedDronespeed == 0.0f)
            {
                cachedDronespeed = droneSpeed;
            }
            droneSpeed = droneSpeed * VitaminsMieseMenu.DroneSlider;

            return true;
        }
        public static bool ExportPrefix(ref DroneComponent __instance, ref BinaryWriter w)
        {
            VitaminsMieseMenu.DroneSlider = 1.0f;

            return true;
        }
        public static bool MechaPrefix(ref Mecha __instance)
        {
            if (cachedWalkSpeed == 0.0f)
            {
                cachedminingSpeed = __instance.miningSpeed;
                cachedWalkSpeed = __instance.walkSpeed;
                cachedreplicate = __instance.replicateSpeed;
            }
            if (VitaminsMieseMenu.MechaModded)
            {
                __instance.coreEnergy = __instance.coreEnergyCap;
                __instance.hp = __instance.hpMax;
                __instance.miningSpeed = cachedminingSpeed * 2;
                __instance.walkSpeed = cachedWalkSpeed * 1.3f;
                __instance.replicateSpeed = cachedreplicate * 10;
            }
            if (!VitaminsMieseMenu.MechaModded)
            {
                __instance.miningSpeed = cachedminingSpeed;
                __instance.walkSpeed = cachedWalkSpeed;
                __instance.replicateSpeed = cachedreplicate;
            }
            return true;

        }
        public static bool MechaExportPrefix(ref Mecha __instance, ref BinaryWriter w)
        {



            w.Write(10);
            w.Write(__instance.coreEnergyCap);
            w.Write(__instance.coreEnergy);
            w.Write(__instance.corePowerGen);
            w.Write(__instance.reactorPowerGen);
            w.Write(__instance.reactorEnergy);
            w.Write(__instance.reactorItemId);
            w.Write(__instance.reactorItemInc);
            w.Write(__instance.autoReplenishFuel);
            __instance.reactorStorage.Export(w);
            __instance.warpStorage.Export(w);
            w.Write(__instance.energyConsumptionCoef);
            w.Write(__instance.walkPower);
            w.Write(__instance.jumpEnergy);
            w.Write(__instance.thrustPowerPerAcc);
            w.Write(__instance.warpKeepingPowerPerSpeed);
            w.Write(__instance.warpStartPowerPerSpeed);
            w.Write(__instance.miningPower);
            w.Write(__instance.replicatePower);
            w.Write(__instance.researchPower);
            w.Write(__instance.droneEjectEnergy);
            w.Write(__instance.droneEnergyPerMeter);
            w.Write(__instance.instantBuildEnergy);
            w.Write(__instance.coreLevel);
            w.Write(__instance.thrusterLevel);
            w.Write(cachedminingSpeed);
            w.Write(cachedreplicate);
            w.Write(cachedWalkSpeed);
            w.Write(__instance.jumpSpeed);
            w.Write(__instance.maxSailSpeed);
            w.Write(__instance.maxWarpSpeed);
            w.Write(__instance.buildArea);
            __instance.forge.Export(w);
            __instance.lab.Export(w);
            __instance.constructionModule.Export(w);
            w.Write(__instance.autoReconstructLastSearchPos.x);
            w.Write(__instance.autoReconstructLastSearchPos.y);
            w.Write(__instance.autoReconstructLastSearchPos.z);
            w.Write(__instance.autoReconstructLastSearchAstroId);
            w.Write(__instance.buildLastSearchPos.x);
            w.Write(__instance.buildLastSearchPos.y);
            w.Write(__instance.buildLastSearchPos.z);
            w.Write(__instance.buildLastSearchAstroId);
            w.Write(__instance.repairLastSearchPos.x);
            w.Write(__instance.repairLastSearchPos.y);
            w.Write(__instance.repairLastSearchPos.z);
            w.Write(__instance.repairLastSearchAstroId);
            w.Write(__instance.hpMax);
            w.Write(__instance.hpMaxUpgrade);
            w.Write(__instance.hpRecover);
            w.Write(__instance.energyShieldUnlocked);
            w.Write(__instance.energyShieldRechargeEnabled);
            w.Write(__instance.energyShieldRechargeSpeed);
            w.Write(__instance.energyShieldRadius);
            w.Write(__instance.energyShieldCapacity);
            w.Write(__instance.energyShieldEnergyRate);
            w.Write(__instance.hp);
            w.Write(__instance.hpRecoverCD);
            w.Write(__instance.energyShieldRecoverCD);
            w.Write(__instance.energyShieldEnergy);
            w.Write(__instance.energyShieldBurstUnlocked);
            w.Write(__instance.energyShieldBurstDamageRate);
            w.Write(__instance.ammoItemId);
            w.Write(__instance.ammoInc);
            w.Write(__instance.ammoBulletCount);
            w.Write(__instance.ammoSelectSlot);
            w.Write(__instance.ammoSelectSlotState);
            w.Write(__instance.ammoMuzzleFire);
            w.Write(__instance.ammoRoundFire);
            w.Write(__instance.ammoMuzzleIndex);
            w.Write(__instance.laserActive);
            w.Write(__instance.laserActiveState);
            w.Write(__instance.laserRecharging);
            w.Write(__instance.laserEnergy);
            w.Write(__instance.laserEnergyCapacity);
            w.Write(__instance.laserFire);
            w.Write(__instance.bombActive);
            w.Write(__instance.bombFire);
            w.Write(__instance.autoReplenishAmmo);
            __instance.ammoStorage.Export(w);
            __instance.bombStorage.Export(w);
            __instance.ammoHatredTarget.Export(w);
            __instance.laserHatredTarget.Export(w);
            w.Write(__instance.bulletLocalAttackRange);
            w.Write(__instance.bulletSpaceAttackRange);
            w.Write(__instance.bulletEnergyCost);
            w.Write(__instance.bulletDamageScale);
            w.Write(__instance.bulletROF);
            w.Write(__instance.bulletMuzzleCount);
            w.Write(__instance.bulletMuzzleInterval);
            w.Write(__instance.bulletRoundInterval);
            w.Write(__instance.cannonLocalAttackRange);
            w.Write(__instance.cannonSpaceAttackRange);
            w.Write(__instance.cannonEnergyCost);
            w.Write(__instance.cannonDamageScale);
            w.Write(__instance.cannonROF);
            w.Write(__instance.cannonMuzzleCount);
            w.Write(__instance.cannonMuzzleInterval);
            w.Write(__instance.cannonRoundInterval);
            w.Write(__instance.plasmaLocalAttackRange);
            w.Write(__instance.plasmaSpaceAttackRange);
            w.Write(__instance.plasmaEnergyCost);
            w.Write(__instance.plasmaDamageScale);
            w.Write(__instance.plasmaROF);
            w.Write(__instance.plasmaMuzzleCount);
            w.Write(__instance.plasmaMuzzleInterval);
            w.Write(__instance.plasmaRoundInterval);
            w.Write(__instance.missileLocalAttackRange);
            w.Write(__instance.missileSpaceAttackRange);
            w.Write(__instance.missileEnergyCost);
            w.Write(__instance.missileDamageScale);
            w.Write(__instance.missileROF);
            w.Write(__instance.missileMuzzleCount);
            w.Write(__instance.missileMuzzleInterval);
            w.Write(__instance.missileRoundInterval);
            w.Write(__instance.laserLocalAttackRange);
            w.Write(__instance.laserSpaceAttackRange);
            w.Write(__instance.laserLocalEnergyCost);
            w.Write(__instance.laserSpaceEnergyCost);
            w.Write(__instance.laserLocalDamage);
            w.Write(__instance.laserSpaceDamage);
            w.Write(__instance.laserLocalInterval);
            w.Write(__instance.laserSpaceInterval);
            w.Write(__instance.autoReplenishHangar);
            __instance.fighterStorage.Export(w);
            __instance.groundCombatModule.Export(w);
            __instance.spaceCombatModule.Export(w);
            if (__instance.energyShieldResistHistory != null)
            {
                w.Write(__instance.energyShieldResistHistory.Length);
                for (int i = 0; i < __instance.energyShieldResistHistory.Length; i++)
                {
                    w.Write(__instance.energyShieldResistHistory[i]);
                }
            }
            else
            {
                w.Write(0);
            }
            __instance.appearance.Export(w);
            __instance.diyAppearance.Export(w);
            w.Write(__instance.diyItems.items.Count);
            foreach (KeyValuePair<int, int> keyValuePair in __instance.diyItems.items)
            {
                w.Write(keyValuePair.Key);
                w.Write(keyValuePair.Value);
            }
            w.Write(2119973658);
            return false;
        }

        public static bool AlterBeltPrefix(ref CargoTraffic __instance, int beltId)
        {
            if (cachedBeltSpeed == 0)
            {
                cachedBeltSpeed = __instance.beltPool[beltId].speed;
            }

            __instance.beltPool[beltId].speed = cachedBeltSpeed * (int)VitaminsMieseMenu.beltSlider;


            return true;
        }
        public static bool AlterBeltConnectPrefix(ref CargoTraffic __instance, int beltId)
        {
            if (cachedBeltSpeed == 0)
            {
                cachedBeltSpeed = __instance.beltPool[beltId].speed;
            }

            __instance.beltPool[beltId].speed = cachedBeltSpeed * (int)VitaminsMieseMenu.beltSlider;


            return true;
        }
        public static bool BeltExportPrefix(ref CargoTraffic __instance, ref BinaryWriter w)
        {
            for (int i = 0; i < __instance.beltPool.Length; i++)
            {
                __instance.beltPool[i].speed = cachedBeltSpeed; // Setze die Geschwindigkeit auf den gewünschten Wert.
            }
            return true;
        }
        public static bool AbnormalyPrefix(GameAbnormalityData_0925 __instance, int protoId, int minRecordVersion, params long[] vals)
        {
            if (!VitaminsMieseMenu.achievementToggle)
            {
                if (protoId > 0 && protoId < 100)
                {
                    int num = protoId * 30;
                    Array.Copy(__instance.runtimeDatas, num, __instance.runtimeDatas, num + 1, 29);
                    __instance.runtimeDatas[num].protoId = protoId;
                    __instance.runtimeDatas[num].triggerTime = GameMain.gameTick;
                    if (vals != null)
                    {
                        __instance.runtimeDatas[num].evidences = vals;
                    }
                    __instance.NotifyOnAbnormalityChecked(protoId);
                    Debug.LogWarning(__instance.runtimeDatas[num].ToDebugString());
                }
            }
            return false;
        }


    }
}
