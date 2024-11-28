# Changelog
All notable additions, removals, changes and fixes will be documented on this page.  
The format is based on [this](https://keepachangelog.com/en/1.0.0/) template.

## [v0.0.5] - name - 00-00-2024

### Added
Existing Scripts:

- TabController
	- New `OnTabChanged` event, which passes the index of the new tab as the parameter.
		- This should make it easier to work with a `TabController`.

- LookAt2D
	- New `GetLookRotation` function overwrite which takes 2 `Vector3` parameters.
		- This makes the `GetLookRotation` function more flexible for contexts that don't include a `Transform`.

### Changed
General:

- Renamed `UI Types` assembly to `UI Data` in order to be more consistent. 

Scripts:

- SadButton
	- `OnClick` event no longer passed a reference to the `SadButton`.

- TabController
	- `CurrentIndex` is no longer *private*, instead it is a *public* readonly field.
		- This, along with the new `OnTabChanged` event, should make it much easier to work with a `TabController`.

### Removed

### Fixed
Scripts:

- TabController
	- No longer relies on `SadButton` to pass a reference of itself in the `OnClick` event.

## [v0.0.4] - UI Update - 14-10-2024
This update focuses on adding 2 new UI elements.  
Additionally, a lot of changes were made to the internal project structure.  
A project where this package will be used has started development, so more updates can be expected soon.

### Added
New Scripts:

- Sad Button
	- A custom implementation for a UI button.
	- It sports (almost) all the features found in the default Unity button and more.
	- Comes with a custom inspector for a smoother experience.

- Tab Controller
	- A script for creating UI panels controlled by tabs.

New Internal Scripts:

- ButtonTransition
	- Enum containing different transition types for the Sad Button.

- ButtonVisualData
	- Data struct containing visual information.
	- Used internally by the button to store information per state.

### Changed

- Overhauled internal project structure.
	- This overhaul focused on 2 elements:
		1. Allow for future work on the package.
		2. Remove unnecessary namespaces.

- Improved performance of **WaitForInstance** in the *Singleton* class.

## [v0.0.3] - Internal Project Restructure - 19-09-2024
This update restructures the project to be more future proof.  
It also addresses the package not working as intended with the legacy input manager.

### Changed

- Overhauled internal project structure.

### Fixed

- Package throwing an error when using the new input system.
  

## [v0.0.2] - New Input System Fix - 19-07-2024
This update addresses the package not working as intended with the new input system.

### Fixed

- Package throwing an error when using the new input system.
  

## [v0.0.1] - Essentials Update - 17-07-2024
This update marks the private release of the Sad Utils package.  
Most of the content in this update is (largely) taken from previous projects.

### Added
New Scripts:

- LookAt2D
	- Allows for easy look at calculations in 2D space.

- Singleton
	- A template for the singleton pattern.

- UnityDictionary
	- A dictionary which works with the Unity Editor.

New Internal Scripts:

- IMouseInputProvider
	- Internal interface used by the Legacy and New Input Provider classes.

- InputProviderFactory
	- Allows for seamless input fetching regardless of which input system is being used.

- LegacyInputProvider
	- Fetches input when the Legacy Input Manager is used.

- NewInputProvider
	- Fetches input when the new Input System is used.