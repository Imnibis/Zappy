/*
** EPITECH PROJECT, 2021
** zapp [WSL: Ubuntu]
** File description:
** teams_exists
*/

#include "server.h"

int team_exists(server_config_t *s, char *team)
{
    for (int i = 0; s->nameX[i] != NULL; i++) {
        if (strcmp(s->nameX[i], team) == 0)
            return 0;
    }
    return 1;
}