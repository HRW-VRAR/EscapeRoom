# EscapeRoom

Scripts:
- Assets/RDL/Security_Pack/Scripts/scr_camera.cs
  - Attached to the swivel of the CCTV cameras
  - Allows the CCTV camera POV to be activated by calling activateCamera()
  - Optionally allows automatic rotation (disabled because this would cause issues in VR)
- Assets/CharacterScript.cs
  - Attached to the container containing the XR origin and the character model
  - Rotates and moves the character model based on the XR origin
  - Allows the user to switch back to the character model's POV by calling activateCamera()
  - Shows and hides the "EXIT" button UI
  - Enables/disables camera and interaction layers and the controller's snap turn provider when needed
- Assets/XRControllerCustom.cs
  - Attached to the XR controller objects
  - Subclass used to override the controller's vertical position when in a CCTV camera
- Assets/Keypad.cs
  - Attached to the keypad
  - Keeps track of the keypad input
  - Updates and clears the keypad input: AppendString(string) and ClearText()
  - Checks the keypad input against the configured code when the user presses enter: Enter()
    - Invokes the relevant event and displays the verification status by changing the background color of the display

Prefabs:
- Assets/KeyPad
  - Keypad that allows the user to enter a 4-digit code
- Assets/Hot_Wire
  - Hot wire puzzle/game
