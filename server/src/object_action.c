/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** object_action
*/

#include "server.h"

int get_elem(char *elem)
{
    if (strcmp(elem, "food") == 0)
        return 0;
    if (strcmp(elem, "linemate") == 0)
        return 1;
    if (strcmp(elem, "deraumere") == 0)
        return 2;
    if (strcmp(elem, "sibur") == 0)
        return 3;
    if (strcmp(elem, "mendiane") == 0)
        return 4;
    if (strcmp(elem, "phiras") == 0)
        return 5;
    if (strcmp(elem, "thystame") == 0)
        return 6;
    return 0;
}

void take(server_t *s, map_t *m)
{
    int re = get_elem(s->acs->elem);

    go_previous(s);
    for (; s->players->next != NULL; s->players = s->players->next)
        if (s->players->fd == s->acs->player->fd)
            break;
    go_prev(m);
    for (; m->tiles->next != NULL; m->tiles = m->tiles->next) {
        if (m->tiles->x == s->players->x && m->tiles->y) {
            if (m->tiles->re[re] > 0) {
                m->tiles->re[re] -= 1;
                s->players->inventory[re] += 1;
                dprintf(s->players->fd, "ok\n");
                dprintf(s->gui_fd, "pgt %d %d\n", s->players->pos, re);
                dprintf(s->gui_fd, "bct %d %d", m->tiles->x, m->tiles->y);
                for (int i = 0; i != 7; i++)
                    dprintf(s->gui_fd, " %d", m->tiles->re[i]);
                dprintf(s->gui_fd, "\n");
            } else
                dprintf(s->players->fd, "ko\n");
            break;
        }
    }
    go_prev(m);
}

void set(server_t *s, map_t *m)
{
    int re = get_elem(s->acs->elem);

   go_previous(s);
    for (; s->players->next != NULL; s->players = s->players->next)
        if (s->players->fd == s->acs->player->fd)
            break;
    go_prev(m);
    for (; m->tiles->next != NULL; m->tiles = m->tiles->next) {
        if (m->tiles->x == s->players->x && m->tiles->y) {
            if (s->players->inventory[re] > 0) {
                m->tiles->re[re] += 1;
                s->players->inventory[re] -= 1;
                dprintf(s->players->fd, "ok\n");
                dprintf(s->gui_fd, "pdr %d %d\n", s->players->pos, re);
                dprintf(s->gui_fd, "bct %d %d", m->tiles->x, m->tiles->y);
                for (int i = 0; i != 7; i++)
                    dprintf(s->gui_fd, " %d", m->tiles->re[i]);
                dprintf(s->gui_fd, "\n");
            } else
                dprintf(s->players->fd, "ko\n");
            break;
        }
    }
    go_prev(m);
}
