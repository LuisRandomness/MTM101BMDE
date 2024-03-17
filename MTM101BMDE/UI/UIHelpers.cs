﻿using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MTM101BaldAPI.UI
{
    /*public enum BaldiFonts
    {
        ComicSans12,
        BoldComicSans12,
        ComicSans18,
        ComicSans24,
        BoldComicSans24,
        ComicSans36,
        SmoothComicSans12,
        SmoothComicSans18,
        SmoothComicSans24,
        SmoothComicSans36
    }*/

    public static class UIExtensions
    {
        /// <summary>
        /// Converts a UI element to a button by adding the necessary components and initializing the correct variables and adding the "Button" tag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="autoAssign">Should the StandardMenuButtons image and text fields be automatically assigned using GetComponent?</param>
        /// <returns></returns>
        public static T ConvertToButton<T>(this GameObject obj, bool autoAssign = true) where T : StandardMenuButton
        {
            T smb = obj.AddComponent<T>();
            smb.InitializeAllEvents();
            if (autoAssign)
            {
                smb.image = obj.GetComponent<Image>();
                smb.text = obj.GetComponent<TMP_Text>();
            }
            smb.gameObject.tag = "Button";
            return smb;
        }

        public static StandardMenuButton InitializeAllEvents(this StandardMenuButton smb)
        {
            smb.OnPress = new UnityEngine.Events.UnityEvent();
            smb.OnHighlight = new UnityEngine.Events.UnityEvent();
            smb.OnRelease = new UnityEngine.Events.UnityEvent();
            return smb;
        }
    }

    public static class UIHelpers
    {
        /// <summary>
        /// Creates an image based off of the sprite, handling its RectTransform.
        /// </summary>
        /// <param name="spr"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="correctPosition">If the position should be corrected based off of the top left of a 4:3 screen. This is primarily for custom field trips and UI.</param>
        /// <param name="scale">The scale of the image</param>
        /// <returns></returns>
        public static Image CreateImage(Sprite spr, Transform parent, Vector3 position, bool correctPosition = false, float scale = 1f)
        {
            Image img = new GameObject().AddComponent<Image>();
            img.gameObject.layer = LayerMask.NameToLayer("UI");
            img.transform.SetParent(parent);
            img.sprite = spr;
            img.gameObject.transform.localScale = Vector3.one;
            img.rectTransform.offsetMin = new Vector2(-spr.texture.width / 2f, -spr.texture.height / 2f);
            img.rectTransform.offsetMax = new Vector2(spr.texture.width / 2f, spr.texture.height / 2f);
            img.rectTransform.anchorMin = new Vector2(0f, 1f);
            img.rectTransform.anchorMax = new Vector2(0f, 1f);
            if (correctPosition)
            {
                img.transform.localPosition = new Vector3(-240f, 180f) + (new Vector3(position.x, position.y * -1f));
            }
            else
            {
                img.transform.localPosition = position;
            }
            img.transform.localScale *= scale;
            return img;
        }

        /// <summary>
        /// Creates an image based off of the sprite, handling its RectTransform.
        /// </summary>
        /// <param name="spr"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="correctPosition">If the position should be corrected based off of the top left of a 4:3 screen. This is primarily for custom field trips and UI.</param>
        /// <returns></returns>
        public static Image CreateImage(Sprite spr, Transform parent, Vector3 position, bool correctPosition = false)
        {
            return CreateImage(spr, parent, position, correctPosition, 1f);
        }

        /*public static T CreateText<T>(BaldiFonts font, string text, Transform parent, Vector3 position, bool correctPosition = false) where T : TMP_Text
        {
            T tmp = new GameObject().AddComponent<T>();
            tmp.name = "Text";
            tmp.gameObject.layer = LayerMask.NameToLayer("UI");
            tmp.transform.SetParent(parent);
            tmp.gameObject.transform.localScale = Vector3.one;
            switch (font)
            {
                case BaldiFonts.ComicSans12:
                    tmp.fontSize = 12f;
                    tmp.font = MTM101BaldiDevAPI.AssetMan.Get<TMP_FontAsset>("COMIC_12_Pro");
                    break;
                case BaldiFonts.BoldComicSans12:
                    tmp.fontSize = 12f;
                    tmp.font = MTM101BaldiDevAPI.AssetMan.Get<TMP_FontAsset>("COMIC_BOLD_12_Pro");
                    break;
                case BaldiFonts.ComicSans18:
                    tmp.fontSize = 18f;
                    tmp.font = MTM101BaldiDevAPI.AssetMan.Get<TMP_FontAsset>("COMIC_18_Pro");
                    break;
                case BaldiFonts.ComicSans24:
                    tmp.fontSize = 24f;
                    tmp.font = MTM101BaldiDevAPI.AssetMan.Get<TMP_FontAsset>("COMIC_24_Pro");
                    break;
                case BaldiFonts.BoldComicSans24:
                    tmp.fontSize = 24f;
                    tmp.font = MTM101BaldiDevAPI.AssetMan.Get<TMP_FontAsset>("COMIC_BOLD_24_Pro");
                    break;
                case BaldiFonts.ComicSans36:
                    tmp.fontSize = 36f;
                    tmp.font = MTM101BaldiDevAPI.AssetMan.Get<TMP_FontAsset>("COMIC_36_Pro");
                    break;
                case BaldiFonts.SmoothComicSans12:
                    tmp.fontSize = 12f;
                    tmp.font = MTM101BaldiDevAPI.AssetMan.Get<TMP_FontAsset>("COMIC_12_Smooth_Pro");
                    break;
                case BaldiFonts.SmoothComicSans18:
                    tmp.fontSize = 18f;
                    tmp.font = MTM101BaldiDevAPI.AssetMan.Get<TMP_FontAsset>("COMIC_18_Smooth_Pro");
                    break;
                case BaldiFonts.SmoothComicSans24:
                    tmp.fontSize = 24f;
                    tmp.font = MTM101BaldiDevAPI.AssetMan.Get<TMP_FontAsset>("COMIC_24_Smooth_Pro");
                    break;
                case BaldiFonts.SmoothComicSans36:
                    tmp.fontSize = 36f;
                    tmp.font = MTM101BaldiDevAPI.AssetMan.Get<TMP_FontAsset>("COMIC_36_Smooth_Pro");
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (correctPosition)
            {
                tmp.transform.localPosition = new Vector3(-240f, 180f) + (new Vector3(position.x, position.y * -1f));
            }
            else
            {
                tmp.transform.localPosition = position;
            }
            tmp.text = text;
            return tmp;
        }*/


    }
}
