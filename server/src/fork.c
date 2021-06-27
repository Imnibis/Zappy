/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** fork
*/

#include "server.h"

void forked(server_t *s, __attribute__((unused)) map_t *m, __attribute__((unused))char *elem)
{
    dprintf(s->players->fd, "ok\n");
}