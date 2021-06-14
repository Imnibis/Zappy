/*
** EPITECH PROJECT, 2020
** B-YEP-410-LYN-4-1-zappy-arthur1.perrot
** File description:
** parser.c
*/

#include "include/config.h"


struct exec tab[] = {
    {'p', setport}, {'x', setwidth}, {'y', setheight}, {'c', setclient}, {'f', setfreq}, {0, }
};

int setheight(const char *height, server_config_t *config)
{
    size_t i = 0;

    while (i != strlen(height)) {
        if (!isdigit(height[i])) {
            printf("error wrong height !\n");
            return 84;
        }
        i++;
    }
    config->height = atoi(height);
    return 0;
}

int setwidth(const char *width, server_config_t *config)
{
    size_t i = 0;

    while (i != strlen(width)) {
        if (!isdigit(width[i])) {
            printf("error wrong width !\n");
            return 84;
        }
        i++;
    }
    config->width = atoi(width);
    return 0;
}

int setport(const char *port, server_config_t *config)
{
    size_t i = 0;

    while (i != strlen(port)) {
        if (!isdigit(port[i])) {
            printf("error wrong port !\n");
            return 84;
        }
        i++;
    }
    config->port = atoi(port);
    return 0;
}

int handling_argument(int ac, char **argv, server_config_t *config)
{
    int isError = 0;
    int p = 0;

    if (ac < 8)
        return 84;
    for (int i = 1; argv[i]; i++) {
        if ((i = find_next_cmd(i, argv, ac)) == 0)
            return 0;
        while (tab[p].flag != 0) {
            if (tab[p].flag == argv[i][1])
                isError = tab[p].fct(argv[i + 1], config);
            if (isError == 84)
                exit(84);
            p++;
        }
        if (!strcmp(argv[i], "-n"))
            setname(argv, i + 1, config);
        p = 0;
    }
    return 0;
}

int find_next_cmd(int index, char **args, int ac)
{
    while (args[index][0] != '-') {
        if (index == ac - 1)
            return 0;
        index++;
    }
    return index;
}
