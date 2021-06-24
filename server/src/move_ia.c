/*
** EPITECH PROJECT, 2021
** zapp [WSL: Ubuntu]
** File description:
** move_ia
*/

#include "server.h"

void forward(server_t *s, map_t *m, __attribute__((unused))char *elem)
{
    if (s->players->dir == SOUTH) {
        if (s->players->y - 1 < 0)
            s->players->y = m->height - 1;
        else
            s->players->y -= 1;
    } else if (s->players->dir == NORTH) {
        if (s->players->y + 1 > m->height - 1)
            s->players->y = 0;
        else
            s->players->y += 1;
    }
    if (s->players->dir == EAST) {
        if (s->players->x + 1 > m->width - 1)
            s->players->x = 0;
        else
            s->players->x += 1;
    } else if (s->players->dir == WEST) {
        if (s->players->x - 1 < 0)
            s->players->x = m->width - 1;
        else
            s->players->x -= 1;
    }
    dprintf(s->gui_fd, "ppo %d %d %d %d\n", s->players->pos, s->players->x,
    s->players->y, s->players->dir);
    dprintf(s->players->fd, "ok\n");
}

void right(server_t *s, __attribute__((unused))map_t *m, __attribute__((unused))char *elem)
{
        switch (s->players->dir) {
        case EAST:
            s->players->dir = SOUTH;
            break;
        case NORTH:
            s->players->dir = EAST;
            break;
        case WEST:
            s->players->dir = NORTH;
            break;
        case SOUTH:
            s->players->dir = WEST;
            break;
        default:
            break;
    }
    dprintf(s->gui_fd, "ppo %d %d %d %d\n", s->players->pos, s->players->x,
    s->players->y, s->players->dir);
    dprintf(s->players->fd, "ok\n");
}

void left(server_t *s, __attribute__((unused))map_t *m, __attribute__((unused))char *elem)
{
    switch (s->players->dir) {
        case EAST:
            s->players->dir = NORTH;
            break;
        case NORTH:
            s->players->dir = WEST;
            break;
        case WEST:
            s->players->dir = SOUTH;
            break;
        case SOUTH:
            s->players->dir = EAST;
            break;
        default:
            break;
    }
    dprintf(s->gui_fd, "ppo %d %d %d %d\n", s->players->pos, s->players->x,
    s->players->y, s->players->dir);
    dprintf(s->players->fd, "ok\n");
}