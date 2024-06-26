﻿using BepInEx;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace MTM101BaldAPI
{
    public static class Extensions
    {

        public static void ConvertToPrefab(this GameObject me, bool setActive)
        {
            if (MTM101BaldiDevAPI.PrefabSubObject == null)
            {
                throw new NullReferenceException("Attempted to ConvertToPrefab before AssetsLoaded!");
            }
            me.MarkAsNeverUnload();
            me.transform.SetParent(MTM101BaldiDevAPI.PrefabSubObject.transform);
            if (setActive)
            {
                me.SetActive(true);
            }
        }

        static MethodInfo _EndTransition = AccessTools.Method(typeof(GlobalCam), "EndTransition");
        static FieldInfo _transitioner = AccessTools.Field(typeof(GlobalCam), "transitioner");
        public static void StopCurrentTransition(this GlobalCam me)
        {
            IEnumerator transitioner = (IEnumerator)_transitioner.GetValue(me);
            me.StopCoroutine(transitioner);
            _EndTransition.Invoke(me, null);
        }

        /// <summary>
        /// Repeatedly calls MoveNext until the IEnumerator is empty
        /// </summary>
        /// <param name="numerator"></param>
        /// <returns>All the things returned by the IEnumerator</returns>
        public static List<object> MoveUntilDone(this IEnumerator numerator)
        {
            List<object> returnValue = new List<object>();
            while (numerator.MoveNext())
            {
                returnValue.Add(numerator.Current);
            }
            return returnValue;
        }

        public static void MarkAsNeverUnload(this UnityEngine.Object me)
        {
            if (!MTM101BaldiDevAPI.keepInMemory.Contains(me))
            {
                MTM101BaldiDevAPI.keepInMemory.Add(me);
            }
        }
        public static void RemoveUnloadMark(this UnityEngine.Object me)
        {
            MTM101BaldiDevAPI.keepInMemory.Remove(me);
        }

        public static void MarkAsNeverUnload(this ScriptableObject me)
        {
            if (!MTM101BaldiDevAPI.keepInMemory.Contains(me))
            {
                MTM101BaldiDevAPI.keepInMemory.Add(me);
            }
        }
        public static void RemoveUnloadMark(this ScriptableObject me)
        {
            MTM101BaldiDevAPI.keepInMemory.Remove(me);
        }

        public static int GetCellIndexAt(this RoomAsset me, int x, int y)
        {
            for (int i = 0; i < me.cells.Count; i++)
            {
                if ((me.cells[i].pos.x == x) && (me.cells[i].pos.z == y))
                {
                    return i;
                }
            }
            return -1;
        }

        public static CellData GetCellAt(this RoomAsset me, int x, int y)
        {
            int index = me.GetCellIndexAt(x, y);
            if (index == -1) return null;
            return me.cells[index];
        }

        public static void SetMainTexture(this Material me, Texture texture)
        {
            me.SetTexture("_MainTex", texture);
        }

        public static void SetMaskTexture(this Material me, Texture texture)
        {
            me.SetTexture("_Mask", texture);
        }

        public static void ApplyDoorMaterials(this StandardDoor me, StandardDoorMats materials, Material mask = null)
        {
            me.overlayShut[0] = materials.shut;
            me.overlayShut[1] = materials.shut;
            me.overlayOpen[0] = materials.open;
            me.overlayOpen[1] = materials.open;
            if (mask != null)
            {
                me.mask[0] = mask;
                me.mask[1] = mask;
            }
            me.UpdateTextures();
        }
    }
}

namespace MTM101BaldAPI.Registers
{
    public static class MetaExtensions
    {
        public static ItemMetaData AddMeta(this ItemObject me, BaseUnityPlugin plugin, ItemFlags flags)
        {
            ItemMetaData meta = new ItemMetaData(plugin.Info, me);
            meta.flags = flags;
            MTM101BaldiDevAPI.itemMetadata.Add(me, meta);
            return meta;
        }

        public static ItemMetaData AddMeta(this ItemObject me, ItemMetaData meta)
        {
            MTM101BaldiDevAPI.itemMetadata.Add(me, meta);
            return meta;
        }

        public static NPCMetadata AddMeta(this NPC me, BaseUnityPlugin plugin, NPCFlags flags)
        {
            NPCMetadata existingMeta = NPCMetaStorage.Instance.Find(x => x.character == me.Character);
            if (existingMeta != null)
            {
                MTM101BaldiDevAPI.Log.LogInfo("NPC " + EnumExtensions.GetExtendedName<Character>((int)me.Character) + " already has meta! Adding prefab instead...");
                existingMeta.prefabs.Add(me.name, me);
                return existingMeta;
            }
            NPCMetadata npcMeta = new NPCMetadata(plugin.Info, new NPC[1] { me }, me.name, flags);
            NPCMetaStorage.Instance.Add(npcMeta);
            return npcMeta;
        }

        public static List<T> ToValues<T>(this List<IMetadata<T>> me)
        {
            List<T> returnL = new List<T>();
            me.Do(x =>
            {
                if (x.value != null)
                {
                    returnL.Add(x.value);
                }
            });
            return returnL;
        }

        public static T[] ToValues<T>(this IMetadata<T>[] me)
        {
            T[] returnL = new T[me.Length];
            for (int i = 0; i < me.Length; i++)
            {
                returnL[i] = me[i].value;
            }
            return returnL;
        }

        public static ItemMetaData GetMeta(this ItemObject me)
        {
            return MTM101BaldiDevAPI.itemMetadata.Get(me);
        }

        public static NPCMetadata GetMeta(this NPC me)
        {
            return NPCMetaStorage.Instance.Get(me.Character);
        }

        public static bool AddMetaPrefab(this NPC me)
        {
            return NPCMetaStorage.Instance.AddPrefab(me);
        }

        public static RandomEventMetadata GetMeta(this RandomEvent randomEvent)
        {
            return RandomEventMetaStorage.Instance.Get(randomEvent.Type);
        }
    }
}