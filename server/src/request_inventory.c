/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** request_inventory
*/

#include "server.h"

void request_inventory(server_t *s, __attribute__((unused))map_t *m, __attribute__((unused))char *elem)
{
    dprintf(s->players->fd, "[ food %d, linemate %d, deraumere %d, sibur %d, mendiane %d, phiras %d, thystame %d ]\n",
    s->players->inventory[0], s->players->inventory[1],
    s->players->inventory[2], s->players->inventory[3],
    s->players->inventory[4], s->players->inventory[5],
    s->players->inventory[6]);
}

void broadcast(__attribute__((unused))server_t *s, __attribute__((unused)) map_t *m, __attribute__((unused))char *elem)
{
    dprintf(s->players->fd, "ko\n");
}

void connect_nbr(__attribute__((unused))server_t *s, __attribute__((unused)) map_t *m, __attribute__((unused))char *elem)
{
    dprintf(s->players->fd, "%d\n", (s->cli_max - s->nb_cli) + 1);
}