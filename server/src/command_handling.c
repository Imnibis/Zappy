/*
** EPITECH PROJECT, 2021
** zapp [WSL: Ubuntu]
** File description:
** command_handling
*/

#include "server.h"

void cmd_ai(server_t *s, map_t *m, char *command)
{
    char *ai_cmds[] = {"Forward", "Right", "Left", "Look", "Inventory", "Broadcast", "Connect_nbr", "Fork", "Eject", "Take", "Set", "Incantation", NULL};
    ai_cmd cmds[] = {&forward, &right, &left, &look, &request_inventory, &broadcast, &connect_nbr, &forked, &eject, &take, &set, &incantation};
    char **tab = str_warray(command, ' ');

    for (int i = 0; ai_cmds[i]; i++) {
        if (strcmp(tab[0], ai_cmds[i]) == 0)
            cmds[i](s, m, tab[1]);
    }
}

void command_handling(server_t *s, map_t *m,  char *command)
{
    if (s->players->type == AI)
        cmd_ai(s,m, command);
    // char *cli_cmd[] = {"msz", "bct", "mct", "tna", "ppo", "plv", "pin", "sgt", "sst", NULL};
}