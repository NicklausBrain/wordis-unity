# Architecture

## Technology
- [Unity](https://unity.com/releases/2019-lts)
- [Block Puzzle Game Kit](https://assetstore.unity.com/packages/templates/packs/block-puzzle-game-kit-ready-to-publish-fun-mobile-game-162436)

## Design

### Note
The design below is speculative and might not fit Unity technical details.

### UML class diagram
```mermaid
classDiagram

class WordisApp{
    -WordisGame game
    -GameSettings settings
    +render()
}
WordisApp --> WordisGame

class GameObject{
    
}

class CharObject{
    +Char char
}
GameObject <|-- CharObject

class WordisGame{
    -TextGenerator textGenerator
    +GameSettings settings
    +GameObject[] matrix
    +Int score
    +next(Input input) WordisGame
}
WordisGame --> TextGenerator
WordisGame --> GameSettings
GameObject --* WordisGame

class GameSettings{
    +Int height
    +Int width
    +Int speed
}

class TextGenerator{
    +next() Char
}

class Input{
    +Direction direction
}

```
