# Algorithm
## Rules
1. Random characters fall from the top;
2. If 3+ character form the genuine word*;
3. Then these characters disappear and the score is increased;
4. Rest of the characters fall to the vacant slots;

*Valid word alignments:
- right to left;
- top to down;
- if words intersect then they all count;

## Example

```
# 1st state

[w][ ][b][ ][ ][ ][ ]

[o][r][ ][i][t][ ][ ]

[r][z][i][t][c][g][h]

[r][z][t][t][c][g][h]
```

```
# 2nd state

[w][ ][ ][ ][ ][ ][ ]

[o][r][b][i][t][ ][ ]

[r][z][i][t][c][g][h]

[r][z][t][t][c][g][h]
```

```
# 3rd state

[w][ ][ ][ ][ ][ ][ ]

[-][-][-][-][-][ ][ ]

[r][z][-][t][c][g][h]

[r][z][-][t][c][g][h]
```

```
# 4th state

[ ][ ][ ][ ][ ][ ][ ]

[w][ ][ ][ ][ ][ ][ ]

[r][z][ ][t][c][g][h]

[r][z][ ][t][c][g][h]
```
