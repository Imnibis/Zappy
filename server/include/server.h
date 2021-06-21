/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** server
*/

#ifndef SERVER_H_
#define SERVER_H_

#include <netinet/ip.h>
#include <sys/types.h>
#include <stdio.h>
#include <ctype.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <sys/time.h>
#include <unistd.h>
#include <string.h>
#include <sys/wait.h>
#include <sys/types.h>
#include <stdlib.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <uuid/uuid.h>
#include <time.h>
#include <signal.h>
#include <sys/stat.h>
#include <stdbool.h>

#include "map.h"
#include "config.h"

typedef struct player_s player_t;

typedef struct player_s {
    int x;
    int y;
    int fd;
    int level;
    bool incantation;
    int inventory[7];
    int pos;
    player_t *next;
    player_t *prev;
} player_t;

typedef struct server_s {
    int sockid;
    int port;
    struct sockaddr_in serv;
    socklen_t size;
    int nb_cli;
    int cli_max;
    player_t *players;
} server_t;

void go_previous(server_t *s);
void init_clients(server_t *s);
int create_server(server_t *s, server_config_t *info);
int serv_attribution(server_t *s);
void init_server(server_t *s, map_t *m);
char *get_next_line(int fd);
void request_inventory(server_t *s);
void map(map_t *m, int height, int width);
void send_map_gui(server_t *s, map_t *m);
void init_map(map_t *map, int height, int width);
#endif /* !SERVER_H_ */
