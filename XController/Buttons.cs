using System.Text;

namespace XController
{
    /// <summary>
    /// Button state structure
    /// </summary>
    public struct Buttons
    {
        public bool A, B, X, Y;
        public bool LBumper, RBumper;
        public bool LThumb, RThumb;
        public bool Up, Down, Left, Right;
        public bool Back, Start;

        /// <summary>
        /// Gets a string representation of this object
        /// </summary>
        /// <returns>A string with pressed button names separated by a comma</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            AppendIfTrue(stringBuilder, A, nameof(A));
            AppendIfTrue(stringBuilder, B, nameof(B));
            AppendIfTrue(stringBuilder, X, nameof(X));
            AppendIfTrue(stringBuilder, Y, nameof(Y));
            AppendIfTrue(stringBuilder, LBumper, nameof(LBumper));
            AppendIfTrue(stringBuilder, RBumper, nameof(RBumper));
            AppendIfTrue(stringBuilder, LThumb, nameof(LThumb));
            AppendIfTrue(stringBuilder, RThumb, nameof(RThumb));
            AppendIfTrue(stringBuilder, Up, nameof(Up));
            AppendIfTrue(stringBuilder, Down, nameof(Down));
            AppendIfTrue(stringBuilder, Left, nameof(Left));
            AppendIfTrue(stringBuilder, Right, nameof(Right));
            AppendIfTrue(stringBuilder, Back, nameof(Back));
            AppendIfTrue(stringBuilder, Start, nameof(Start));

            // Remove the trailing ", " if any
            if (stringBuilder.Length >= 2)
                stringBuilder.Length -= 2;

            return stringBuilder.ToString();
        }

        private void AppendIfTrue(StringBuilder stringBuilder, bool value, string name)
        {
            if (value)
                stringBuilder.Append(name).Append(", ");
        }
    }

    /// <summary>
    /// Contains utility functions for the Buttons structure
    /// </summary>
    public static class ButtonsTools
    {

        /// <summary>
        /// Compares two button states
        /// </summary>
        /// <param name="prev">Previous button state</param>
        /// <param name="next">Next button state</param>
        /// <returns>-1 if button state falls, 1 if button state rises, 0 if nothing changes</returns>
        public static int CompareButtonStates(Buttons prev, Buttons next)
        {
            // Button state rise
            if ((!prev.A && next.A) ||
                (!prev.B && next.B) ||
                (!prev.X && next.X) ||
                (!prev.Y && next.Y) ||

                (!prev.LBumper && next.LBumper) ||
                (!prev.RBumper && next.RBumper) ||
                (!prev.LThumb && next.LThumb) ||
                (!prev.RThumb && next.RThumb) ||

                (!prev.Up && next.Up) ||
                (!prev.Down && next.Down) ||
                (!prev.Left && next.Left) ||
                (!prev.Right && next.Right) ||

                (!prev.Back && next.Back) ||
                (!prev.Start && next.Start))
                return 1;

            // Button state fall
            if ((prev.A && !next.A) ||
                (prev.B && !next.B) ||
                (prev.X && !next.X) ||
                (prev.Y && !next.Y) ||

                (prev.LBumper && !next.LBumper) ||
                (prev.RBumper && !next.RBumper) ||
                (prev.LThumb && !next.LThumb) ||
                (prev.RThumb && !next.RThumb) ||

                (prev.Up && !next.Up) ||
                (prev.Down && !next.Down) ||
                (prev.Left && !next.Left) ||
                (prev.Right && !next.Right) ||

                (prev.Back && !next.Back) ||
                (prev.Start && !next.Start))
                return -1;

            // No change
            return 0;
        }

        /// <summary>
        /// Gets only the buttons that have changed from not pressed to being pressed
        /// </summary>
        /// <param name="prev">Previous button state</param>
        /// <param name="next">Next button state</param>
        /// <returns>A new Buttons object</returns>
        public static Buttons ButtonsInNewState(Buttons prev, Buttons next)
        {
            return new Buttons()
            {
                A = !prev.A && next.A,
                B = !prev.B && next.B,
                X = !prev.X && next.X,
                Y = !prev.Y && next.Y,

                LBumper = !prev.LBumper && next.LBumper,
                RBumper = !prev.RBumper && next.RBumper,
                LThumb = !prev.LThumb && next.LThumb,
                RThumb = !prev.RThumb && next.RThumb,

                Up = !prev.Up && next.Up,
                Down = !prev.Down && next.Down,
                Left = !prev.Left && next.Left,
                Right = !prev.Right && next.Right,

                Back = !prev.Back && next.Back,
                Start = !prev.Start && next.Start
            };
        }

        /// <summary>
        /// Gets only the buttons that have changed from being pressed to not being pressed
        /// </summary>
        /// <param name="prev">Previous button state</param>
        /// <param name="next">Next button state</param>
        /// <returns>A new Buttons object</returns>
        public static Buttons ButtonsInPreviousState(Buttons prev, Buttons next)
        {
            return new Buttons()
            {
                A = prev.A && !next.A,
                B = prev.B && !next.B,
                X = prev.X && !next.X,
                Y = prev.Y && !next.Y,

                LBumper = prev.LBumper && !next.LBumper,
                RBumper = prev.RBumper && !next.RBumper,
                LThumb = prev.LThumb && !next.LThumb,
                RThumb = prev.RThumb && !next.RThumb,

                Up = prev.Up && !next.Up,
                Down = prev.Down && !next.Down,
                Left = prev.Left && !next.Left,
                Right = prev.Right && !next.Right,

                Back = prev.Back && !next.Back,
                Start = prev.Start && !next.Start
            };
        }
    }

}
