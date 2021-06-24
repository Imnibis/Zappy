/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** look
*/

#include "server.h"

void print_re(int re, int time)
{
    if (re == 0)
        for (int i = 0; i!= time; i++)
            printf(" food");
    if (re == 1)
        for (int i = 0; i!= time; i++)
            printf(" linemate");
    if (re == 2)
        for (int i = 0; i!= time; i++)
            printf(" deraumere");
    if (re == 3)
        for (int i = 0; i!= time; i++)
            printf(" sibur");
    if (re == 4)
        for (int i = 0; i!= time; i++)
            printf(" mendiane");
    if (re == 5)
        for (int i = 0; i!= time; i++)
            printf(" phiras");
    if (re == 6)
        for (int i = 0; i!= time; i++)
            printf(" thystame");
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

void get_tile(map_t *m, int x, int y)
{
    go_prev(m);
    for (; m->tiles->next != NULL; m->tiles = m->tiles->next) {
        if (m->tiles->x == x && m->tiles->y == y) {
            for (int i = 0; m->tiles->re[i] != -84; i++) {
                print_re( i, m->tiles->re[i]);
            }
        }
    }
}

void looked(map_t *m, int level, int x, int y, dir_t dir)
{
    int tmp = 3;
    int v = 0;
    int ctt = 3;

    for (int i = 1; i < level; i++, ctt += 3 + 2 * i);
    printf("[player,");
    if (dir == SOUTH) {
        for (int i = 1; i != level + 1; i++) {
            for (int j = 1; j != ((tmp - 1) / 2) + 1; j++) {
                get_tile(m, get_pos_x(x + j, m), get_pos_y(y + i, m));
                v++;
                printf(",");
            }
            get_tile(m, get_pos_x(x, m), get_pos_y(y + i, m));
            printf(",");
            v++;
            for (int j = ((tmp - 1) / 2); j != 0; j--) {
                get_tile(m, get_pos_x(x - j, m), get_pos_y( + i, m));
                v++;
                if (v != ctt)
                    printf(",");
            }
        }
    }
    if (dir == NORTH) {
        for (int i = 1; i != level + 1; i++) {
            for (int j = ((tmp - 1) / 2); j != 0; j--) {
                get_tile(m, get_pos_x(x - j, m), get_pos_y( - i, m));
                printf(",");
                v++;
            }
            get_tile(m, get_pos_x(x, m), get_pos_y(y - i, m));
            printf(",");
            v++;
            for (int j = 1; j != ((tmp - 1) / 2) + 1; j++) {
                    get_tile(m, get_pos_x(x + j, m), get_pos_y(y - i, m));
                v++;
                if (v != ctt)
                    printf(",");
            }
            tmp +=2;
        }
    }
    printf("]\n");
}

void look(__attribute__((unused))server_t *s, __attribute__((unused)) map_t *m, __attribute__((unused))int re)
{
    dprintf(s->players->fd, "[player,,,]\n");
}