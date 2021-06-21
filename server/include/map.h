/*
** EPITECH PROJECT, 2021
** zappy [WSL: Ubuntu]
** File description:
** zappy_server
*/

#ifndef ZAPPY_SERVER_H_
#define ZAPPY_SERVER_H_

#include <stdio.h>
#include <stdlib.h>
#include <time.h>

typedef struct tile_s tile_t;

typedef struct tile_s {
    int x;
    int y;
    int pos;
    int re[8];
    tile_t *next;
    tile_t *prev;
} tile_t;

typedef struct map_s {
    int height;
    int width;
    tile_t *tiles;
} map_t;

void go_prev(map_t *map);
#endif /* !ZAPPY_SERVER_H_ */
