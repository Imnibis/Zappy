/*
** EPITECH PROJECT, 2020
** B-YEP-410-LYN-4-1-zappy-arthur1.perrot
** File description:
** parser.h
*/

#ifndef PARSER_SERVER

#include <stdio.h>
#include <ctype.h>
#include <stdlib.h>
#include <string.h>

typedef struct server_config_s {
    int port;
    int width;
    int height;
    char **nameX;
    int clientsNb;
    int freq;
}server_config_t;

struct exec
{
    char *flag;
    int (*fct) (const char *, server_config_t *);
    int executed;
};

void handling_argument(int ac, char **argv, server_config_t *config);
int find_next_cmd(int index, char **args, int ac);
int setport(const char *port, server_config_t *config);
int setwidth(const char *width, server_config_t *config);
int setheight(const char *height, server_config_t *config);
int setclient(const char *nb, server_config_t *config);
int setfreq(const char *freq, server_config_t *config);
void setname(char **args, int index, server_config_t *config);
int cmd_done(struct exec tab[]);

#endif // !PARSER_SERVER