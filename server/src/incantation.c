/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** incantation
*/

#include "server.h"

void incantation(__attribute__((unused))server_t *s, __attribute__((unused)) map_t *m, __attribute__((unused))int re)
{
    dprintf(s->players->fd, "ko\n");
}