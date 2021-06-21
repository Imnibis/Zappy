/*
** EPITECH PROJECT, 2021
** zappy [WSL: Ubuntu]
** File description:
** map_utils
*/

#include "map.h"

void init_map(map_t *map, int height, int width)
{
    go_prev(map);
    for (int i = 0; i != height; i++) {
        for (int j = 0; j != width; j++) {
            map->tiles->x = j;
            map->tiles->y = i;
            map->tiles = map->tiles->next;
        }
    }
}

void go_prev(map_t *map)
{
    for (; map->tiles->prev != NULL; map->tiles = map->tiles->prev);
}

void free_map(map_t *map)
{
    for (; map->tiles->prev != NULL; map->tiles = map->tiles->prev)
        free(map->tiles->next);
    free(map->tiles);
    free(map);
}