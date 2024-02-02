using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XController
{
    /// <summary>
    /// Class for deadzone calculation functions
    /// </summary>
    internal class Deadzone
    {
        /// <summary>
        /// Clips the vector's coordinates to a maximum
        /// </summary>
        /// <param name="input">Input vector</param>
        /// <param name="max">Maximum vector</param>
        /// <returns>Clipped vector</returns>
        public static Vector Clip(Vector input, double max)
        {
            Vector result = new Vector(input.X, input.Y);
            if (Math.Abs(input.X) > max)
            {
                result.X = max * Math.Sign(input.X);
            }
            if (Math.Abs(input.Y) > max)
            {
                result.Y = max * Math.Sign(input.Y);
            }
            return result;
        }

        /// <summary>
        /// Calculate coordinates with scaled radial method
        /// Source: https://github.com/Minimuino/thumbstick-deadzones
        /// </summary>
        /// <param name="input">Controller input vector</param>
        /// <param name="deadzone">Deadzone ratio (0 to 1)</param>
        /// <returns>Calculated vector</returns>
        public static Vector ScaledRadial(Vector input, double deadzone)
        {
            double magnitude = input.Magnitude();
            if (magnitude < deadzone)
            {
                return new Vector(0, 0);
            }

            Vector normalized = input / magnitude;

            return normalized * Utility.Map(magnitude, deadzone, 1, 0, 1);
        }

        /// <summary>
        /// Calculate coordinates with sloped scaled axial method
        /// Source: https://github.com/Minimuino/thumbstick-deadzones
        /// </summary>
        /// <param name="input">Controller input vector</param>
        /// <param name="deadzone">Deadzone ratio (0 to 1)</param>
        /// <returns>Calculated vector</returns>
        public static Vector SlopedScaledAxial(Vector input, double deadzone)
        {
            Vector deadzoneV = new Vector(deadzone * Math.Abs(input.X), deadzone * Math.Abs(input.Y));

            Vector result = new Vector(0, 0);

            Vector sign = new Vector(Math.Sign(input.X), Math.Sign(input.Y));

            if (Math.Abs(input.X) > deadzoneV.X)
            {
                result.X = sign.X * Utility.Map(Math.Abs(input.X), deadzoneV.X, 1, 0, 1);
            }

            if (Math.Abs(input.Y) > deadzoneV.Y)
            {
                result.Y = sign.Y * Utility.Map(Math.Abs(input.Y), deadzoneV.Y, 1, 0, 1);
            }

            return result;
        }

        /// <summary>
        /// Calculate coordinates in a hybrid mode
        /// Source: https://github.com/Minimuino/thumbstick-deadzones
        /// </summary>
        /// <param name="input">Controller input vector</param>
        /// <param name="deadzone">Deadzone ratio (0 to 1)</param>
        /// <returns>Calculated vector</returns>
        public static Vector Hybrid(Vector input, double deadzone)
        {
            double magnitude = input.Magnitude();

            if (magnitude < deadzone)
            {
                return new Vector(0, 0);
            }

            Vector partial = ScaledRadial(input, deadzone);

            return Clip(SlopedScaledAxial(partial, deadzone), 1);
        }
    }
}
