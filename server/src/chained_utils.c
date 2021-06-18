/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** chained_utils
*/

#include "server.h"

void go_previous(server_t *s)
{
    for (; s->players->prev != NULL; s->players = s->players->prev);
}