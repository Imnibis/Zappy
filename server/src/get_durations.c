/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** get_durations
*/

#include "server.h"

int get_durations(duration_t dur)
{
    if (dur == CONNECT || dur == DEATH)
        return 0;
    if (dur == INVENTORY)
        return 1;
    if (dur == FORWARD || dur == LEFT || dur == LOOK
    || dur == EJECT || dur == TAKE || dur == SET || dur == RIGHT)
        return 7;
    if (dur == REFILL)
        return 20;
    if (dur == FORK)
        return 42;
    if (dur == FOOD)
        return 126;
    if (dur == INCANTATION)
        return 300;
    return 0;
}