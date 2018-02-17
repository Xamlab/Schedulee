using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using GalaSoft.MvvmLight.Helpers;
using Schedulee.Droid.Views.Base;

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

        public static void ShowOverlay(this Context context, View overlay)
        {
            overlay.Visibility = ViewStates.Visible;
            var fadeInOverlay = AnimationUtils.LoadAnimation(context, Resource.Animation.overlay_fade_in_animation);
            overlay.StartAnimation(fadeInOverlay);
        }

        public static void HideOverlay(this Context context, View overlay)
        {
            var fadeOutAnimation = AnimationUtils.LoadAnimation(context, Resource.Animation.overlay_fade_out_animation);
            overlay.StartAnimation(fadeOutAnimation);
            overlay.Visibility = ViewStates.Invisible;
        }

        public static void ClearBindings(this IEnumerable<Binding> bindings)
        {
            if(bindings == null) return;
            foreach(var binding in bindings)
            {
                binding.Detach();
            }
        }

        public static Binding<TSource, TTarget> SetBindingEx<TSource, TTarget>(this BaseActivity source, Expression<Func<TSource>> sourcePropertyExpression, Expression<Func<TTarget>> targetPropertyExpression = null, BindingMode mode = BindingMode.Default, TSource fallbackValue = default(TSource), TSource targetNullValue = default(TSource))
        {
            var binding = new Binding<TSource, TTarget>(source, sourcePropertyExpression, null, targetPropertyExpression, mode, fallbackValue, targetNullValue);
            source.Bindings.Add(binding);
            return binding;
        }
    }
}