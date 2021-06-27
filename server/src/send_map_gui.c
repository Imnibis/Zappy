/*
** EPITECH PROJECT, 2021
** zapp [WSL: Ubuntu]
** File description:
** send_map_gui
*/

#include "server.h"

void send_map_gui(server_t *s, map_t *m, char **teams)
{
    dprintf(s->gui_fd, "msz %d %d\n", m->width, m->height);
    dprintf(s->gui_fd, "sgt %d\n", 100);
    for (; m->tiles->next != NULL; m->tiles = m->tiles->next) {
        dprintf(s->gui_fd, "bct %d %d", m->tiles->x, m->tiles->y);
        for (int i = 0; m->tiles->re[i] != -84; i++)
            dprintf(s->gui_fd, " %d", m->tiles->re[i]);
        dprintf(s->gui_fd, "\n");
    }
    go_prev(m);
    dprintf(s->gui_fd, "tna");
    for (int i = 0; teams[i]; i++) {
        dprintf(s->players->fd, " %s", teams[i]);
    dprintf(s->gui_fd, "\n");
    }
}
