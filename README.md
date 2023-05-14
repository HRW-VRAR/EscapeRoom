# EscapeRoom | [GitHub](https://github.com/HRW-VRAR/EscapeRoom)

Scene: Assets/Sci-Fi Styled Modular Pack/Example scenes/outpost on desert.unity

Scripts:
- Assets/RDL/Security_Pack/Scripts/scr_camera.cs
  - Attached to the swivel of the CCTV cameras, replacing existing script
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
- Assets/HotWireScript.cs
  - Attached to the wire
  - Has options to change how long it takes for the wire to overheat
  - Checks for collision with handle and indicates it visually
  - Resets the hot wire when the handle and the wire have been colliding for too long
- Assets/HotWireFinishScript.cs
  - Attached to the finish trigger
  - Notifies the HotWireScript when the handle reaches the end of the wire

Prefabs:
- Assets/KeyPad
  - Keypad that allows the user to enter a 4-digit code
- Assets/Hot_Wire
  - Hot wire puzzle/game

External assets:
- FREE PBR Security Camera
  - CCTV camera model by Red Dot Lab
  - https://assetstore.unity.com/packages/3d/props/electronics/free-pbr-security-camera-70061
  - Location: Assets/RDL/Security_Pack
- Character Pack: Free Sample
  - Character model by Supercyan
  - https://assetstore.unity.com/packages/3d/characters/humanoids/character-pack-free-sample-79870
  - Location: Assets/Supercyan Character Pack Free Sample
- Sci-Fi Styled Modular Pack
  - Futuristic assets and scenes by karboosx
  - https://assetstore.unity.com/packages/3d/environments/sci-fi/sci-fi-styled-modular-pack-82913
  - Location: Assets/Sci-Fi Styled Modular Pack
- Oculus Integration
  - Oculus integration by Oculus
  - NOTE: Only import Oculus/SampleFramework/Core/CustomHands, the rest is not needed
  - https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022
  - Location: Assets/Oculus/SampleFramework/Core/CustomHands

Packages:
- TextMeshPro
  - Used to display text (keypad display and buttons, CCTV exit button)
- XR Interaction Toolkit
  - Used for VR interactions (teleportation, rotation, etc.)
