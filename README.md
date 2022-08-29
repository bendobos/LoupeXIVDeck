![LoupeFFXIVDeckIcon](LoupeXIVDeck/Resources/Icons/icon_256.png)

# LoupeXIVDeck

A LoupeDeck plugin for [XIVDeck](https://github.com/KazWolfe/XIVDeck/blob/main/README.md) by @KazWolfe.

> ⚠️ This project currently is "unofficial" and the LoupeDeck pretends to be a Stream Deck in order to interface with the game.
> 
> Keep in mind that this means that this plugin could break anytime something changes inside XIVDeck.

## Features

LoupeXIVDeck supports the same commands as the Stream Deck plugin by @KazWolfe, with some additional adjustments using LoupeDeck's rotary encoders.

* __Text Command__: Run any slash command in-game. 
* __Execute Hotbar Slot__: Execute any hotbar slot.
* __Execute Action__: Execute a supported action (see XIVDeck documentation for more information on what an "action" is).
* __Run In-Game Macro__: Trigger any macro by ID.
* __Switch class__: Switch to a specific class directly.
* __*Rotary Encoder* | Adjust Audio Channel Volume__: Adjust the volume of any audio channel that is available in the game-settings.

## TODO / Known Issues

* Add proper error and exception handling. For example, the plugin might currently crash if IDs for actions are given that are out of bounds.
* Improve usability for Action and Class commands by using `GetClasses` and `GetActions`.
* Changing the WebSocket port is currently not supported, so make sure you use the XIVDeck default port. 
* Add some pictures and examples to this repo.

## Using the Plugin

1. Install the XIVDeck Game Plugin as explained on the [XIVDeck Github page](https://github.com/KazWolfe/XIVDeck/blob/main/README.md#installing-the-plugin).
2. Download the [latest release](https://github.com/bendobos/LoupeXIVDeck/releases) of this plugin.
3. In your LoupeDeck software, go to: `Click User Profile` -> `Plugins and Assets` -> `Install Plugin` and wait until the plugin is installed.
4. You are ready to go! Add commands from LoupeXIVDeck anywhere you need them.

