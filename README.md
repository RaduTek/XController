# XController

Helper library for using XInput controllers in a .NET application in a way similar to a keyboard and a mouse.

The `Controller` object contains events that trigger when the user performs an input on the controller.

The library also implements deadzones for the thumb sticks. Based on code from https://github.com/Minimuino/thumbstick-deadzones.

## Requirements

-   .NET Framework 4.8
-   SharpDX 4.2.0 - [NuGet](https://www.nuget.org/packages/SharpDX.XInput)

## Example

```C#
// Initializes a new Controller object, with UserIndex = Any (by default)
Controller controller = new Controller();

// Prints out the pressed buttons
Console.WriteLine(controller.Buttons.ToString());

// Prints out the current thumb stick values
// in format X: 0.000, Y: 0.000
Console.WriteLine(controller.LeftThumb.ToString());
Console.WriteLine(controller.RightThumb.ToString());
```

## Properties

### Controller

-   `Connected`: bool

    -   Gets the controller connection status

-   `UserIndex`: UserIndex

    -   Gets or sets the controller user index

### Polling

-   `PollingInterval`: int

    -   Gets or sets the controller polling interval
    -   Value cannot be less than 1

-   `EnablePolling`: bool

    -   Gets or sets if polling is enabled

### Buttons and triggers

-   `Buttons`: Buttons

    -   Gets the button press state

-   `Triggers`: Triggers

    -   Gets the value of the triggers
    -   Range: 0 to 1

### Thumbsticks

-   `LeftThumb`: Vector

    -   Gets the value of the left thumbstick
    -   Range: -1 to 1

-   `LeftDeadzone`: double

    -   Gets or sets the dead zone ratio of the left thumbstick
    -   Range: 0 to 1

-   `RightThumb`: Vector

    -   Gets the value of the left thumbstick
    -   Range: -1 to 1

-   `RightDeadzone`: double

    -   Gets or sets the dead zone ratio of the left thumbstick
    -   Range: 0 to 1

## Events

### Controller

-   `ConnectionStatusChanged`: EventArgs.Empty

    -   Event that triggers when connection status changes

### Buttons

-   `ButtonsPressed`: ButtonsPressedEventArgs

    -   Event that triggers when a button is pressed

-   `ButtonsReleased`: ButtonsReleasedEventArgs

    -   Event that triggers when a button is released

### Triggers

-   `TriggersMoved`: EventArgs.Empty

    -   Event that triggers when the trigger levers are moved

### Thumbsticks

-   `LeftThumbMoved`: EventArgs.Empty

    -   Event that triggers when the left thumb stick is moved

-   `RightThumbMoved`: EventArgs.Empty

    -   Event that triggers when the right thumb stick is moved

## Event arguments

The following information applies for both `ButtonsPressedEventArgs` and `ButtonsReleasedEventArgs`.

-   `PreviousState`

    -   The state before the change that triggered the event

-   `NewState`

    -   The state that triggered the event

-   `Buttons`
    -   Buttons that changed between the previous and new state (that were pressed or released)
    -   Use this to handle cases where a button is pressed/released while another button is also pressed

## UserIndex enum

The `UserIndex` enum defines the user index to be chosen for the controller through the XInput API.

`XController.Controller.UserIndex` is equivalent to `SharpDX.XInput.UserIndex` and a variable of one type can be directly type casted into the other type.

From [XController.cs](XController/XController.cs):

```C#
public Controller(UserIndex userIndex = UserIndex.Any)
{
    ...
    _controller = new SharpDX.XInput.Controller((SharpDX.XInput.UserIndex)userIndex);
    ...
}
```
