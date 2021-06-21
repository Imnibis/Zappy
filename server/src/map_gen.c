/*
** EPITECH PROJECT, 2021
** zappy [WSL: Ubuntu]
** File description:
** map_gen
*/

#include "server.h"

void print_map(map_t *map)
{
    FILE *fd = fopen("map.txt", "w");

    go_prev(map);
    for (int i = 0; map->tiles->next != NULL; map->tiles = map->tiles->next, i++) {
        fprintf(fd, "X: %d | Y: %d : ", map->tiles->x, map->tiles->y);
        for (int j = 0; map->tiles->re[j] != -84; j++)
            fprintf(fd, "%d ", map->tiles->re[j]);
        fprintf(fd, "\n");
    }
}

void fill_rdm(map_t *map, int x, int y, int type)
{
    go_prev(map);
    for (; map->tiles->next != NULL; map->tiles = map->tiles->next) {
        if (x == map->tiles->x && y == map->tiles->y) {
            map->tiles->re[type]++;
        }
    }
}

void fill_map(map_t *map, int height, int width)
{
    int ttl = height * width;
    int re[] = {ttl * 0.5, ttl * 0.3, ttl * 0.15, ttl * 0.1,
    ttl * 0.1, ttl * 0.08, ttl * 0.05, -84};

    srand(time(NULL));
    for (int i = 0; re[i] != -84; i++) {
        while (re[i] != 0) {
            fill_rdm(map, rand() % width, rand() % height, i);
            re[i]--;
        }
    }

}

void init_ressources(map_t *map)
{
    for (int i = 0; i != 7; i++)
        map->tiles->re[i] = 0;
    map->tiles->re[7] = -84;
}

void map(map_t *map, int height, int width)
{
    map->tiles = malloc(sizeof(tile_t));
    map->tiles->prev = NULL;
    map->height = height;
    map->width = width;
    for (int i = 0; i != width * height; i++) {
        map->tiles->pos = i + 1;
        map->tiles->next = malloc(sizeof(tile_t));
        map->tiles->next->next = NULL;
        init_ressources(map);
        map->tiles->next->prev = map->tiles;
        map->tiles = map->tiles->next;
    }
    init_map(map, height, width);
    go_prev(map);
    fill_map(map, height, width);
    go_prev(map);
}