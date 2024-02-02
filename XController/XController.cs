using System;
using System.Windows.Forms;
using SharpDX.XInput;

namespace XController
{
    /// <summary>
    /// User index of controller
    /// </summary>
    public enum UserIndex : byte
    {
        Any = byte.MaxValue,
        One = 0,
        Two = 1,
        Three = 2,
        Four = 3
    }

    /// <summary>
    /// Thumb value structure
    /// </summary>
    public struct ThumbValue
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
        /// Create a new instance of the ThumbValue structure
        /// </summary>
        public ThumbValue(double x, double y)
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
    }

    /// <summary>
    /// Trigger value structure
    /// </summary>
    public struct TriggerValue
    {
        /// <summary>
        /// Value of the left trigger
        /// </summary>
        public double Left;

        /// <summary>
        /// Value of the right trigger
        /// </summary>
        public double Right;

        /// <summary>
        /// Create a new instance of the TriggerValue structure
        /// </summary>
        /// <param name="left">Left trigger value</param>
        /// <param name="right">Right trigger value</param>
        public TriggerValue(double left, double right)
        {
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Gets a string representation of the object
        /// </summary>
        /// <returns>A string with Left and Right values</returns>
        public override string ToString()
        {
            return $"Left: {Left:0.000}, Right: {Right:0.000}";
        }
    }

    /// <summary>
    /// Event argument class for a Buttons pressed type event
    /// </summary>
    public class ButtonsPressedEventArgs : EventArgs
    {
        /// <summary>
        /// Previous button state
        /// </summary>
        public Buttons PreviousState { get; private set; }

        /// <summary>
        /// New button state
        /// </summary>
        public Buttons NewState { get; private set; }

        /// <summary>
        /// Buttons that are pressed only in the new state
        /// </summary>
        public Buttons Buttons { get; private set; }

        /// <summary>
        /// Create a new event argument object
        /// </summary>
        /// <param name="previousState">Previous button state</param>
        /// <param name="newState">New button state</param>
        public ButtonsPressedEventArgs(Buttons previousState, Buttons newState)
        {
            PreviousState = previousState;
            NewState = newState;
            Buttons = ButtonsTools.ButtonsInNewState(previousState, newState);
        }
    }

    /// <summary>
    /// Event argument class for a Buttons released type event
    /// </summary>
    public class ButtonsReleasedEventArgs : EventArgs
    {
        /// <summary>
        /// Previous button state
        /// </summary>
        public Buttons PreviousState { get; private set; }

        /// <summary>
        /// New button state
        /// </summary>
        public Buttons NewState { get; private set; }

        /// <summary>
        /// Buttons that were pressed only in the previous state
        /// </summary>
        public Buttons Buttons { get; private set; }

        /// <summary>
        /// Create a new event argument object
        /// </summary>
        /// <param name="previousState">Previous button state</param>
        /// <param name="newState">New button state</param>
        public ButtonsReleasedEventArgs(Buttons previousState, Buttons newState)
        {
            PreviousState = previousState;
            NewState = newState;
            Buttons = ButtonsTools.ButtonsInPreviousState(previousState, newState);
        }
    }


    /// <summary>
    /// XInput controller wrapper class that provides events
    /// </summary>
    public class Controller
    {
        #region "Private Variables"

        UserIndex _userIndex;

        SharpDX.XInput.Controller _controller = null;
        private readonly Timer _timer = null;
        int _pollingInterval = 10;
        bool _enablePolling = true;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Initializes a new instance of the XController class
        /// </summary>
        public Controller(UserIndex userIndex = UserIndex.One)
        {
            Buttons = new Buttons()
            {
                A = false,
                B = false,
                X = false,
                Y = false,
                LBumper = false,
                RBumper = false,
                LThumb = false,
                RThumb = false,
                Up = false,
                Down = false,
                Left = false,
                Right = false,
                Back = false,
                Start = false
            };

            _userIndex = userIndex;
            _controller = new SharpDX.XInput.Controller((SharpDX.XInput.UserIndex)userIndex);

            _timer = new Timer();
            _timer.Tick += PollingTimer_Tick;
            UpdateTimerSettings();
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// Gets the controller connection status
        /// </summary>
        public bool Connected
        {
            get; private set;
        }


        /// <summary>
        /// Gets or sets the controller user index
        /// </summary>
        public UserIndex UserIndex
        {
            get { return _userIndex; }
            set
            {
                _userIndex = value;
                _controller = new SharpDX.XInput.Controller((SharpDX.XInput.UserIndex)value);
                UpdateControllerValues();
            }
        }

        /// <summary>
        /// Gets or sets the controller polling interval
        /// Value cannot be less than 1
        /// </summary>
        public int PollingInterval
        {
            get { return _pollingInterval; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");

                _pollingInterval = value;
                UpdateTimerSettings();
            }
        }

        /// <summary>
        /// Gets or sets if polling is enabled
        /// </summary>
        public bool EnablePolling
        {
            get
            {
                return _enablePolling;
            }
            set
            {
                _enablePolling = value;
                UpdateTimerSettings();
            }
        }

        /// <summary>
        /// Gets the button press state
        /// </summary>
        public Buttons Buttons
        {
            get; private set;
        }

        /// <summary>
        /// Gets the value of the triggers
        /// Range: 0 to 1
        /// </summary>
        public TriggerValue Triggers
        {
            get; private set;
        } = new TriggerValue();

        /// <summary>
        /// Gets the value of the left thumb
        /// Range: 0 to 1
        /// </summary>
        public ThumbValue LeftThumb
        {
            get; private set;
        } = new ThumbValue();

        /// <summary>
        /// Gets the value of the right thumb
        /// Range: 0 to 1
        /// </summary>
        public ThumbValue RightThumb
        {
            get; private set;
        } = new ThumbValue();

        #endregion

        #region "Public Events"

        /// <summary>
        /// Event that triggers when a button is pressed
        /// </summary>
        public event EventHandler<ButtonsPressedEventArgs> ButtonsPressed;

        /// <summary>
        /// Event that triggers when a button is released
        /// </summary>
        public event EventHandler<ButtonsReleasedEventArgs> ButtonsReleased;

        /// <summary>
        /// Event that triggers when connection status changes
        /// </summary>
        public event EventHandler ConnectionStatusChanged;

        #endregion

        #region "Private Methods"


        /// <summary>
        /// Map a Gamepad object to a Buttons object
        /// </summary>
        /// <param name="gamepad">Gamepad state object</param>
        /// <returns>Buttons object</returns>
        private Buttons GamepadToButtons(Gamepad gamepad)
        {
            return new Buttons
            {
                A = (gamepad.Buttons & GamepadButtonFlags.A) != 0,
                B = (gamepad.Buttons & GamepadButtonFlags.B) != 0,
                X = (gamepad.Buttons & GamepadButtonFlags.X) != 0,
                Y = (gamepad.Buttons & GamepadButtonFlags.Y) != 0,

                LBumper = (gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0,
                RBumper = (gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0,
                LThumb = (gamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0,
                RThumb = (gamepad.Buttons & GamepadButtonFlags.RightThumb) != 0,

                Up = (gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0,
                Down = (gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0,
                Left = (gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0,
                Right = (gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0,

                Back = (gamepad.Buttons & GamepadButtonFlags.Back) != 0,
                Start = (gamepad.Buttons & GamepadButtonFlags.Start) != 0,
            };
        }

        /// <summary>
        /// Update controller values
        /// </summary>
        private void UpdateControllerValues()
        {
            if (Connected != _controller.IsConnected)
            {
                Connected = _controller.IsConnected;
                ConnectionStatusChanged?.Invoke(this, EventArgs.Empty);
            }

            if (!Connected)
            {
                return;
            }

            State state = _controller.GetState();
            Gamepad gamepad = state.Gamepad;

            UpdateButtonState(gamepad);
        }

        /// <summary>
        /// Update button state
        /// </summary>
        /// <param name="gamepad">Gamepad state</param>
        private void UpdateButtonState(Gamepad gamepad)
        {
            Buttons newState = GamepadToButtons(gamepad);

            int result = ButtonsTools.CompareButtonStates(Buttons, newState);

            if (result < 0)
            {
                ButtonsReleased?.Invoke(this, new ButtonsReleasedEventArgs(Buttons, newState));
            }
            else if (result > 0)
            {
                ButtonsPressed?.Invoke(this, new ButtonsPressedEventArgs(Buttons, newState));
            }

            Buttons = newState;
        }

        /// <summary>
        /// Update the timer object's settings
        /// </summary>
        private void UpdateTimerSettings()
        {
            _timer.Interval = _pollingInterval;
            _timer.Enabled = _enablePolling;
        }

        /// <summary>
        /// Polling timer tick event handler
        /// </summary>
        private void PollingTimer_Tick(object sender, EventArgs e)
        {
            UpdateControllerValues();
        }

        #endregion
    }
}
