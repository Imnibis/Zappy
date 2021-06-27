/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** incantation
*/

#include "server.h"

void incantation(server_t *s)
{
    go_previous(s);
    for (; s->players->next != NULL; s->players = s->players->next)
        if (s->players->fd == s->acs->player->fd)
            break;
    dprintf(s->players->fd, "ko\n");
    go_previous(s);
}