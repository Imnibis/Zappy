/*
** EPITECH PROJECT, 2020
** B-YEP-410-LYN-4-1-zappy-arthur1.perrot
** File description:
** utils.c
*/

#include "config.h"

int setclient(const char *nb, server_config_t *config)
{
    size_t i = 0;

    while (i != strlen(nb)) {
        if (!isdigit(nb[i])) {
            printf("error wrong clientNb must be number !\n");
            return 84;
        }
        i++;
    }
    config->clientsNb = atoi(nb);
    return 0;
}

int setfreq(const char *freq, server_config_t *config)
{
    size_t i = 0;

    while (i != strlen(freq)) {
        if (!isdigit(freq[i])) {
            printf("error wrong frequency !\n");
            return 84;
        }
        i++;
    }
    config->freq = atoi(freq);
    return 0;
}

void setname(char **args, int index, server_config_t *config)
{
    int p = index;
    int count = 0;

    for (count = 0; strcmp(args[index], "-c"); count++, index++);
    config->nameX = malloc(sizeof(char *) * count);
    index = p;
    p = 0;

    while (strcmp(args[index], "-c")) {
        config->nameX[p] = strdup(args[index]);
        p++;
        index++;
    }
}

int cmd_done(struct exec tab[])
{
    int i = 0;

    while (tab[i].flag != 0) {
        if (tab[i].executed != 1)
            return(84);
        i++;
    }

    return 0;
}