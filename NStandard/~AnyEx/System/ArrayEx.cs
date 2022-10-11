﻿using System;
using System.Linq;
#if NET5_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace NStandard
{
    public static class ArrayEx
    {
        private static string InsufficientElements() => "Insufficient elements in source array.";
        private static string CopyingOverflow() => "Copying the specified array results in overflow.";

#if NET5_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NET46_OR_GREATER
#else
        private static class EmptyArray<T>
        {
#pragma warning disable CA1825 // this is the implementation of Array.Empty<T>()
            internal static readonly T[] Value = new T[0];
#pragma warning restore CA1825
        }

        public static T[] Empty<T>()
        {
            return EmptyArray<T>.Value;
        }
#endif

        /// <summary>
        /// Assign values to arrays of indeterminate dimensions using one-dimensional arrays.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        public static void Assign<T>(Array destination, T[] source)
        {
            if (destination.GetSequenceLength() < source.Length) throw new ArgumentException(CopyingOverflow(), nameof(source));

            var stepper = new IndicesStepper(0, destination.GetLengths());
#if NET5_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
            foreach (var (value, indices) in Any.Zip(source, stepper))
            {
#else
            foreach (var pair in Any.Zip(source, stepper, (Value, Indices) => new { Value, Indices }))
            {
                var value = pair.Value;
                var indices = pair.Indices;
#endif
                destination.SetValue(value, indices);
            }
        }

        /// <summary>
        /// Assign values to arrays of indeterminate dimensions using one-dimensional arrays.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destination"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="source"></param>
        /// <param name="sourceIndex"></param>
        /// <param name="length"></param>
        public static void Assign<T>(Array destination, int destinationIndex, T[] source, int sourceIndex, int length)
        {
            if ((source.Length - sourceIndex) < length) throw new ArgumentException(InsufficientElements(), nameof(source));
            if ((destination.GetSequenceLength() - destinationIndex) < length) throw new ArgumentException(CopyingOverflow(), nameof(source));

            var stepper = new IndicesStepper(destinationIndex, destination.GetLengths());
#if NET5_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
            foreach (var (value, indices) in Any.Zip(source.Skip(sourceIndex).Take(length), stepper))
            {
#else
            foreach (var pair in Any.Zip(source.Skip(sourceIndex).Take(length), stepper, (Value, Indices) => new { Value, Indices }))
            {
                var value = pair.Value;
                var indices = pair.Indices;
#endif
                destination.SetValue(value, indices);
            }
        }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Assign values to arrays of indeterminate dimensions using one-dimensional arrays.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="length"></param>
        public static unsafe void Assign<T>(T* destination, T* source, int length) where T : unmanaged
        {
            Unsafe.CopyBlock(destination, source, (uint)(length * sizeof(T)));
        }

        /// <summary>
        /// Assign values to arrays of indeterminate dimensions using one-dimensional arrays.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destination"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="source"></param>
        /// <param name="sourceIndex"></param>
        /// <param name="length"></param>
        public static unsafe void Assign<T>(T* destination, int destinationIndex, T* source, int sourceIndex, int length) where T : unmanaged
        {
            Unsafe.CopyBlock(destination + destinationIndex, source + sourceIndex, (uint)(length * sizeof(T)));
        }
#endif
    }
}
