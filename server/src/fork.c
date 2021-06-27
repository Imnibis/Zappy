/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** fork
*/

#include "server.h"

void forked(server_t *s)
{
    go_previous(s);
    for (; s->players->next != NULL; s->players = s->players->next)
        if (s->players->fd == s->acs->player->fd)
            break;
    dprintf(s->gui_fd, "enw 1 %d %d %d\n", s->players->pos, s->players->x, s->players->y);
    dprintf(s->players->fd, "ok\n");
    go_previous(s);
}