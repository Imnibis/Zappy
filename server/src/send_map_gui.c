/*
** EPITECH PROJECT, 2021
** zapp [WSL: Ubuntu]
** File description:
** send_map_gui
*/

#include "server.h"

void send_map_gui(server_t *s, map_t *m)
{
    dprintf(s->players->fd, "msz %d %d\n", m->width, m->height);
    dprintf(s->players->fd, "sgt %d\n", 100);
    for (; m->tiles->next != NULL; m->tiles = m->tiles->next) {
        dprintf(s->players->fd, "bct %d %d", m->tiles->x, m->tiles->y);
        for (int i = 0; m->tiles->re[i] != -84; i++)
            dprintf(s->players->fd, " %d", m->tiles->re[i]);
        dprintf(s->players->fd, "\n");
    }
    go_prev(m);
    dprintf(s->players->fd, "tna bite\n");
}
