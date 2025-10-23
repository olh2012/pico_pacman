# PICO Device Testing Guide

## Overview
This guide provides instructions for testing the VR Pac-Man game on PICO 3 and PICO 4 devices to ensure optimal performance and user experience.

## Prerequisites
1. PICO 3 or PICO 4 headset
2. PICO Developer account
3. Unity 2021.3 LTS or later
4. PICO Unity Integration SDK 3.3.0 or later
5. Android SDK & NDK (configured in Unity)
6. USB-C cable for device connection

## Device Setup

### PICO 4 Setup
1. Enable Developer Mode:
   - Go to Settings > About > Version (tap 8 times)
   - Go back to Settings > Developer Options
   - Enable "USB Debugging"
   - Enable "Unknown Sources"

2. Connect Device:
   - Connect PICO 4 to computer via USB-C cable
   - Accept any connection prompts on the headset

### PICO 3 Setup
1. Enable Developer Mode:
   - Go to Settings > About > Version (tap 8 times)
   - Go back to Settings > Developer Options
   - Enable "USB Debugging"
   - Enable "Unknown Sources"

2. Connect Device:
   - Connect PICO 3 to computer via USB-C cable
   - Accept any connection prompts on the headset

## Build Configuration

### Unity Player Settings
1. Open Unity Editor
2. Go to File > Build Settings
3. Select Android platform and Switch Platform
4. Go to Edit > Project Settings > Player
5. Configure the following settings:

#### Resolution and Presentation
- Default Orientation: Landscape Left
- Use 32-bit Display Buffer: Enabled
- Resizable Window: Disabled
- Visible in Background: Enabled
- Render Outside Safe Area: Enabled
- Use Player Log: Enabled

#### Android Settings
- Package Name: com.yourcompany.picopacman
- Version: 1.0
- Bundle Version Code: 1
- Minimum API Level: Android 7.0 (API level 24)
- Target API Level: Automatic (highest installed)
- Install Location: Automatic

#### XR Settings
- Virtual Reality Supported: Enabled
- Stereo Rendering Mode: Multi Pass
- Virtual Reality SDKs: OpenXR

#### Other Settings
- Color Space: Linear
- Target Architectures: ARM64
- Graphics APIs: 
  - First: OpenGL ES 3.0
  - Second: Vulkan (optional)
- Scripting Backend: IL2CPP
- Api Compatibility Level: .NET 4.x

### PICO Specific Settings
1. In Project Settings > XR Plugin Management:
   - Enable OpenXR
   - Configure PICO settings:
     - Device Type: PICO 4 (or PICO 3)
     - Render Quality: Performance (for 90fps target)

2. In Player Settings > Publishing Settings:
   - Check "PICO"
   - Set appropriate permissions:
     - Vibrate
     - Internet
     - Read External Storage
     - Write External Storage

## Performance Testing

### Frame Rate Monitoring
1. Launch the game on the device
2. Navigate through all menus and gameplay
3. Monitor frame rate using PICO Device Manager or log output
4. Ensure consistent 90fps during gameplay
5. Check for frame drops during:
   - Ghost movements
   - Particle effects
   - Audio playback
   - Menu transitions

### Comfort Testing
1. Test teleportation movement:
   - Verify smooth ray casting
   - Check teleport indicator visibility
   - Confirm teleport activation responsiveness
   - Test teleport landing accuracy

2. Test smooth movement:
   - Verify continuous movement comfort
   - Check tunnel vision effect
   - Test snap turn functionality
   - Evaluate motion sickness potential

3. Test hand presence:
   - Verify controller model visibility
   - Check hand tracking (if available)
   - Confirm model switching based on tracking state

### Audio Testing
1. Test spatial audio:
   - Verify ghost sounds are directional
   - Check audio distance attenuation
   - Confirm Doppler effect
   - Test audio occlusion

2. Test music and SFX:
   - Verify background music loops properly
   - Check sound effect clarity
   - Confirm volume settings work
   - Test audio mixing balance

### Visual Testing
1. Test UI visibility:
   - Check text readability at various distances
   - Verify button sizes are appropriate
   - Confirm color contrast in VR
   - Test UI positioning in player view

2. Test visual effects:
   - Verify particle effects performance
   - Check screen shake effects
   - Confirm color transitions
   - Test animation smoothness

## Gameplay Testing

### Core Mechanics
1. Player movement:
   - Test all movement modes
   - Verify collision detection
   - Check interaction radius
   - Confirm movement speed settings

2. Ghost AI:
   - Test all four ghost behaviors
   - Verify state transitions
   - Check collision detection
   - Confirm movement patterns

3. Pellet collection:
   - Test regular pellet collection
   - Verify power pellet effects
   - Check scoring system
   - Confirm level completion

### Menu Navigation
1. Main menu:
   - Test all button functions
   - Verify high score display
   - Check settings persistence
   - Confirm quit functionality

2. Settings menu:
   - Test all toggle options
   - Verify slider functionality
   - Confirm settings save
   - Check default values

3. Game over screen:
   - Verify score display
   - Test restart option
   - Confirm main menu return
   - Check high score updates

### Performance Benchmarks
1. Memory usage:
   - Monitor RAM consumption
   - Check for memory leaks
   - Verify garbage collection
   - Confirm object pooling

2. Battery consumption:
   - Monitor battery drain rate
   - Check for optimization opportunities
   - Verify thermal management
   - Confirm long play sessions

## Troubleshooting

### Common Issues
1. Low Frame Rate:
   - Reduce particle effect count
   - Optimize draw calls
   - Lower texture resolution
   - Simplify shaders

2. Audio Issues:
   - Check audio mixer settings
   - Verify spatializer configuration
   - Confirm audio source settings
   - Test on different devices

3. Controller Issues:
   - Verify input mapping
   - Check controller connection
   - Confirm trigger sensitivity
   - Test haptic feedback

4. UI Issues:
   - Adjust UI scale
   - Modify text sizes
   - Reposition elements
   - Check visibility angles

### Device Specific Notes
#### PICO 4
- Target resolution: 2160x2160 per eye
- Refresh rate: 90Hz
- Processor: Qualcomm Snapdragon XR2 Gen1
- RAM: 8GB

#### PICO 3
- Target resolution: 1832x1920 per eye
- Refresh rate: 90Hz
- Processor: Qualcomm Snapdragon XR2
- RAM: 6GB

## Test Results Documentation
Record the following information for each test session:
1. Device model and firmware version
2. Application version
3. Test date and duration
4. Performance metrics (fps, memory usage, battery drain)
5. Issues encountered and resolutions
6. User comfort feedback
7. Recommendations for improvement

## Conclusion
This testing guide ensures comprehensive evaluation of the VR Pac-Man game on PICO devices. Regular testing during development helps maintain optimal performance and user experience.