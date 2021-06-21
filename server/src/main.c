/*
** EPITECH PROJECT, 2020
** B-YEP-410-LYN-4-1-zappy-arthur1.perrot
** File description:
** main.c
*/

#include "map.h"
#include "server.h"

int print_help()
{
    printf("USAGE: ./zappy_server -p port -x width -y height -n name1 name2 ");
    printf("... -c clientsNb -f freq\n");
    printf("\tport \t  is the port number\n");
    printf("\twidth \t  is the width of the world\n");
    printf("\theight \t  is the height of the world\n");
    printf("\tnameX \t  is the name of the team X\n");
    printf("\tclientsNb is the number of authorized clients per team\n");
    printf("\tfreq \t  is the reciprocal of time unit for execution of actions");

    return 0;
}

int main(int ac, char **argv)
{
    server_config_t server_info;
    server_t *s = NULL;

    if ((!strcmp(argv[1], "-help")) == 1)
        return print_help();
    handling_argument(ac, argv, &server_info);
    s = malloc(sizeof(server_t));
    if (create_server(s, &server_info) == 1) {
        free(s);
        return 84;
    }
    if (serv_attribution(s) == 1)
        return 84;
    init_server(s);
    return 0;
}
