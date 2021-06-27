/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** chained_utils
*/

#include "server.h"

void go_previous(server_t *s)
{
    for (; s->players->prev != NULL; s->players = s->players->prev);
}

void go_pos(server_t *s, int pos)
{
    go_previous(s);
    for (; s->players->prev != NULL; s->players = s->players->prev) {
        if (s->players->pos == pos)
            break;
    }
}