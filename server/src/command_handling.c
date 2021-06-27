/*
** EPITECH PROJECT, 2021
** zapp [WSL: Ubuntu]
** File description:
** command_handling
*/

#include "server.h"

void cmd_ai(server_t *s, char *command)
{
    action_t *head = s->acs;
    char *ai_cmds[] = {"Connect", "UNUSED", "Inventory", "Forward", "Right", "Left", "Look", "Eject", "Take", "Set", "UNUSED", "Fork", "UNUSED", "Incantation", NULL};
    char **tab = str_warray(command, ' ');

    for (int i = 0; ai_cmds[i]; i++) {
        if (strcmp(tab[0], ai_cmds[i]) == 0) {
            for (; s->acs->next != NULL; s->acs = s->acs->next);
            s->acs->next = malloc(sizeof(action_t));
            s->acs->next->next = NULL;
            s->acs->next->begin = clock();
            s->acs->next->player = s->players;
            s->acs->next->dur = i;
            s->acs->next->prev = s->acs;
            if (tab[1] != NULL)
                s->acs->next->elem = strdup(tab[1]);
            s->acs = head;
        }
    }
}

void command_handling(server_t *s,  char *command)
{
    if (s->players->type == AI)
        cmd_ai(s, command);
}