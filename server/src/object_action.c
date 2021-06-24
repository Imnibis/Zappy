/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** object_action
*/

#include "server.h"

void take(server_t *s, map_t *m, int re)
{
    go_prev(m);
    for (; m->tiles->next != NULL; m->tiles = m->tiles->next) {
        if (m->tiles->x == s->players->x && m->tiles->y) {
            if (m->tiles->re[re] > 0) {
                m->tiles->re[re] -= 1;
                s->players->inventory[re] += 1;
                dprintf(s->players->fd, "ok\n");
                dprintf(s->gui_fd, "pgt %d %d\n", s->players->pos, re);
            } else
                dprintf(s->players->fd, "ko\n");
        }
    }
    go_prev(m);
}

void set(__attribute__((unused))server_t *s, __attribute__((unused)) map_t *m, __attribute__((unused))int re)
{
    go_prev(m);
    for (; m->tiles->next != NULL; m->tiles = m->tiles->next) {
        if (m->tiles->x == s->players->x && m->tiles->y) {
            if (s->players->inventory[re] > 0) {
                m->tiles->re[re] += 1;
                s->players->inventory[re] -= 1;
                dprintf(s->players->fd, "ok\n");
                dprintf(s->gui_fd, "pdr %d %d\n", s->players->pos, re);
            } else
                dprintf(s->players->fd, "ko\n");
        }
    }
    go_prev(m);
}
