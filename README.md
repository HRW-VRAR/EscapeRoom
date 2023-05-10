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
