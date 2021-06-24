/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** client_handling
*/

#include "server.h"

void init_clients(server_t *s)
{
    s->players = malloc(sizeof(player_t));
    s->players->prev = NULL;

    for (int i = 0; i < s->cli_max; i++, s->players = s->players->next) {
        s->players->fd = 0;
        for (int j = 0; j != 7; j++)
            s->players->inventory[j] = 0;
        s->players->inventory[7] = -84;
        s->players->level = 1;
        s->players->type = ANY;
        s->players->dir = NORTH;
        s->players->next = malloc(sizeof(player_t));
        s->players->next->next = NULL;
        s->players->next->prev = s->players;
        s->players->pos = i;
    }
    go_previous(s);
}