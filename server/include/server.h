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

typedef enum duration_s
{
    CONNECT = 0,
    DEATH = 1,
    INVENTORY = 2,
    FORWARD = 3,
    TURN = 4,
    LOOK = 5,
    EJECT = 6,
    TAKE = 7,
    SET = 8,
    REFILL = 9,
    FORK = 10,
    FOOD = 11,
    INCANTATION = 12
} duration_t;

typedef struct action_s action_t;
typedef struct player_s player_t;

typedef struct action_s
{
    clock_t begin;
    duration_t dur;
    player_t *player;
    char *elem;
    action_t *prev;
    action_t *next;
} action_t;

typedef enum type_s
{
    ANY = 0,
    AI = 1,
    CLIENT = 2
} type_t;

typedef enum dir_s
{
    NONE = 0,
    NORTH = 1,
    EAST = 2,
    SOUTH = 3,
    WEST = 4
} dir_t;

typedef struct player_s {
    int x;
    int y;
    int fd;
    int level;
    bool incantation;
    int inventory[7];
    int pos;
    char *team;
    dir_t dir;
    type_t type;
    player_t *next;
    player_t *prev;
} player_t;

typedef struct server_s {
    int sockid;
    int port;
    int gui_fd;
    struct sockaddr_in serv;
    socklen_t size;
    int nb_cli;
    int cli_max;
    int freq;
    action_t *acs;
    player_t *players;
} server_t;

void go_previous(server_t *s);
void go_pos(server_t *s, int pos);
void init_clients(server_t *s);
int create_server(server_t *s, server_config_t *info);
int serv_attribution(server_t *s);
void init_server(server_t *s, map_t *m, server_config_t *si);
char *get_next_line(int fd);
void map(map_t *m, int height, int width);
void send_map_gui(server_t *s, map_t *m);
void init_map(map_t *map, int height, int width);
void command_handling(server_t *s, map_t *m, char *command);
int team_exists(server_config_t *s, char *team);
void request_inventory(server_t *s, map_t *m, char *elem);
void forward(server_t *s, map_t *m, char *elem);
void left(server_t *s, map_t *m, char *elem);
void right(server_t *s, map_t *m, char *elem);
void look(server_t *s,  map_t *m, char *elem);
void broadcast(server_t *s,  map_t *m, char *elem);
void connect_nbr(server_t *s,  map_t *m, char *elem);
void forked(server_t *s,  map_t *m, char *elem);
void eject(server_t *s,  map_t *m, char *elem);
void take(server_t *s,  map_t *m, char *elem);
void set(server_t *s,  map_t *m, char *elem);
void incantation(server_t *s,  map_t *m, char *elem);
typedef void (*ai_cmd)(server_t *s, map_t *m, char *elem);
char **str_warray(char const *str, char f);
int get_durations(duration_t dur);
void actions(server_t *s, map_t *m);
#endif /* !SERVER_H_ */
