/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** look
*/

#include "server.h"

void print_re(int re, int time, int fd)
{
    if (re == 0)
        for (int i = 0; i != time; i++)
            dprintf(fd, " food");
    if (re == 1)
        for (int i = 0; i != time; i++)
            dprintf(fd, " linemate");
    if (re == 2)
        for (int i = 0; i != time; i++)
            dprintf(fd, " deraumere");
    if (re == 3)
        for (int i = 0; i != time; i++)
            dprintf(fd, " sibur");
    if (re == 4)
        for (int i = 0; i != time; i++)
            dprintf(fd, " mendiane");
    if (re == 5)
        for (int i = 0; i != time; i++)
            dprintf(fd, " phiras");
    if (re == 6)
        for (int i = 0; i != time; i++)
            dprintf(fd, " thystame");
}

int get_pos_x(int x, map_t *m)
{
    if (x > (m->width - 1))
        x = x - m->width;
    if (x < 0)
        x = m->width + x;
    return x;
}

int get_pos_y(int y, map_t *m)
{
    if (y > (m->height - 1))
        y = y - m->height;
    if (y < 0)
        y = m->height + y;
    return y;
}

void get_tile(map_t *m, int x, int y, player_t *s)
{
    go_prev(m);
    for (; m->tiles->next != NULL; m->tiles = m->tiles->next) {
        if (m->tiles->x == x && m->tiles->y == y) {
            for (int i = 0; m->tiles->re[i] != -84; i++) {
                print_re(i, m->tiles->re[i], s->fd);
            }
        }
    }
}

void looked(map_t *m, player_t *s)
{
    int tmp = 3;
    int v = 0;
    int ctt = 3;
    int x = s->x;
    int y = s->y;

    for (int i = 1; i < s->level; i++, ctt += 3 + 2 * i);
    dprintf(s->fd, "[ player,");
    for (int i = 1; i != s->level + 1; i++) {
        if (s->dir == EAST) {
            for (int j = 1; j != ((tmp - 1) / 2) + 1; j++) {
                get_tile(m, get_pos_x(x - i, m), get_pos_y(y + j, m), s);
                v++;
                dprintf(s->fd, ",");
            }
            get_tile(m, get_pos_x(x - i, m), get_pos_y(y, m), s);
            dprintf(s->fd, ",");
            v++;
            for (int j = ((tmp - 1) / 2); j != 0; j--) {
                get_tile(m, get_pos_x(x - i, m), get_pos_y(y - j, m), s);
                v++;
                if (v != ctt)
                    dprintf(s->fd, ",");
            }
        }
        if (s->dir == EAST) {
            for (int j = ((tmp - 1) / 2); j != 0; j--) {
                get_tile(m, get_pos_x(x - i, m), get_pos_y(y - j, m), s);
                dprintf(s->fd, ",");
                v++;
            }
            get_tile(m, get_pos_x(x - i, m), get_pos_y(y, m), s);
            dprintf(s->fd, ",");
            v++;
            for (int j = 1; j != ((tmp - 1) / 2) + 1; j++) {
                get_tile(m, get_pos_x(x - i, m), get_pos_y(y + j, m), s);
                v++;
                if (v != ctt)
                    dprintf(s->fd, ",");
            }
        }
        if (s->dir == SOUTH) {
            for (int j = 1; j != ((tmp - 1) / 2) + 1; j++) {
                get_tile(m, get_pos_x(x + j, m), get_pos_y(y + i, m), s);
                v++;
                dprintf(s->fd, ",");
            }
            get_tile(m, get_pos_x(x, m), get_pos_y(y + i, m), s);
            dprintf(s->fd, ",");
            v++;
            for (int j = ((tmp - 1) / 2); j != 0; j--) {
                get_tile(m, get_pos_x(x - j, m), get_pos_y(y + i, m), s);
                v++;
                if (v != ctt)
                    dprintf(s->fd, ",");
            }
        }
        if (s->dir == NORTH) {
            for (int j = ((tmp - 1) / 2); j != 0; j--) {
                get_tile(m, get_pos_x(x - j, m), get_pos_y(y - i, m), s);
                dprintf(s->fd, ",");
                v++;
            }
            get_tile(m, get_pos_x(x, m), get_pos_y(y - i, m), s);
            dprintf(s->fd, ",");
            v++;
            for (int j = 1; j != ((tmp - 1) / 2) + 1; j++) {
                get_tile(m, get_pos_x(x + j, m), get_pos_y(y - i, m), s);
                v++;
                if (v != ctt)
                    dprintf(s->fd, ",");
            }
        }
        tmp += 2;
    }
    dprintf(s->fd, " ]\n");
}

void look(server_t *s, map_t *m)
{
    looked(m, s->acs->player);
}