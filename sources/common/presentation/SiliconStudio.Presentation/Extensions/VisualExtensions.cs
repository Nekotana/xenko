﻿// Copyright (c) 2014-2016 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using SiliconStudio.Presentation.Interop;

namespace SiliconStudio.Presentation.Extensions
{
    public static class VisualExtensions
    {
        /// <summary>
        /// Gets the position of the cursor in the current coordinates of the given <paramref name="visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        /// <remarks>
        /// This method does not rely on <see cref="System.Windows.Input.Mouse"/> but calls native code to retrieve the position of the cursor.
        /// </remarks>
        /// <seealso cref="NativeHelper.GetCursorPos"/>
        /// <seealso cref="Visual.PointFromScreen"/>
        public static Point GetCursorRelativePosition(this Visual visual)
        {
            if (visual == null) throw new ArgumentNullException(nameof(visual));

            NativeHelper.POINT point;
            NativeHelper.GetCursorPos(out point);
            return visual.PointFromScreen((Point)point);
        }

        /// <summary>
        /// Gets the position of the cursor in the WPF screen coordinates of the given <paramref name="visual"/>'s hosting window.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        /// <seealso cref="WindowHelper.GetCursorScreenPosition"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point GetCursorScreenPosition(this Visual visual)
        {
            if (visual == null) throw new ArgumentNullException(nameof(visual));

            return Window.GetWindow(visual).GetCursorScreenPosition();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Visual FindAdornable(this Visual source)
        {
            return FindAdornableOfType<Visual>(source);
        }

        public static T FindAdornableOfType<T>(this Visual source) where T : Visual
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (AdornerLayer.GetAdornerLayer(source) != null && source is T)
                return (T)source;

            var childCount = VisualTreeHelper.GetChildrenCount(source);
            for (var i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(source, i) as T;
                var test = child?.FindAdornableOfType<T>();
                if (test != null)
                    return test;
            }

            return null;
        }

        public static AdornerLayer FindAdornerLayer(this Visual source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var adornerLayer = AdornerLayer.GetAdornerLayer(source);
            if (adornerLayer != null)
                return adornerLayer;

            var childCount = VisualTreeHelper.GetChildrenCount(source);
            for (var i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(source, i) as Visual;
                var test = child?.FindAdornerLayer();
                if (test != null)
                    return test;
            }

            return null;
        }

        /// <summary>
        /// Converts a <see cref="Rect"/> in screen coordinates into a <see cref="Rect"/> that represents the current coordinate system of the <see cref="Visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="rect">The <see cref="Rect"/> value in screen coordinates.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect RectFromScreen(this Visual visual, Rect rect)
        {
            return RectFromScreen(visual, ref rect);
        }

        /// <summary>
        /// Converts a <see cref="Rect"/> in screen coordinates into a <see cref="Rect"/> that represents the current coordinate system of the <see cref="Visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="rect">The <see cref="Rect"/> value in screen coordinates.</param>
        /// <returns></returns>
        public static Rect RectFromScreen(this Visual visual, ref Rect rect)
        {
            if (visual == null) throw new ArgumentNullException(nameof(visual));

            var topLeft = visual.PointFromScreen(new Point(rect.Left, rect.Top));
            var bottomRight = visual.PointFromScreen(new Point(rect.Right, rect.Bottom));
            return new Rect(topLeft, bottomRight);
        }

        /// <summary>
        /// Converts a <see cref="Rect"/> that represents the current coordinate system of the <see cref="Visual"/> into a <see cref="Rect"/> in screen coordinates.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="rect">The <see cref="Rect"/> value that represents the current coordinate system of the <see cref="Visual"/>.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect RectToScreen(this Visual visual, Rect rect)
        {
            return RectToScreen(visual, ref rect);
        }

        /// <summary>
        /// Converts a <see cref="Rect"/> that represents the current coordinate system of the <see cref="Visual"/> into a <see cref="Rect"/> in screen coordinates.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="rect">The <see cref="Rect"/> value that represents the current coordinate system of the <see cref="Visual"/>.</param>
        /// <returns></returns>
        public static Rect RectToScreen(this Visual visual, ref Rect rect)
        {
            if (visual == null) throw new ArgumentNullException(nameof(visual));

            var topLeft = visual.PointToScreen(new Point(rect.Left, rect.Top));
            var bottomRight = visual.PointToScreen(new Point(rect.Right, rect.Bottom));
            return new Rect(topLeft, bottomRight);
        }

    }
}
