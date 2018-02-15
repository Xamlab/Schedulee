﻿using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;

namespace Schedulee.Droid.Extensions
{
    public static class ViewExtensions
    {
        private static readonly Dictionary<string, Typeface> FontFaces = new Dictionary<string, Typeface>();

        public static void DisableChildren(this ViewGroup viewGroup)
        {
            if(viewGroup == null) return;

            for(int i = 0; i < viewGroup.ChildCount; i++)
            {
                var child = viewGroup.GetChildAt(i);
                child.Focusable = false;
                child.FocusableInTouchMode = false;
                child.Clickable = false;
                child.Enabled = false;

                if(child is ViewGroup childViewGroup) DisableChildren(childViewGroup);
            }
        }

        public static float ConvertDpToPixel(this Context context, float dp)
        {
            var resources = context.Resources;
            var metrics = resources.DisplayMetrics;
            float px = dp * ((float) metrics.DensityDpi / (int) DisplayMetricsDensity.Default);
            return px;
        }

        public static float ConvertPixelsToDp(this Context context, float px)
        {
            var resources = context.Resources;
            var metrics = resources.DisplayMetrics;
            float dp = px / ((float) metrics.DensityDpi / (int) DisplayMetricsDensity.Default);
            return dp;
        }
    }
}