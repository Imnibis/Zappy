/*
** EPITECH PROJECT, 2021
** zapp [WSL: Ubuntu]
** File description:
** player
*/

#ifndef PLAYER_H_
#define PLAYER_H_

typedef struct player_s {
    int x;
    int y;
    int level;
    bool incantation;
    int inventory[7];
} players_t;
#endif /* !PLAYER_H_ */
