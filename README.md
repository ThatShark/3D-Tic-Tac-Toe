3D OOXX
===

>A simple 3D tic-tac-toc, it's easy... maybe?

Main rule
---
Player who gets more **Victory Conditions**  at the same time wins the game.

Victory Condition: Align three of the same pattern in a straight line, horizontal line, or diagonal line.

Game Progress
---
Two players take turns, with Player O starting. In each turn, a player can choose to place a block, use a skill, or surrender. 

- Placing a block means placing one of their blocks in any empty space at the top(Press Up Key) or bottom(Press Down Key) of a layer. All blocks are affected by gravity and will automatically fall down. A maximum of three layers can be stacked. 

- There are three skills in total, and only one skill can be used per turn. Each skill can only be used once.


Skill
---
- Triangle:
When using the skill, instead of placing their own block, a player places a triangle.

  - When placed from the top, the triangle destroys the items directly below it.

  - When placed from the bottom, all blocks above the triangle will automatically move up by one layer. If there are already three layers above, the third layer item will be moved to the fourth layer, ignoring the maximum layer limit.

==Caution: Triangle can't destroy another triangle. Besides, no blocks can be put under a triangle.==

- Spin:
When using the skill, all items can rotate 90 degrees in one of the four directions (front, back, left, or right). After the rotation, all items will still be affected by gravity and the triangle's effects.

==Caution: Blocks that were originally moved beyond the third layer due to the triangle will disappear after the rotation.==

- UpsideDown:
When using the skill, all items will be flipped upside down. After the flip, the effects will be the same as the "spin" skill.


Shortcut Keys
---

| Keys | Function |
| :-- | :-- |
| 1, 2, 3 | Show one row |
| 4, 5, 6 | Show one column |
| 7, 8, 9 | Show one layer |
| 0 | Show all |
| W | Triangle |
| A | Spin |
| S | UpsideDoown |
| Q | Surrender |
| Enter | Confirm |
| Esc | Back |
