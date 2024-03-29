﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XController
{

    /// <summary>
    /// 2D vector structure
    /// </summary>
    public struct Vector
    {
        /// <summary>
        /// Horizontal coordinates relative to the center
        /// </summary>
        public double X;

        /// <summary>
        /// Vertical coordinates relative to the center
        /// </summary>
        public double Y;

        /// <summary>
        /// Create a new instance of the Vector structure
        /// </summary>
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the magnitude of the vector
        /// </summary>
        /// <returns>Magnitude as double</returns>
        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        /// <summary>
        /// Gets a string representation of the object
        /// </summary>
        /// <returns>A string with X and Y values</returns>
        public override string ToString()
        {
            return $"X: {X:0.000}, Y: {Y:0.000}";
        }

        /// <summary>
        /// Multiply a vector with a scalar
        /// </summary>
        /// <param name="vector">Base vector</param>
        /// <param name="scalar">Scalar</param>
        /// <returns>Scaled vector</returns>
        public static Vector operator *(Vector vector, double scalar)
        {
            return new Vector(vector.X * scalar, vector.Y * scalar);
        }

        /// <summary>
        /// Divide a vector with a scalar
        /// </summary>
        /// <param name="vector">Base vector</param>
        /// <param name="scalar">Scalar</param>
        /// <returns>Scaled vector</returns>
        public static Vector operator /(Vector vector, double scalar)
        {
            return new Vector(vector.X / scalar, vector.Y / scalar);
        }
    }

    /// <summary>
    /// Class for utility functions
    /// </summary>
    internal static class Utility
    {
        /// <summary>
        /// Maps a value from one range to another
        /// </summary>
        /// <param name="value">Value to map</param>
        /// <param name="oldMin">Source lower bound</param>
        /// <param name="oldMax">Source upper bound</param>
        /// <param name="newMin">New lower bound</param>
        /// <param name="newMax">New upper bound</param>
        /// <returns>Value in new range as double</returns>
        public static double Map(double value, double oldMin, double oldMax, double newMin, double newMax)
        {
            return newMin + (newMax - newMin) * (value - oldMin) / (oldMax - oldMin);
        }

        /// <summary>
        /// Append string to StringBuilder if value is true
        /// </summary>
        /// <param name="stringBuilder">StringBuilder object</param>
        /// <param name="value">Boolean condition</param>
        /// <param name="s">String to append</param>
        /// <param name="separator">Separator to append</param>
        public static void AppendIfTrue(StringBuilder stringBuilder, bool value, string s, string separator = ", ")
        {
            if (value)
                stringBuilder.Append(s).Append(separator);
        }
    }
}
