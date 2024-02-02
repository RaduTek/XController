using System;
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
        /// Gets a string representation of the object
        /// </summary>
        /// <returns>A string with X and Y values</returns>
        public override string ToString()
        {
            return $"X: {X:0.000}, Y: {Y:0.000}";
        }

        /// <summary>
        /// Scale a vector object (2d vector)
        /// </summary>
        /// <param name="vector">Base vector</param>
        /// <param name="scalar">Scalar</param>
        /// <returns></returns>
        public static Vector operator *(Vector vector, double scalar)
        {
            return new Vector(vector.X * scalar, vector.Y * scalar);
        }
    }

    /// <summary>
    /// Class for utility functions
    /// </summary>
    internal static class Utility
    {
    }
}
